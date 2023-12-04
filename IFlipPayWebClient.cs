using FlipPayApiLibrary.Models.Common;
using FlipPayApiLibrary.Models.Direct;
using FlipPayApiLibrary.Models.General;
using FlipPayApiLibrary.Models.Link;
using FlipPayApiLibrary.Models.Onboard;
using FlipPayApiLibrary.Models.PayLater;
using FlipPayApiLibrary.Models.PayNow;

namespace FlipPayApiLibrary
{
    public interface IFlipPayWebClient
    {
        Task CancelADirectFundingRequest(string prId);
        Task CancelAnOnboardingRequest(string onboardingId);
        Task CancelAPayLaterEnabledRequest(string prId);
        Task<DirectPostResponse?> CreateADirectFundingRequest(DirectPostRequest directPostRequest);
        Task<OnboardPostResponse?> CreateAnOnboardingRequest(OnboardPostRequest onboardPostRequest);
        Task<PayLaterPostResponse?> CreateAPayLaterEnabledRequest(PayLaterPostRequest payLaterPostRequest);
        Task<PayNowPostResponse?> CreateAPayNowEnabledRequest(PayNowPostRequest payNowPostRequest);
        Task DeleteAPayNowEnabledRequest(string prId);
        Task RemoveAnAccountLink(string merchantId);
        Task RequestAnAccountLink(LinkPostRequest linkPostRequest);
        Task<DirectGetResponse?> RetrieveADirectFundingRequest(string prId);
        Task<DirectGetListResponse?> RetrieveAListOfDirectFundingRequests(string queryParameters);
        Task<OnboardGetResponse?> RetrieveAnOnboardingRequest(string onboardingId);
        Task<PayLaterGetResponse?> RetrieveAPayLaterEnabledRequest(string prId);
        Task<PayNowGetResponse?> RetrieveAPayNowEnabledRequest(string prId);
        Task<GetBankAccountsResponse?> RetrieveBankAccounts(string merchantId);
        Task<GetProductsResponse?> RetrieveProductsOnAMerchantAccount(string merchantId);
        Task<LinkGetResponse?> RetrieveTheStatusOfAnAccountLink(string merchantId);
        Task UpdateADirectFundingRequest(string prId, List<ProductField> productFields);
        Task UpdateAPayLaterEnabledRequest(string prId, PayLaterPatchRequest payLaterPatchRequest);
    }
}