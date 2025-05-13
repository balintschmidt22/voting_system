using AutoMapper;
using Blazored.LocalStorage;
using VotingSystem.Shared.Models;
using System.Net.Http.Json;
using VotingSystem.Blazor.WebAssembly.Exception;
using VotingSystem.Blazor.WebAssembly.Infrastructure;
using VotingSystem.Blazor.WebAssembly.ViewModels;

namespace VotingSystem.Blazor.WebAssembly.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly IMapper _mapper;
        private readonly IHttpRequestUtility _httpRequestUtility;
        private readonly VotingSystemIndexDatabase _votingSystemIndexDatabase;


        public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorageService,
            IMapper mapper, IToastService toastService, IHttpRequestUtility httpRequestUtility,
            VotingSystemIndexDatabase votingSystemIndexDatabase) : base(toastService)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _localStorageService = localStorageService;
            _httpRequestUtility = httpRequestUtility;
            _votingSystemIndexDatabase = votingSystemIndexDatabase;
        }

        public async Task<bool> LoginAsync(LoginViewModel loginBindingViewModel)
        {
            var loginDto = _mapper.Map<LoginRequestDto>(loginBindingViewModel);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsJsonAsync("users/login", loginDto);
            }
            catch (System.Exception)
            {
                ShowErrorMessage("Unknown error occured");
                return false;
            }

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<LoginResponseDto>()
                    ?? throw new ArgumentNullException("Error with auth response.");

                await _localStorageService.SetItemAsStringAsync("AuthToken", responseBody.AuthToken);
                await _localStorageService.SetItemAsStringAsync("RefreshToken", responseBody.RefreshToken);
                await SetCurrentUserNameAsync(responseBody.UserId);
                await _localStorageService.SetItemAsStringAsync("Id", responseBody.UserId);

                return true;
            }
            else
            {
                await HandleHttpError(response);
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _httpRequestUtility.ExecutePostHttpRequestAsync("users/logout");
            }
            catch (HttpRequestException) { }

            var keys = new List<string>() { "AuthToken", "RefreshToken", "UserName" };
            await _localStorageService.RemoveItemsAsync(keys);
            await _votingSystemIndexDatabase.Votes.ClearStoreAsync();
        }

        public async Task<bool> TryAutoLoginAsync()
        {
            if (!(await _localStorageService.ContainKeyAsync("RefreshToken")))
                return false;
            
            try
            {
                await _httpRequestUtility.RedeemTokenAsync();
            }
            catch (HttpRequestErrorException)
            {
                var keys = new List<string>() { "AuthToken", "RefreshToken", "UserName" };
                await _localStorageService.RemoveItemsAsync(keys);
                return false;
            }
            return true;
        }

        public async Task<string?> GetCurrentlyLoggedInUserAsync()
        {
            return await _localStorageService.GetItemAsStringAsync("UserName");
        }

        private async Task SetCurrentUserNameAsync(string currentUserId)
        {
            var response = await _httpRequestUtility.ExecuteGetHttpRequestAsync<UserResponseDto>($"users/{currentUserId}");
            await _localStorageService.SetItemAsStringAsync("UserName", response.Response.UserName);
        }
    }
}
