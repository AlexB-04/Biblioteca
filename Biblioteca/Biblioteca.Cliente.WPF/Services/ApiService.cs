namespace Biblioteca.Cliente.WPF.Services
{
    using Biblioteca.Cliente.WPF.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Text;

    public class ApiService
    {
        private string ObterMensagemErro(string result)
        {
            try
            {
                var erro = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                if (erro != null && erro.ContainsKey("Message"))
                {
                    return erro["Message"];
                }
            }
            catch (JsonException)
            {
                // A resposta não está em formato JSON.
            }

            return result;
        }

        public async Task<Response> GetCategorias(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result),
                    };
                }

                var categorias = JsonConvert.DeserializeObject<List<Categoria>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = categorias
                };
            }

            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response> PostCategoria(string urlBase, string controller, Categoria categoria)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                string json = JsonConvert.SerializeObject(categoria);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(controller, content);

                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var categoriaCriada = JsonConvert.DeserializeObject<Categoria>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = categoriaCriada
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> PutCategoria(string urlBase, string controller, Categoria categoria)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                string json = JsonConvert.SerializeObject(categoria);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{controller}/{categoria.IdCategoria}", content);

                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var categoriaAlterada = JsonConvert.DeserializeObject<Categoria>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = categoriaAlterada
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> DeleteCategoria(string urlBase, string controller, int id)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.DeleteAsync($"{controller}/{id}");

                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = ObterMensagemErro(result)
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}