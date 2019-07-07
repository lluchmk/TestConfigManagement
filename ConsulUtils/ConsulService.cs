using ConsulUtils.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsulUtils
{
    public class ConsulService : IConsulService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        private string consulAddress => _config.GetValue<string>("ConsulAddress");
        private string serviceId => _config.GetValue<string>("ServiceId");
        private string serviceName => _config.GetValue<string>("ServiceName");

        public ConsulService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IEnumerable<ConsulServiceNodeResponse>> GetServiceAddress(string serviceName)
        {
            var uri = $"{consulAddress}/v1/catalog/service/{serviceName}";
            var result = await _httpClient.GetAsync(uri);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Could read value");
            }

            return JsonConvert.DeserializeObject<IEnumerable<ConsulServiceNodeResponse>>(await result.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<KvResponse>> GetValue(string key)
        {
            var uri = $"{consulAddress}//v1/kv/{key}";
            var result = await _httpClient.GetAsync(uri);

            // TODO: Deal with 404
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Could read value");
            }

            return JsonConvert.DeserializeObject<IEnumerable<KvResponse>>(await result.Content.ReadAsStringAsync());
        }

        public async Task Register(string address, int port)
        {
            var registerRequest = new ConsulRegisterRequest
            {
                ID = serviceId,
                Name = serviceName,
                Address = address,
                Port = port
            };

            var uri = $"{consulAddress}/v1/agent/service/register";
            var serializedRequest = JsonConvert.SerializeObject(registerRequest);
            var request = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync(uri, request);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Could not register service");
            }
        }

        public async Task Deregister()
        {
            var uri = $"{consulAddress}/v1/agent/service/deregister/{serviceId}";
            var result = await _httpClient.PutAsync(uri, null);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Could not deregister service");
            }
        }
    }
}
