namespace Biblioteca.API.Controllers
{
    using System.Linq;
    using System.Web.Http;

    public class UtilizadoresController : ApiController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(
            System.Configuration.ConfigurationManager
            .ConnectionStrings["BibliotecaDBConnectionString"]
            .ConnectionString);

        // GET api/utilizadores
        public IHttpActionResult Get()
        {
            var utilizadores = dc.Utilizadors
                .Select(u => new
                {
                    u.IdUtilizador,
                    u.Nome,
                    u.Contacto,
                    u.Email,
                    u.TipoUtilizador,
                    u.LimiteEmprestimos,
                    u.Atrasos
                })
                .ToList();

            return Ok(utilizadores);
        }

        // GET api/utilizadores/1
        public IHttpActionResult Get(int id)
        {
            var utilizador = dc.Utilizadors
                .Where(u => u.IdUtilizador == id)
                .Select(u => new
                {
                    u.IdUtilizador,
                    u.Nome,
                    u.Contacto,
                    u.Email,
                    u.TipoUtilizador,
                    u.LimiteEmprestimos,
                    u.Atrasos
                })
                .FirstOrDefault();

            if (utilizador == null)
            {
                return NotFound();
            }

            return Ok(utilizador);
        }
    }
}