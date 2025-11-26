using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeHubAPI.Application.Contracts.Responses
{
    public class NameResponse : StandardResponse
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
    }
}