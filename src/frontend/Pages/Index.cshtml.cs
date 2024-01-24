using Azure.Data.Tables;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;
using System.Configuration;
using CafeReadConf.Frontend.Models;
using CafeReadConf.Frontend.Service;

namespace CafeReadConf.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string? Secret { get; set; }
        public List<UserEntity> Users { get; set; }
        public IUserService _userService { get; set; }

        public void OnGet(){}

        public IndexModel(ILogger<IndexModel> logger, 
        IUserService userService,
        IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;

            Secret = configuration.GetValue<string>("secret");

            ReadItems();
        }

        /// <summary>
        /// Read data from Azure Table Storage
        /// </summary>
        private async void ReadItems()
        {
            Users = await _userService.GetUsers();
        }

        // private string GetConfig(string key)
        // {
        //     string value =  System.Configuration.ConfigurationManager.AppSettings[key];
        //     if(string.IsNullOrEmpty(value))
        //     {
        //         value = Environment.GetEnvironmentVariable(key);
        //     }
        //     return value;
        // }
    }
}
