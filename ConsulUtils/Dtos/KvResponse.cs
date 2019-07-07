using System;
using System.Text;

namespace ConsulUtils.Dtos
{
    public class KvResponse
    {
        public int CreateIndex { get; set; }
        public int ModifyIndex { get; set; }
        public int LockIndex { get; set; }
        public string Key { get; set; }
        public int Flags { get; set; }
        public string Value { get; set; }
        public string Session { get; set; }

        public string GetValue() => Encoding.UTF8.GetString(Convert.FromBase64String(Value));
    }
}
