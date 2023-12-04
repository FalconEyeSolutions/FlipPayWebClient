using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using FlipPayApiLibrary.Models.Onboard;
using FlipPayApiLibrary.Models.General;
using FlipPayApiLibrary.Models.Link;
using FlipPayApiLibrary.Models.PayLater;
using FlipPayApiLibrary.Models.PayNow;
using FlipPayApiLibrary.Models.Direct;
using FlipPayApiLibrary.Models.Common;

namespace FlipPayApiLibrary;

// API Documentation: https://api-docs.flippay.com.au/v2.html
public class FlipPayWebClient
{
    private const string productionUrl = "https://app.flippay.com.au/api/v2";
    private const string demoUrl = "https://demo.flippay.com.au/api/v2";
    private readonly HttpClient _httpClient = new();
    private readonly string _baseUrl;
    private readonly ILogger _logger;

    public FlipPayWebClient(string token, bool isDemo, ILogger logger)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _baseUrl = isDemo ? demoUrl : productionUrl;
        _logger = logger;
    }

    #region Introducers

    #region Onboard

    /// <summary>
    /// Create an onboarding request for a merchant.
    /// </summary>
    /// <param name="onboardPostRequest"></param>
    /// <returns>OnboardPostResponse</returns>
    public async Task<OnboardPostResponse?> CreateAnOnboardingRequest(OnboardPostRequest onboardPostRequest)
    {
        return await PostAsync<OnboardPostResponse>($"{_baseUrl}/onboard", onboardPostRequest, nameof(CreateAnOnboardingRequest));
    }

    /// <summary>
    /// Retrieve the status of an onboarding request.
    /// </summary>
    /// <param name="onboardingId"></param>
    public async Task<OnboardGetResponse?> RetrieveAnOnboardingRequest(string onboardingId)
    {
        return await GetAsync<OnboardGetResponse>($"{_baseUrl}/onboard/{onboardingId}", nameof(RetrieveAnOnboardingRequest));
    }

    /// <summary>
    /// Cancel an onboarding request.
    /// </summary>
    /// <param name="onboardingId"></param>
    public async Task CancelAnOnboardingRequest(string onboardingId)
    {
        await DeleteAsync($"{_baseUrl}/onboard/{onboardingId}", nameof(CancelAnOnboardingRequest));
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
        _ = await PostAsync<object>($"{_baseUrl}/link", linkPostRequest, nameof(RequestAnAccountLink));
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
        return await GetAsync<LinkGetResponse>($"{_baseUrl}/link/{merchantId}", nameof(RetrieveTheStatusOfAnAccountLink));
    }

    /// <summary>
    /// Remove a link between an integrated partner and a merchant account.
    /// </summary>
    /// <param name="merchantId"></param>
    public async Task RemoveAnAccountLink(string merchantId)
    {
        await DeleteAsync($"{_baseUrl}/link/{merchantId}", nameof(RemoveAnAccountLink));
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
    public async Task<PayLaterPostResponse?> CreateAPayLaterEnabledRequest(PayLaterPostRequest payLaterPostRequest)
    {
        return await PostAsync<PayLaterPostResponse>($"{_baseUrl}/paylater", payLaterPostRequest, nameof(CreateAPayLaterEnabledRequest));
    }

    /// <summary>
    /// Update a payment request between a merchant and customer, enabled with "pay later" payment options
    /// Refer to the integration guide to confirm specific field format requirements(e.g.dates, currency, etc).
    /// </summary>
    /// <param name="prId"></param>
    /// <param name="payLaterPatchRequest"></param>
    public async Task UpdateAPayLaterEnabledRequest(string prId, PayLaterPatchRequest payLaterPatchRequest)
    {
        await PatchAsync($"{_baseUrl}/paylater/{prId}", payLaterPatchRequest, nameof(UpdateAPayLaterEnabledRequest));
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
        return await GetAsync<PayLaterGetResponse>($"{_baseUrl}/paylater/{prId}", nameof(RetrieveAPayLaterEnabledRequest));
    }

    /// <summary>
    /// Cancel a payment request between a merchant and customer, enabled with "pay later" payment options
    /// Refer to the integration guide to confirm specific field format requirements(e.g.dates, currency, etc).
    /// </summary>
    /// <param name="prId"></param>
    public async Task CancelAPayLaterEnabledRequest(string prId)
    {
        await DeleteAsync($"{_baseUrl}/paylater/{prId}", nameof(CancelAPayLaterEnabledRequest));
    }

    #endregion

    #region Pay Now

    /// <summary>
    /// Create a payment request between a merchant and customer, enabled with immediate card payment functionality only.
    /// </summary>
    /// <param name="payNowPostRequest"></param>
    /// <returns>PayNowPostResponse</returns>
    public async Task<PayNowPostResponse?> CreateAPayNowEnabledRequest(PayNowPostRequest payNowPostRequest)
    {
        return await PostAsync<PayNowPostResponse>($"{_baseUrl}/paynow", payNowPostRequest, nameof(CreateAPayNowEnabledRequest));
    }

    /// <summary>
    /// Retrieve a pay now enabled request
    /// </summary>
    /// <param name="prId"></param>
    /// <returns>PayNowGetResponse</returns>
    public async Task<PayNowGetResponse?> RetrieveAPayNowEnabledRequest(string prId)
    {
        return await GetAsync<PayNowGetResponse>($"{_baseUrl}/paynow/{prId}", nameof(RetrieveAPayNowEnabledRequest));
    }

    /// <summary>
    /// Cancel a payment request between a merchant and customer, enabled with immediate card payment functionality only.
    /// </summary>
    /// <param name="prId"></param>
    public async Task DeleteAPayNowEnabledRequest(string prId)
    {
        await DeleteAsync($"{_baseUrl}/paynow/{prId}", nameof(DeleteAPayNowEnabledRequest));
    }

    #endregion

    #region Direct

    /// <summary>
    /// Create a B2B funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="directPostRequest"></param>
    /// <returns>DirectPostResponse</returns>
    public async Task<DirectPostResponse?> CreateADirectFundingRequest(DirectPostRequest directPostRequest)
    {
        return await PostAsync<DirectPostResponse>($"{_baseUrl}/direct", directPostRequest, nameof(CreateADirectFundingRequest));
    }

    /// <summary>
    /// Update a direct funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be updated</param>
    /// <param name="productFields">Product fields to update on the PR</param>
    public async Task UpdateADirectFundingRequest(string prId, List<ProductField> productFields)
    {
        await PatchAsync($"{_baseUrl}/direct/{prId}", productFields, nameof(UpdateADirectFundingRequest));
    }

    /// <summary>
    /// Retrieve a direct funding request
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be retrieved.</param>
    /// <returns>DirectGetResponse</returns>
    public async Task<DirectGetResponse?> RetrieveADirectFundingRequest(string prId)
    {
        return await GetAsync<DirectGetResponse>($"{_baseUrl}/direct/{prId}", nameof(RetrieveADirectFundingRequest));
    }

    /// <summary>
    /// Cancel a B2B funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be cancelled.</param>
    public async Task CancelADirectFundingRequest(string prId)
    {
        await DeleteAsync($"{_baseUrl}/direct/{prId}", nameof(CancelADirectFundingRequest));
    }

    /// <summary>
    /// Retrieve a filtered list of direct funding requests
    /// - When authenticating as a merchant, no single parameter is mandatory, all are optional
    /// - When authenticating as an integrated partner, merchantId is mandatory(the service will only provide records for a single merchant)
    /// </summary>
    /// <param name="queryParameters">Query parameters to filter the list of direct funding requests</param>
    public async Task<DirectGetListResponse?> RetrieveAListOfDirectFundingRequests(string queryParameters)
    {
        return await GetAsync<DirectGetListResponse>($"{_baseUrl}/direct?{queryParameters}", nameof(RetrieveAListOfDirectFundingRequests));
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
        return await GetAsync<GetBankAccountsResponse>($"{_baseUrl}/bankaccounts/{merchantId}", nameof(RetrieveBankAccounts));
    }

    /// <summary>
    /// Retrieve products enabled on a merchant account.
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns>GetProductsResponse</returns>
    public async Task<GetProductsResponse?> RetrieveProductsOnAMerchantAccount(string merchantId)
    {
        return await GetAsync<GetProductsResponse>($"{_baseUrl}/products/{merchantId}", nameof(RetrieveProductsOnAMerchantAccount));
    }

    #endregion

    #endregion

    #region Helper Methods

    private const string contentType = "application/json";

    private async Task<T?> GetAsync<T>(string url, string methodName) where T : class
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
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

    private async Task<T?> PostAsync<T>(string url, object payload, string methodName) where T : class
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, contentType));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
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
            var response = await _httpClient.PatchAsync(url, new StringContent(jsonPayload, Encoding.UTF8, contentType));
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
            var response = await _httpClient.DeleteAsync(url);
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