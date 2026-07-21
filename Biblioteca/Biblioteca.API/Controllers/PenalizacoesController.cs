using System.Linq;
using System.Web.Http;

namespace Biblioteca.API.Controllers
{
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
            var penalizacao = dc.Penalizacoes
                .Where(p => p.IdPenalizacao == id)
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
    }
}