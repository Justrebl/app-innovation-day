using Azure.Data.Tables;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;
using System.Configuration;
using CafeReadConf.Frontend.Models;
using CafeReadConf.Frontend.Service;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CafeReadConf.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string? Secret { get; set; }
        public List<UserEntity> Users { get; set; }
        public IUserService _userService { get; set; }

        public IndexModel(ILogger<IndexModel> logger,
        IUserService userService,
        IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;

            Secret = configuration.GetValue<string>("secret");
        }

        public async Task OnGetAsync()
        {

            Users = await ReadItems();
        }

        /// <summary>
        /// Read data from Azure Table Storage or the API based on the configuration
        /// </summary>
        private async Task<List<UserEntity>> ReadItems()
        {
            return await _userService.GetUsers();
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
