using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;




namespace Backup_algoritmus
{
    public class ClientService
    {
        private HttpClient client;

        public ClientService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://localhost:18788");

        }
        public async Task<int> GetId()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            var macAddr =
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();

            string url = @"/Stations/CheckID/"+localIP+ @"/"+ macAddr;
            string result = await this.client.GetStringAsync(url);
            Stations data = JsonConvert.DeserializeObject<Stations>(result);


            if (data == null)
                return -100;
            else
                return data.Id;

          
        }

        public async Task Create()
        {
           string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            var macAddr =
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();
            string strComputerName = Environment.MachineName.ToString();

            Station station = new Station() { mac=macAddr, ip = localIP, alias =strComputerName};
            
            await this.client.PostAsJsonAsync("/Stations/CreateOne", station);
        }
    }
}
