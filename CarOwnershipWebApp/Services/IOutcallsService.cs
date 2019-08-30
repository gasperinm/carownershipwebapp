using CarOwnershipWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public interface IOutcallsService
    {
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, string data);
    }
}
