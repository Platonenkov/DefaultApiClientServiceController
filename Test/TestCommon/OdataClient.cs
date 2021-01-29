using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DefaultApiClientService.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Simple.OData.Client;

namespace TestCommon
{
    public class OdataClient: ODataClient
    {
        public OdataClient(IConfiguration configuration) : base($"{new Uri(configuration["ClientAddress"])}api/")
        {
        }
    }
}
