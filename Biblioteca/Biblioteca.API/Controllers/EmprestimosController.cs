namespace Biblioteca.API.Controllers
{
    using System.Linq;
    using System.Web.Http;

    public class EmprestimosController : ApiController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(System.Configuration.ConfigurationManager
            .ConnectionStrings["BibliotecaDBConnectionString"].ConnectionString);

        // GET api/emprestimos
        public IHttpActionResult Get()
        {
            var emprestimos = dc.Emprestimos
                .Select(e => new
                {
                    e.IdEmprestimo,
                    e.IdUtilizador,
                    Utilizador = e.Utilizador.Nome,
                    e.IdLivro,
                    Livro = e.Livro.Titulo,
                    e.DataEmprestimo,
                    e.PrazoDevolucao,
                    e.DataDevolucao,
                    e.Devolvido,
                    e.Multa
                })
                .ToList();

            return Ok(emprestimos);
        }

        // GET api/emprestimos/1
        public IHttpActionResult Get(int id)
        {
            var emprestimo = dc.Emprestimos.Where(e => e.IdEmprestimo == id)
                .Select(e => new
                {
                    e.IdEmprestimo,
                    e.IdUtilizador,
                    Utilizador = e.Utilizador.Nome,
                    e.IdLivro,
                    Livro = e.Livro.Titulo,
                    e.DataEmprestimo,
                    e.PrazoDevolucao,
                    e.DataDevolucao,
                    e.Devolvido,
                    e.Multa
                })
                .FirstOrDefault();

            if (emprestimo == null)
            {
                return NotFound();
            }

            return Ok(emprestimo);
        }
    }
}