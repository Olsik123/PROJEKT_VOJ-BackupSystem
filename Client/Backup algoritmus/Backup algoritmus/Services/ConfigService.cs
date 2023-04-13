using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Backup_algoritmus
{
    public class ConfigService
    {
        private HttpClient client;
        public ConfigService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://localhost:18788");
        } 

        public async Task<List<Configuration>> GetAllConfigs(int stationId)
        {

            string result = await this.client.GetStringAsync("/Configurations/GetByStationId/" + stationId);

            List<Configuration> data = JsonConvert.DeserializeObject<List<Configuration>>(result);

            return data;

        }
    }
}
