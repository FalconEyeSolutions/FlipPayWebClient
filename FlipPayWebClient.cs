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
        var url = $"{_baseUrl}/onboard";

        var jsonPayload = JsonSerializer.Serialize(onboardPostRequest);

        try
        {
            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OnboardPostResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error requesting onboarding: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Retrieve the status of an onboarding request.
    /// </summary>
    /// <param name="onboardingId"></param>
    public async Task<OnboardGetResponse?> RetrieveAnOnboardingRequest(
        string onboardingId
    )
    {
        var url = $"{_baseUrl}/onboard/{onboardingId}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OnboardGetResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching onboarding status: {e.Message}");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Cancel an onboarding request.
    /// </summary>
    /// <param name="onboardingId"></param>
    public async Task CancelAnOnboardingRequest(string onboardingId)
    {
        var url = $"{_baseUrl}/onboard/{onboardingId}";

        try
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Onboarding request successfully cancelled.");
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Failed to cancel onboarding request. Error: {e.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while cancelling the onboarding request: {ex.Message}");
        }
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
        var url = $"{_baseUrl}/link";
        var jsonPayload = JsonSerializer.Serialize(linkPostRequest);

        try
        {
            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Account link request sent");
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error requesting account link: {e.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex.Message}");
        }
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
        var url = $"{_baseUrl}/link/{merchantId}";

        _logger.LogInformation($"Retrieving link status for merchant {merchantId}...");

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkGetResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching link status: {e.Message}");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Remove a link between an integrated partner and a merchant account.
    /// </summary>
    /// <param name="merchantId"></param>
    public async Task RemoveAnAccountLink(string merchantId)
    {
        var url = $"{_baseUrl}/link/{merchantId}";

        try
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Link successfully removed.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Failed to remove link. Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while removing the link: {ex.Message}");
        }
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
        var url = $"{_baseUrl}/paylater";

        var jsonPayload = JsonSerializer.Serialize(payLaterPostRequest);

        try
        {
            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PayLaterPostResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error requesting payment request: {e.Message}");
            return null;
        }
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
        var url = $"{_baseUrl}/paylater/{prId}";

        var jsonPayload = JsonSerializer.Serialize(payLaterPatchRequest);

        try
        {
            var response = await _httpClient.PatchAsync(
                url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Pay Later payment successfully updated.");
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error updating payment request: {e.Message}");
        }
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
        var url = $"{_baseUrl}/paylater/{prId}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PayLaterGetResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching payment request: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Cancel a payment request between a merchant and customer, enabled with "pay later" payment options
    /// Refer to the integration guide to confirm specific field format requirements(e.g.dates, currency, etc).
    /// </summary>
    /// <param name="prId"></param>
    public async Task CancelAPayLaterEnabledRequest(string prId)
    {
        var url = $"{_baseUrl}/paylater/{prId}";

        try
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Pay Later payment successfully cancelled.");
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error cancelling payment request: {e.Message}");
        }
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
        var url = $"{_baseUrl}/paynow";

        var jsonPayload = JsonSerializer.Serialize(payNowPostRequest);

        try
        {
            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PayNowPostResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error requesting payment request: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Retrieve a pay now enabled request
    /// </summary>
    /// <param name="prId"></param>
    /// <returns>PayNowGetResponse</returns>
    public async Task<PayNowGetResponse?> RetrieveAPayNowEnabledRequest(string prId)
    {
        var url = $"{_baseUrl}/paynow/{prId}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PayNowGetResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching payment request: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Cancel a payment request between a merchant and customer, enabled with immediate card payment functionality only.
    /// </summary>
    /// <param name="prId"></param>
    public async Task DeleteAPayNowEnabledRequest(string prId)
    {
        var url = $"{_baseUrl}/paynow/{prId}";

        try
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Pay Now payment request successfully cancelled.");
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error cancelling payment request: {e.Message}");
        }
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
        var url = $"{_baseUrl}/direct";

        var jsonPayload = JsonSerializer.Serialize(directPostRequest);

        try
        {
            var response = await _httpClient.PostAsync(
                url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DirectPostResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error requesting payment request: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Update a direct funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be updated</param>
    /// <param name="productFields">Product fields to update on the PR</param>
    public async Task UpdateADirectFundingRequest(string prId, List<ProductField> productFields)
    {
        var url = $"{_baseUrl}/direct/{prId}";

        var jsonPayload = JsonSerializer.Serialize(productFields);

        try
        {
            var response = await _httpClient.PatchAsync(
                url,
                new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            );
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Direct funding request successfully updated.");
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error updating payment request: {e.Message}");
        }
    }

    /// <summary>
    /// Retrieve a direct funding request
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be retrieved.</param>
    /// <returns>DirectGetResponse</returns>
    public async Task<DirectGetResponse?> RetrieveADirectFundingRequest(string prId)
    {
        var url = $"{_baseUrl}/direct/{prId}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DirectGetResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching payment request: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Cancel a B2B funding request between an onboarded entity and FlipPay.
    /// </summary>
    /// <param name="prId">The unique ID of the payment request to be cancelled.</param>
    public async Task CancelADirectFundingRequest(string prId)
    {
        var url = $"{_baseUrl}/direct/{prId}";

        try
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Direct funding request successfully cancelled.");
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error cancelling payment request: {e.Message}");
        }
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
        var url = $"{_baseUrl}/direct?{queryParameters}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<DirectGetListResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching payment request: {e.Message}");
            return null;
        }
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
        var url = $"{_baseUrl}/bankaccounts/{merchantId}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetBankAccountsResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching accounts: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Retrieve products enabled on a merchant account.
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns>GetProductsResponse</returns>
    public async Task<GetProductsResponse?> RetrieveProductsOnAMerchantAccount(string merchantId)
    {
        var url = $"{_baseUrl}/products/{merchantId}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetProductsResponse>(content);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError($"Error fetching products: {e.Message}");
            return null;
        }
    }

    #endregion

    #endregion
}
