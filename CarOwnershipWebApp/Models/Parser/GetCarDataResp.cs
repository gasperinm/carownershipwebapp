using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Models.Parser
{
    public class GetCarDataResp
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CarData Data { get; set; }
    }
}
