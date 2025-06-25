using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Shared
{
    [MemoryPackable]
    public partial class LoginRequest
    {
        public long TestValue01 { get; set; } = 0;
        public string TestValue02 { get; set; } = string.Empty;
    }

    [MemoryPackable]
    public partial class LoginResponse
    {
        public int TestValue01 { get; set; } = 0;

        public long TestValue02 { get; set; } = 0;
    }
}
