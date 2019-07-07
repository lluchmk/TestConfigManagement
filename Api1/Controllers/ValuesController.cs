using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConsulUtils;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConsulService _consulService;

        public ValuesController(IConsulService consulService)
        {
            _consulService = consulService;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Response from Api1";
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<string>> GetValue(string key)
        {
            var value = await _consulService.GetValue(key);
            return $"Api1: Value from Consul kv store: {value.FirstOrDefault().GetValue()}";
        }

        [HttpGet("api2")]
        public async Task<ActionResult<string>> GetValueFromApi2()
        {
            var api2Address = (await _consulService.GetServiceAddress("api2")).First();
            var client = new HttpClient();
            var uri = $"http://{api2Address.ServiceAddress}:{api2Address.ServicePort}/api/values";

            var result = await client.GetAsync(uri);
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Error accessing api2 with uri {uri}");
            }

            var api2Value = await result.Content.ReadAsStringAsync();

            return $"Accessing API2 from API1: {api2Value}";
        }
    }
}
