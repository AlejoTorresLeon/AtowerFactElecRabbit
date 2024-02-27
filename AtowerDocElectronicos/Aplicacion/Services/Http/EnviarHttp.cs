﻿using AtowerDocElectronico.Aplicacion.Interfaces;
using System.Net.Http.Headers;

namespace AtowerDocElectronico.Aplicacion.Services.Http
{
    public class EnviarHttp:IEnviarHttp
    {
        public async Task<HttpResponseMessage> SendAsync(string url, HttpMethod method, HttpContent content, string? accessToken = null)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(method, url) { Content = content };

                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await client.SendAsync(request);
            }
        }
    }
}