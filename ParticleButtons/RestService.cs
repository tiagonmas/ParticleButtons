using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Xamarin.Forms;
using System.Threading.Tasks;
using ParticleButtons.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;


namespace ParticleButtons
{
    public class RestService
    {
        System.Net.Http.HttpClient client;
        private const string baseUrl = "https://api.particle.io/v1/devices/{0}/{1}";
        public RestService()
        {

            client = new HttpClient();

        }

        public async Task<ParticleFunctionReturn> CallParticleFunction(ParticleFunction pFunc)
        {
            ParticleFunctionReturn pfRet = new ParticleFunctionReturn();

            try
            {
            
                Uri uri = new Uri(string.Format(baseUrl, pFunc.DeviceId, pFunc.FuncName));
                Uri pfuri = new Uri(string.Format(baseUrl, pFunc.DeviceId, pFunc.FuncName));

                FormUrlEncodedContent postData = new FormUrlEncodedContent(
                   new List<KeyValuePair<string, string>>
                    {
                                        new KeyValuePair<string, string>("arg", pFunc.Args)

                    }
                );
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = postData,
                    RequestUri = pfuri
                };

                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", pFunc.Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string content = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    Analytics.TrackEvent("response.Ok");
                    
                    pfRet = JsonConvert.DeserializeObject<ParticleFunctionReturn>(content);
                    pfRet.Error = false;
                }
                else
                {
                    Analytics.TrackEvent("response.Error");
                    pfRet.Error = true;
                    //var ErrResult = response.Content.ReadAsStringAsync().Result;
                    if (!String.IsNullOrEmpty(content))
                    {
                        ParticleFunctionError pfErr = JsonConvert.DeserializeObject<ParticleFunctionError>(content);
                        if (String.IsNullOrEmpty(pfErr.info))
                        {
                            pfRet.ErrorDetail = pfErr.error;
                        }
                        else
                        {
                            pfRet.ErrorDetail = pfErr.error + " : " + pfErr.info;
                        }
                        
                    }

                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);               
            }

            return pfRet;
        }

    }
}
