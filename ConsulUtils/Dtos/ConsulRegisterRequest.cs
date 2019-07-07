using System;
using System.Collections.Generic;
using System.Text;

namespace ConsulUtils.Dtos
{
    public class ConsulRegisterRequest
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }

    }
}
