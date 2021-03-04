using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace Global.Util
{
    public static class HttpHelper
    {
        public static Token GenerateToken(string uriBaseAdress)
        {
            using (var httpClient = new HttpClient())
            {
                Token token = new Token();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpClient.BaseAddress = new Uri(uriBaseAdress);
                var request = new HttpRequestMessage(HttpMethod.Post, "token");
                request.Content = new StringContent(string.Format("withCredentials", true));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var httpResponse = httpClient.SendAsync(request).Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = JToken.Parse(httpResponse.Content.ReadAsStringAsync().Result);

                    token = new Token()
                    {
                        AccessToken = response["access_token"].ToString(),
                        DataHoraVencimento = DateTime.Now.AddHours(40)///DateTime.Now.AddSeconds(int.Parse(response["expires_in"].ToString())).AddMinutes(-10)
                    };
                }
                else
                    throw new HttpRequestException(httpResponse.ReasonPhrase);
                return token;
            }


        }

        public static T Get<T>(string uriBaseAddress, string route, dynamic parameters = null, dynamic headerOptions = null, bool isXML = false)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(uriBaseAddress);

                if (parameters != null)
                {
                    var jsonParameters = JObject.Parse(JsonConvert.SerializeObject(parameters));

                    var paramsList = new List<string>();

                    foreach (JProperty prop in jsonParameters.Children())
                        paramsList.Add(prop.Name + "=" + prop.Value);

                    route += "?" + string.Join("&", paramsList);
                }

                var request = new HttpRequestMessage(HttpMethod.Get, route);


                if (headerOptions != null)
                {
                    var jsonParameters = JObject.Parse(JsonConvert.SerializeObject(headerOptions));

                    foreach (JProperty prop in jsonParameters.Children())
                        request.Headers.Add(prop.Name, prop.Value.ToString());

                }

                var httpResponse = httpClient.SendAsync(request).Result;

                var result = httpResponse.Content.ReadAsStringAsync().Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        if (!isXML)
                            return JToken.Parse(result).ToObject<T>();
                        else
                        {

                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(result);
                            string jsonText = JsonConvert.SerializeXmlNode(xml);
                            return JToken.Parse(jsonText).ToObject<T>();

                        }
                    }
                }
                else
                    throw new Exception(JToken.Parse(result)["Message"].ToString());

                return default(T);
            }
        }

        public static T Post<T>(string uriBaseAddress, string route, dynamic parameters = null, dynamic headerOptions = null)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(uriBaseAddress);
                var request = new HttpRequestMessage(HttpMethod.Post, route);

                foreach (var header in headerOptions)
                    request.Headers.Add(header.Key, header.Value);

                if (parameters != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(parameters));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                var httpResponse = httpClient.SendAsync(request).Result;

                var result = httpResponse.Content.ReadAsStringAsync().Result;

                if (httpResponse.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(result))
                        return JToken.Parse(result).ToObject<T>();
                }
                else
                    throw new Exception(JToken.Parse(result)["Message"].ToString());

                return default(T);
            }
        }

        public class Token
        {
            public string AccessToken { get; set; }
            public DateTime DataHoraVencimento { get; set; }
        }

    }
}
