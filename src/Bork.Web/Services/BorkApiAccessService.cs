using Microsoft.Extensions.Configuration;
using NLog;
using System.Net.Http;
using System;
using System.Text;
using Bork.Contracts;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bork.Web.Services
{
    public class BorkApiAccessService : IBorkApiAccessService
    {
        private HttpClient _httpClient;

        public BorkApiAccessService(ILogger logger,
            IConfiguration config)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(config["BorkApiAddress"])
            };
        }

        public async Task<IList<BorkRecord>> GetTopBorks()
        {
            var resp = await _httpClient.GetAsync("/bork");
            var respString = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<BorkRecord>>(respString);
        }

        public async Task<BorkRecord> GetBorkById(int id)
        {
            var resp = await _httpClient.GetAsync($"/bork/{id}");
            var respString = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BorkRecord>(respString);
        }

        public async Task<IList<ReBorkRecord>> GetTopReBorks()
        {
            var resp = await _httpClient.GetAsync("/rebork");
            var respString = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<ReBorkRecord>>(respString);
        }

        public async Task<BorkRecord> CreateBork(BorkRecord bork)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(bork),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/bork", content);
            var respString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BorkRecord>(respString);
        }

        public async Task<ReBorkRecord> CreateReBork(ReBorkRecord reBork)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(reBork),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("/rebork", content);
            var respString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ReBorkRecord>(respString);
        }

        public async Task<BorkStats> GetBorkStats(string username)
        {
            var resp = await _httpClient.GetAsync($"/stats/{username}");
            var respString = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BorkStats>(respString);
        }
    }
}
