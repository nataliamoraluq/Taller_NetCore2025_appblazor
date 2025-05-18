using TallerNatBlazorApp.Components;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace TallerNatBlazorApp.Data
{
    public class Consumer
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        //public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        //{
        //    _sessionStorageService = sessionStorageService;
        //}

        public static HttpMethod MapearMetodo(methodHttp method)
        {
            switch (method)
            {
                case methodHttp.GET:
                    return HttpMethod.Get;
                case methodHttp.POST:
                    return HttpMethod.Post;
                case methodHttp.PUT:
                    return HttpMethod.Put;
                case methodHttp.DELETE:
                    return HttpMethod.Delete;
                default:
                    throw new NotImplementedException("Verbo http no implementado");
            }
        }
        public static async Task<Response<R>> Execute<R,T>(string endpoint, methodHttp methodHttp, T Data, string? token = null)
        {
            //string urlBaseApi = "http://gracosoftnet2025.runasp.net/api/";
            //string urlBaseApi = "https://localhost:7215/api/";
            //
            string urlBaseApi = "http://localhost:5025/";
            //
            //ojito aqui, con usar el local q es al correr la api
            //
            Response<R> response = new();
            try
            {
                // Instancia de la clase HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // URL
                    string url = @$"{urlBaseApi}{endpoint}";
                    //Data - informacion a mandar
                    string dataString = JsonConvert.SerializeObject(methodHttp != methodHttp.GET ? methodHttp != methodHttp.DELETE ? Data : "" : "");
                    var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(dataString));
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    //var content = new StringContent(dataString, Encoding.UTF8, "application/json");
                    // Hacer la peticion

                    //el tipo de peticion 
                    var request = new HttpRequestMessage(MapearMetodo(methodHttp), url)
                    {
                        Content = methodHttp != methodHttp.GET ? methodHttp != methodHttp.DELETE ? byteContent : null : null
                    }; 
                    //request.Content = byteContent;
                        if(token!=null)
                        //
                            client.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer", token);
                        //
                        using(HttpResponseMessage responseApi = await client.SendAsync(request))
                        {
                            using(HttpContent content = responseApi.Content)
                            {
                                response.StatusCode = responseApi.StatusCode.ToString();

                                string dataResponse = await content.ReadAsStringAsync();
                                if (dataResponse != null)
                                {
                                    try
                                    {
                                        response.Data = JsonConvert.DeserializeObject<R>(dataResponse);
                                        response.Ok = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        response.Ok = response.StatusCode != "400";
                                        //
                                        if(response.StatusCode == "InternalServerError" || response.StatusCode == "BadRequest")
                                            response.Ok = false;
                                        response.Message = dataResponse;
                                    }
                                }
                            }

                        };
                    
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public static async Task<Response<T>> Execute<T>(string endpoint, methodHttp methodHttp, T Data, string? token = null)
        {
            //string urlBaseApi = "http://gracosoftnet2025.runasp.net/api/";
            //string urlBaseApi = "https://localhost:7215/api/";
            //
            string urlBaseApi = "http://localhost:5025/";
            //
            //ojito aqui, con usar el local q es al correr la api
            //
            Response<T> response = new();
            try
            {
                // Instancia de la clase HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // URL
                    string url = @$"{urlBaseApi}{endpoint}";
                    //Data - informacion a mandar
                    string dataString = JsonConvert.SerializeObject(methodHttp != methodHttp.GET ? methodHttp != methodHttp.DELETE ? Data : "" : "");
                    var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(dataString));
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    if(token != null)
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);
                    //var content = new StringContent(dataString, Encoding.UTF8, "application/json");
                    // Hacer la peticion

                    //el tipo de peticion 
                    var request = new HttpRequestMessage(MapearMetodo(methodHttp), url)
                    {
                        Content = methodHttp != methodHttp.GET ? methodHttp != methodHttp.DELETE ? byteContent : null : null,
                    };
                    //request.Content = byteContent;

                    using (HttpResponseMessage responseApi = await client.SendAsync(request))
                    {
                        using (HttpContent content = responseApi.Content)
                        {
                            string dataResponse = await content.ReadAsStringAsync();
                            if (dataResponse != null)
                            {
                                try
                                {
                                    response.Data = JsonConvert.DeserializeObject<T>(dataResponse);
                                    response.Ok = true;
                                }
                                catch (Exception ex) {
                                    response.Message = dataResponse;
                                }
                            }
                            response.StatusCode = responseApi.StatusCode.ToString();
                        }

                    }
                    ;
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}