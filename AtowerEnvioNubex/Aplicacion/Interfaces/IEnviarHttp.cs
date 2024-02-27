namespace AtowerEnvioNubex.Aplicacion.Interfaces
{
    public interface IEnviarHttp
    {
        Task<HttpResponseMessage> SendAsync(string url, HttpMethod method, HttpContent content, string accessToken = null);
    }
}
