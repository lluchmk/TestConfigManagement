using ConsulUtils.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsulUtils
{
    public interface IConsulService
    {
        Task Register(string address, int port);

        Task Deregister();

        Task<IEnumerable<ConsulServiceNodeResponse>> GetServiceAddress(string serviceName);

        Task<IEnumerable<KvResponse>> GetValue(string key);
    }
}
