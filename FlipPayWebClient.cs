using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using FlipPayApiLibrary.Models.Onboard;
using FlipPayApiLibrary.Models.General;
using FlipPayApiLibrary.Models.Link;
using FlipPayApiLibrary.Models.PayLater;
using FlipPayApiLibrary.Models.PayNow;
using FlipPayApiLibrary.Models.Direct;
using FlipPayApiLibrary.Models.Common;
using System.Net.Http.Headers;

namespace FlipPayApiLibrary;

// API Documentation: https://api-docs.flippay.com.au/v2.html
public class FlipPayWebClient : IFlipPayWebClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FlipPayWebClient> _logger;

    #region Constructors

    public FlipPayWebClient(HttpClient httpClient, ILogger<FlipPayWebClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public FlipPayWebClient(FlipPayConfig config, ILogger<FlipPayWebClient> logger)
    {
        _httpClient = new();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            config.Token
        );
        _httpClient.BaseAddress = new Uri(config.IsDemo ? config.DemoUrl : config.ProductionUrl);
        _logger = logger;
    }

    #endregion

    #region Introducers

    #region Onboard

    /// <summary>
    /// Create an onboarding request for a merchant.
    /// </summary>
    /// <param name="onboardPostRequest"></param>
    /// <returns>OnboardPostResponse</returns>
    public async Task<OnboardPostResponse?> CreateAnOnboardingRequest(
        OnboardPostRequest onboardPostRequest
    )
    {
        return await PostAsync<OnboardPostResponse>(
            $"onboard",
            onboardPostRequest,
            nameof(CreateAnOnboardingRequest)
        );
    }

    /// <summary>
    /// Retrieve the status of an onboarding request.
    /// </summary>
    /// <param name="onboardingId"></param>
    public async Task<OnboardGetResponse?> RetrieveAnOnboardingRequest(string onboardingId)
    {
        return await GetAsync<OnboardGetResponse>(
            $"onboard/{onboardingId}",
            nameof(RetrieveAnOnboardingRequest)
        );
    }

    /// <summary>
    /// Cancel an onboarding request.
    /// </summary>
    /// <param name="onboardingId"></param>
    public async Task CancelAnOnboardingRequest(string onboardingId)
    {
        await DeleteAsync($"onboard/{onboardingId}", nameof(CancelAnOnboardingRequest));
    }

    #endregion

    #region Link

    /// <summary>
    /// Request a link between an integrated partner and a merchant account. Notifications received (if enabled) are for the linked and not-linked statuses
    /// - note that if accounts are linked, then one party removes the link some time later, a not-linked notification will be sent to the original webhook url.
    /// </summary>
    /// <param name="linkPostRequest"></param>
    public async Task RequestAnAccountLink(LinkPostRequest linkPostRequest)
    {
        _ = await PostAsync<object>($"link", linkPostRequest, nameof(RequestAnAccountLink));
    }

    /// <summary>
    /// Retrieve the status of a link between an integrated partner and a merchant account. Statuses returned are:
    ///     - pending: link request has been sent but not actioned by the merchant
    ///     - linked: accounts are linked
    ///     - not-linked: accounts are not linked
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns>LinkGetResponse</returns>
    public async Task<LinkGetResponse?> RetrieveTheStatusOfAnAccountLink(string merchantId)
    {
        return await GetAsync<LinkGetResponse>(
            $"link/{merchantId}",
            nameof(RetrieveTheStatusOfAnAccountLink)
        );
    }

    /// <summary>
    /// Remove a link between an integrated partner and a merchant account.
    /// </summary>
    /// <param name="merchantId"></param>
    public async Task RemoveAnAccountLink(string merchantId)
    {
        await DeleteAsync($"link/{merchantId}", nameof(RemoveAnAccountLink));
    }

    #endregion

    #endregion

    #region Merchant

    #region Pay Later

    /// <summary>
    /// Create a payment request between a merchant and customer, enabled with "pay later" payment options. Refer to the integration guide for specific products to confirm product field format requirements (e.g. dates, currency, etc).
    /// </summary>
    /// <param name="payLaterPostRequest"></param>
    /// <returns>PayLaterPostResponse</returns>
    public async Task<PayLaterPostResponse?> CreateAPayLaterEnabledRequest(
        PayLaterPostRequest payLaterPostRequest
    )
    {
        return await PostAsync<PayLaterPostResponse>(
            $"paylater",
            payLaterPostRequest,
            nameof(CreateAPayLaterEnabledRequest)
        );
    }

    /// <summary>
    /// Update a payment request between a merchant and customer, enabled with "pay later" payment options
    /// Refer to the integration guide to confirm specific field format requirements(e.g.dates, currency, etc).
    /// </summary>
    /// <param name="prId"></param>
    /// <param name="payLaterPatchRequest"></param>
    public async Task UpdateAPayLaterEnabledRequest(
        string prId,
        PayLaterPatchRequest payLaterPatchRequest
    )
    {
        await PatchAsync(
            $"paylater/{prId}",
            payLaterPatchRequest,
            nameof(UpdateAPayLaterEnabledRequest)
        );
    }

    /// <summary>
    /// Retrieve a payment request created between a merchant and customer, enabled with "pay later" payment options.
    /// Refer to the integration guide to confirm specific field format requirements(e.g.dates, currency, etc).
    /// Note that if a payment request has not yet been activated, and was enabled with multiple products to offer,
    /// multiple products will be returned.If a payment request has been activated, only the product that was approved will be returned.
    /// </summary>
    /// <param name="prId"></param>"
    /// <returns>PayLaterGetResponse</returns>
    public async Task<PayLaterGetResponse?> RetrieveAPayLaterEnabledRequest(string prId)
    {
        return await GetAsync<PayLaterGetResponse>(
            $"paylater/{prId}",
            nameof(RetrieveAPayLaterEnabledRequest)
        );
    }

    /// <summary>
    /// Cancel a payment request between a merchant and customer, enabled with "pay later" payment options
    /// Refer to the integration guide to confirm specific field format requirements(e.g.dates, currency, etc).
    /// </summary>
    /// <param name="prId"></param>
    public async Task CancelAPayLaterEnabledRequest(string prId)
    {
        await DeleteAsync($"paylater/{prId}", nameof(CancelAPayLaterEnabledRequest));
    }

    #endregion

    #region Pay Now

    /// <summary>
    /// Create a payment request between a merchant and customer, enabled with immediate card payment functionality only.
    /// </summary>
    /// <param name="payNowPostRequest"></param>
    /// <returns>PayNowPostResponse</returns>
    public async Task<PayNowPostResponse?> CreateAPayNowEnabledRequest(
        PayNowPostRequest payNowPostRequest
    )
    {
        return await PostAsync<PayNowPostResponse>(
            $"paynow",
            payNowPostRequest,
            nameof(CreateAPayNowEnabledRequest)
        );
    }

    /// <summary>
    /// Retrieve a pay now enabled request
    /// </summary>
    /// <param name="prId"></param>
    /// <returns>PayNowGetResponse</returns>
    public async Task<PayNowGetResponse?> RetrieveAPayNowEnabledRequest(string prId)
    {
        return await GetAsync<PayNowGetResponse>(
            $"paynow/{prId}",
            nameof(RetrieveAPayNowEnabledRequest)
        );
    }

    /// <summary>
    /// Cancel a payment request between a merchant and customer, enabled with immediate card payment functionality only.
    /// </summary>
    /// <param name="prId"></param>
    public async Task DeleteAPayNowEnabledRequest(string prId)
    {
        await DeleteAsync($"paynow/{prId}", nameof(DeleteAPayNowEnabledRequest));
    }

    #endregion

    #region Direct

    /// <summary>
    /// Create a B2B funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="directPostRequest"></param>
    /// <returns>DirectPostResponse</returns>
    public async Task<DirectPostResponse?> CreateADirectFundingRequest(
        DirectPostRequest directPostRequest
    )
    {
        return await PostAsync<DirectPostResponse>(
            $"direct",
            directPostRequest,
            nameof(CreateADirectFundingRequest)
        );
    }

    /// <summary>
    /// Update a direct funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be updated</param>
    /// <param name="productFields">Product fields to update on the PR</param>
    public async Task UpdateADirectFundingRequest(string prId, List<ProductField> productFields)
    {
        await PatchAsync($"direct/{prId}", productFields, nameof(UpdateADirectFundingRequest));
    }

    /// <summary>
    /// Retrieve a direct funding request
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be retrieved.</param>
    /// <returns>DirectGetResponse</returns>
    public async Task<DirectGetResponse?> RetrieveADirectFundingRequest(string prId)
    {
        return await GetAsync<DirectGetResponse>(
            $"direct/{prId}",
            nameof(RetrieveADirectFundingRequest)
        );
    }

    /// <summary>
    /// Cancel a B2B funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be cancelled.</param>
    public async Task CancelADirectFundingRequest(string prId)
    {
        await DeleteAsync($"direct/{prId}", nameof(CancelADirectFundingRequest));
    }

    /// <summary>
    /// Retrieve a filtered list of direct funding requests
    /// - When authenticating as a merchant, no single parameter is mandatory, all are optional
    /// - When authenticating as an integrated partner, merchantId is mandatory(the service will only provide records for a single merchant)
    /// </summary>
    /// <param name="queryParameters">Query parameters to filter the list of direct funding requests</param>
    public async Task<DirectGetListResponse?> RetrieveAListOfDirectFundingRequests(
        string queryParameters
    )
    {
        return await GetAsync<DirectGetListResponse>(
            $"direct?{queryParameters}",
            nameof(RetrieveAListOfDirectFundingRequests)
        );
    }

    #endregion

    #region General

    /// <summary>
    /// Retrieve bank accounts enabled on a merchant account
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns>GetBankAccountsResponse</returns>
    public async Task<GetBankAccountsResponse?> RetrieveBankAccounts(string merchantId)
    {
        return await GetAsync<GetBankAccountsResponse>(
            $"bankaccounts/{merchantId}",
            nameof(RetrieveBankAccounts)
        );
    }

    /// <summary>
    /// Retrieve products enabled on a merchant account.
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns>GetProductsResponse</returns>
    public async Task<GetProductsResponse?> RetrieveProductsOnAMerchantAccount(string merchantId)
    {
        return await GetAsync<GetProductsResponse>(
            $"products/{merchantId}",
            nameof(RetrieveProductsOnAMerchantAccount)
        );
    }

    #endregion

    #endregion

    #region Helper Methods

    private const string contentType = "application/json";

    private async Task<T?> GetAsync<T>(string url, string methodName)
        where T : class
    {
        try
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(content);
        }
        catch (HttpRequestException e)
        {
            HandleError(e, $"Error fetching data in {methodName}: {e.Message}");
        }
        catch (JsonException ex)
        {
            HandleError(ex, $"Error using JSON in {methodName}: {ex.Message}");
        }
        catch (Exception ex)
        {
            HandleError(ex, $"Unexpected error in {methodName}: {ex.Message}");
        }

        return null;
    }

    private async Task<T?> PostAsync<T>(string url, object payload, string methodName)
        where T : class
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            var response = await _httpClient
                .PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, contentType))
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(content);
        }
        catch (HttpRequestException e)
        {
            HandleError(e, $"Error posting data in {methodName}: {e.Message}");
        }
        catch (JsonException ex)
        {
            HandleError(ex, $"Error using JSON in {methodName}: {ex.Message}");
        }
        catch (Exception ex)
        {
            HandleError(ex, $"Unexpected error in {methodName}: {ex.Message}");
        }

        return null;
    }

    private async Task PatchAsync(string url, object payload, string methodName)
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            var response = await _httpClient
                .PatchAsync(url, new StringContent(jsonPayload, Encoding.UTF8, contentType))
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation($"{methodName} successfully executed.");
        }
        catch (HttpRequestException e)
        {
            HandleError(e, $"Error patching data in {methodName}: {e.Message}");
        }
        catch (JsonException ex)
        {
            HandleError(ex, $"Error using JSON in {methodName}: {ex.Message}");
        }
        catch (Exception ex)
        {
            HandleError(ex, $"Unexpected error in {methodName}: {ex.Message}");
        }
    }

    private async Task DeleteAsync(string url, string methodName)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation($"{methodName} successfully executed.");
        }
        catch (HttpRequestException e)
        {
            HandleError(e, $"Error deleting data in {methodName}: {e.Message}");
        }
        catch (Exception ex)
        {
            HandleError(ex, $"Unexpected error in {methodName}: {ex.Message}");
        }
    }

    private void HandleError(Exception ex, string message)
    {
        _logger.LogError(message);
    }

    #endregion
}
