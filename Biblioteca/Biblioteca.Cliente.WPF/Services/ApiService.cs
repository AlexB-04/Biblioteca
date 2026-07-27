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

                var json = JsonConvert.SerializeObject(categoria);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(controller, content);

                var result = await response.Content.ReadAsStringAsync();

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

                var json = JsonConvert.SerializeObject(categoria);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{controller}/{categoria.IdCategoria}", content);

                var result = await response.Content.ReadAsStringAsync();

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

                var result = await response.Content.ReadAsStringAsync();

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

        public async Task<Response> GetLivros(string urlBase, string controller)
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
                        Message = ObterMensagemErro(result)
                    };
                }

                var livros = JsonConvert.DeserializeObject<List<Livro>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = livros
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
        public async Task<Response> PostLivro(string urlBase, string controller, Livro livro)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var json = JsonConvert.SerializeObject(livro);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(controller, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var livroCriado = JsonConvert.DeserializeObject<Livro>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = livroCriado
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
        public async Task<Response> PutLivro(string urlBase, string controller, Livro livro)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var json = JsonConvert.SerializeObject(livro);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{controller}/{livro.IdLivro}", content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var livroAlterado = JsonConvert.DeserializeObject<Livro>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = livroAlterado
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

        public async Task<Response> DeleteLivro(string urlBase, string controller, int id)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.DeleteAsync($"{controller}/{id}");

                var result = await response.Content.ReadAsStringAsync();

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
                    Message = result
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

        public async Task<Response> GetUtilizadores(string urlBase, string controller)
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
                        Message = ObterMensagemErro(result)
                    };
                }

                var utilizadores = JsonConvert.DeserializeObject<List<Utilizador>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = utilizadores
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

        public async Task<Response> PostUtilizador(string urlBase, string controller, Utilizador utilizador)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var json = JsonConvert.SerializeObject(utilizador);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(controller, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var utilizadorCriado = JsonConvert.DeserializeObject<Utilizador>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = utilizadorCriado
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
        public async Task<Response> PutUtilizador(string urlBase, string controller, Utilizador utilizador)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var json = JsonConvert.SerializeObject(utilizador);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{controller}/{utilizador.IdUtilizador}", content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var utilizadorAlterado = JsonConvert.DeserializeObject<Utilizador>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = utilizadorAlterado
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
        public async Task<Response> DeleteUtilizador(string urlBase, string controller, int id)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.DeleteAsync($"{controller}/{id}");

                var result = await response.Content.ReadAsStringAsync();

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
                    Message = result
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
        public async Task<Response> GetEmprestimos(string urlBase, string controller)
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
                        Message = ObterMensagemErro(result)
                    };
                }

                var emprestimos = JsonConvert.DeserializeObject<List<Emprestimo>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = emprestimos
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
        public async Task<Response> PostEmprestimo(string urlBase, string controller, Emprestimo emprestimo)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var dadosEmprestimo = new
                {
                    emprestimo.IdUtilizador,
                    emprestimo.IdLivro
                };

                var json = JsonConvert.SerializeObject(dadosEmprestimo);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(controller, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var emprestimoCriado = JsonConvert.DeserializeObject<Emprestimo>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = emprestimoCriado
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
        public async Task<Response> PutEmprestimo(string urlBase, string controller, Emprestimo emprestimo)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var content = new StringContent("");

                var response = await client.PutAsync($"{controller}/{emprestimo.IdEmprestimo}", content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var emprestimoAlterado = JsonConvert.DeserializeObject<Emprestimo>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = emprestimoAlterado
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
        public async Task<Response> DeleteEmprestimo(string urlBase, string controller, int id)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.DeleteAsync($"{controller}/{id}");

                var result = await response.Content.ReadAsStringAsync();

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
                    Message = result
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

        public async Task<Response> GetReservas(string urlBase, string controller)
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
                        Message = ObterMensagemErro(result)
                    };
                }

                var reservas = JsonConvert.DeserializeObject<List<Reserva>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = reservas
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

        public async Task<Response> PostReserva(string urlBase, string controller, Reserva reserva)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var dadosReserva = new
                {
                    reserva.IdUtilizador,
                    reserva.IdLivro
                };

                var json = JsonConvert.SerializeObject(dadosReserva);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(controller, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var reservaCriada = JsonConvert.DeserializeObject<Reserva>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = reservaCriada
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

        public async Task<Response> PutReserva(string urlBase, string controller, Reserva reserva)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var content = new StringContent("");

                var response = await client.PutAsync($"{controller}/{reserva.IdReserva}", content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var reservaAlterada = JsonConvert.DeserializeObject<Reserva>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = reservaAlterada
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
        public async Task<Response> DeleteReserva(string urlBase, string controller, int id)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.DeleteAsync($"{controller}/{id}");

                var result = await response.Content.ReadAsStringAsync();

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
                    Message = result
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

        public async Task<Response> GetPenalizacoes(string urlBase, string controller)
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
                        Message = ObterMensagemErro(result)
                    };
                }

                var penalizacoes = JsonConvert.DeserializeObject<List<Penalizacao>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = penalizacoes
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

        public async Task<Response> PostPenalizacao(string urlBase, string controller, Penalizacao penalizacao)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var dadosPenalizacao = new
                {
                    penalizacao.IdUtilizador,
                    penalizacao.IdEmprestimo,
                    penalizacao.DiasAtraso,
                    penalizacao.Motivo
                };

                var json = JsonConvert.SerializeObject(dadosPenalizacao);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(controller, content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var penalizacaoCriada = JsonConvert.DeserializeObject<Penalizacao>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = penalizacaoCriada
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

        public async Task<Response> PutPenalizacao(string urlBase, string controller, Penalizacao penalizacao)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var content = new StringContent("");

                var response = await client.PutAsync(
                    $"{controller}/{penalizacao.IdPenalizacao}",
                    content);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = ObterMensagemErro(result)
                    };
                }

                var penalizacaoAlterada =
                    JsonConvert.DeserializeObject<Penalizacao>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = penalizacaoAlterada
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

        public async Task<Response> DeletePenalizacao(string urlBase, string controller, int id)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);

                var response = await client.DeleteAsync($"{controller}/{id}");

                var result = await response.Content.ReadAsStringAsync();

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
                    Message = result
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