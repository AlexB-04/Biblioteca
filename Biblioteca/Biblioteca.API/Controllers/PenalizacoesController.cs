namespace Biblioteca.API.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    public class PenalizacoesController : ApiController
    {
        private readonly DataClasses1DataContext dc =
            new DataClasses1DataContext(
                System.Configuration.ConfigurationManager
                .ConnectionStrings["BibliotecaDBConnectionString"]
                .ConnectionString);

        // GET api/penalizacoes
        public IHttpActionResult Get()
        {
            var penalizacoes = dc.Penalizacoes
                .Select(p => new
                {
                    p.IdPenalizacao,
                    p.IdUtilizador,
                    Utilizador = p.Utilizador.Nome,
                    p.IdEmprestimo,
                    p.Valor,
                    p.DiasAtraso,
                    p.Motivo,
                    p.DataPenalizacao,
                    p.Pago,
                    p.DataPagamento
                })
                .ToList();

            return Ok(penalizacoes);
        }

        // GET api/penalizacoes/1
        public IHttpActionResult Get(int id)
        {
            var penalizacao = dc.Penalizacoes.Where(p => p.IdPenalizacao == id)
                .Select(p => new
                {
                    p.IdPenalizacao,
                    p.IdUtilizador,
                    Utilizador = p.Utilizador.Nome,
                    p.IdEmprestimo,
                    p.Valor,
                    p.DiasAtraso,
                    p.Motivo,
                    p.DataPenalizacao,
                    p.Pago,
                    p.DataPagamento
                })
                .FirstOrDefault();

            if (penalizacao == null)
            {
                return NotFound();
            }

            return Ok(penalizacao);
        }

        // POST api/penalizacoes
        public IHttpActionResult Post([FromBody] Penalizacoes penalizacao)
        {
            if (penalizacao == null)
            {
                return BadRequest("Dados da penalização inválidos.");
            }

            Utilizador utilizador = dc.Utilizadors.FirstOrDefault(u => u.IdUtilizador == penalizacao.IdUtilizador);

            if (utilizador == null)
            {
                return BadRequest("O utilizador indicado não existe.");
            }

            if (penalizacao.DiasAtraso <= 0)
            {
                return BadRequest("Os dias de atraso devem ser superiores a zero.");
            }

            if (string.IsNullOrWhiteSpace(penalizacao.Motivo))
            {
                return BadRequest("O motivo da penalização é obrigatório.");
            }

            Emprestimo emprestimo = null;

            if (penalizacao.IdEmprestimo.HasValue)
            {
                emprestimo = dc.Emprestimos.FirstOrDefault(e => e.IdEmprestimo == penalizacao.IdEmprestimo.Value);

                if (emprestimo == null)
                {
                    return BadRequest("O empréstimo indicado não existe.");
                }

                if (emprestimo.IdUtilizador != penalizacao.IdUtilizador)
                {
                    return BadRequest("O empréstimo não pertence ao utilizador indicado.");
                }

                bool penalizacaoExistente = dc.Penalizacoes.Any(p =>p.IdEmprestimo == penalizacao.IdEmprestimo && p.Pago == false);

                if (penalizacaoExistente)
                {
                    return BadRequest("Este empréstimo já possui uma penalização não paga.");
                }
            }

            const decimal valorPorDia = 0.50m;

            penalizacao.Valor = penalizacao.DiasAtraso * valorPorDia;
            penalizacao.DataPenalizacao = DateTime.Now;
            penalizacao.Pago = false;
            penalizacao.DataPagamento = null;

            dc.Penalizacoes.InsertOnSubmit(penalizacao);
            dc.SubmitChanges();

            return Ok(new
            {
                penalizacao.IdPenalizacao,
                penalizacao.IdUtilizador,
                Utilizador = utilizador.Nome,
                penalizacao.IdEmprestimo,
                penalizacao.Valor,
                penalizacao.DiasAtraso,
                penalizacao.Motivo,
                penalizacao.DataPenalizacao,
                penalizacao.Pago,
                penalizacao.DataPagamento
            });
        }

        // PUT api/penalizacoes/1
        public IHttpActionResult Put(int id)
        {
            Penalizacoes penalizacao = dc.Penalizacoes.FirstOrDefault(p => p.IdPenalizacao == id);

            if (penalizacao == null)
            {
                return NotFound();
            }

            if (penalizacao.Pago)
            {
                return BadRequest("Esta penalização já foi paga.");
            }

            penalizacao.Pago = true;
            penalizacao.DataPagamento = DateTime.Now;

            if (penalizacao.IdEmprestimo.HasValue)
            {
                Emprestimo emprestimo = dc.Emprestimos.FirstOrDefault(e =>e.IdEmprestimo == penalizacao.IdEmprestimo.Value);

                if (emprestimo != null)
                {
                    emprestimo.Multa = 0;
                }
            }

            dc.SubmitChanges();

            return Ok(new
            {
                penalizacao.IdPenalizacao,
                penalizacao.IdUtilizador,
                Utilizador = penalizacao.Utilizador.Nome,
                penalizacao.IdEmprestimo,
                penalizacao.Valor,
                penalizacao.DiasAtraso,
                penalizacao.Motivo,
                penalizacao.DataPenalizacao,
                penalizacao.Pago,
                penalizacao.DataPagamento
            });
        }

        // DELETE api/penalizacoes/1
        public IHttpActionResult Delete(int id)
        {
            Penalizacoes penalizacao = dc.Penalizacoes.FirstOrDefault(p => p.IdPenalizacao == id);

            if (penalizacao == null)
            {
                return NotFound();
            }

            if (!penalizacao.Pago)
            {
                return BadRequest("Não é possível eliminar uma penalização não paga.");
            }

            dc.Penalizacoes.DeleteOnSubmit(penalizacao);
            dc.SubmitChanges();

            return Ok();
        }
    }
}