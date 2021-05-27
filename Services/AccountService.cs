using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppBlazor.Data;

namespace WebAppBlazor.Services
{
    public interface IAccountService
    {
        KL_User User { get; }
        Task Initialize();
        Task Login(Login model);
        Task Logout();
        Task<IList<KL_User>> GetAll();
        Task<KL_User> GetByUserName(string username);
    }

    public class AccountService : IAccountService
    {
        private HttpClient client;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private string _userKey = "user";

        public KL_User User { get; private set; }

        public AccountService(
            HttpClient _client,
            NavigationManager navigationManager
        )
        {
            client = _client;
            _navigationManager = navigationManager;
        }

        public AccountService()
        {

        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<KL_User>(_userKey);
        }

        public async Task Login(Login model)
        {
            KL_User valid_user = new KL_User();
            try
            {
                var api_name = "http://localhost:5555/kl/login/check/";
                var httpResponse = await client.PostAsJsonAsync(api_name, model);

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    valid_user = JsonConvert.DeserializeObject<KL_User>(await httpResponse.Content.ReadAsStringAsync());

                    User = valid_user;
                    //await _localStorageService.SetItem<KL_User>(_userKey, valid_user);
                }
                else
                {
                    throw new Exception("Login Failed " + httpResponse.StatusCode + " : " + valid_user.username + " : " + valid_user.exo_name);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Login Failed \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        public async Task Logout()
        {
            User = null;
            //await _localStorageService.RemoveItem(_userKey);
            _navigationManager.NavigateTo("account/login");
        }

        public async Task<IList<KL_User>> GetAll()
        {
            var api_name = "http://localhost:5555/kl/login/getusers";
            var httpResponse = await client.GetAsync(api_name);

            if (httpResponse.IsSuccessStatusCode)
            {
                Users_Response responseData = JsonConvert.DeserializeObject<Users_Response>(await httpResponse.Content.ReadAsStringAsync());

                return responseData.Users;
            }

            return null;
        }

        public async Task<KL_User> GetByUserName(string username)
        {
            var api_name = $"http://localhost:5555/kl/login/getuser/{username}";
            var httpResponse = await client.GetAsync(api_name);

            KL_User user = JsonConvert.DeserializeObject<KL_User>(await httpResponse.Content.ReadAsStringAsync());

            return user;
        }
    }
}
