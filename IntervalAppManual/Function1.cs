using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntervalAppManual
{
    public static class IntervalFunction
    {
        [FunctionName("IntervalFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Starting {DateTime.Now}");

            HttpClient client = new HttpClient();
            string auth = "xxxx" + ":" + "xxxxx";
            client.DefaultRequestHeaders.Add("Authorization", "Basic "+ System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth)));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            

            int[] peopleOnReport = { 293277 };
            foreach (int person in peopleOnReport)
            {
                log.Info($"https://api.myintervals.com/time/?personid={person.ToString()}");
                var responseString = await client.GetStringAsync($"https://api.myintervals.com/time/?personid={person.ToString()}");
                log.Info(responseString);
                //IList<DataItem> allItem = JsonConvert.DeserializeObject<IList<DataItem>>(myJsonArray);
                JObject obj = JObject.Parse(responseString);
                var times = obj.Value<JArray>("time");
                foreach(JObject time in times)
                {
                    log.Info($"time is {time.Value<String>("time")}");
                }
                var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(responseString);

                log.Info($"time1 is {obj[0].Value<String>("time")}");
                
            }




            //// parse query parameter
            //string name = req.GetQueryNameValuePairs()
            //    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            //    .Value;

            //if (name == null)
            //{
            //    // Get request body
            //    dynamic data = await req.Content.ReadAsAsync<object>();
            //    name = data?.name;
            //}

            //return name == null ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")

            //    : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);

            return req.CreateResponse(HttpStatusCode.OK, "worked");
        }
    }
}
