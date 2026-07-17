namespace Biblioteca.API.Controllers
{
    using System.Linq;
    using System.Web.Http;

    public class ReservasController : ApiController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(
            System.Configuration.ConfigurationManager
            .ConnectionStrings["BibliotecaDBConnectionString"]
            .ConnectionString);

        // GET api/reservas
        public IHttpActionResult Get()
        {
            var reservas = dc.Reservas
                .Select(r => new
                {
                    r.IdReserva,
                    r.IdUtilizador,
                    Utilizador = r.Utilizador.Nome,
                    r.IdLivro,
                    Livro = r.Livro.Titulo,
                    r.DataReserva,
                    r.Ordem,
                    r.Ativa
                })
                .ToList();

            return Ok(reservas);
        }

        // GET api/reservas/1
        public IHttpActionResult Get(int id)
        {
            var reserva = dc.Reservas.Where(r => r.IdReserva == id)
                .Select(r => new
                {
                    r.IdReserva,
                    r.IdUtilizador,
                    Utilizador = r.Utilizador.Nome,
                    r.IdLivro,
                    Livro = r.Livro.Titulo,
                    r.DataReserva,
                    r.Ordem,
                    r.Ativa
                })
                .FirstOrDefault();

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
    }
}