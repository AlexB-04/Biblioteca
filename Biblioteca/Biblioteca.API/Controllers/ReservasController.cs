namespace Biblioteca.API.Controllers
{
    using System;
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

        // POST api/reservas
        public IHttpActionResult Post([FromBody] Reserva reserva)
        {
            if (reserva == null)
            {
                return BadRequest("Dados da reserva inválidos.");
            }

            Utilizador utilizador = dc.Utilizadors.FirstOrDefault(u => u.IdUtilizador == reserva.IdUtilizador);

            if (utilizador == null)
            {
                return BadRequest("O utilizador indicado não existe.");
            }

            Livro livro = dc.Livros.FirstOrDefault(l => l.IdLivro == reserva.IdLivro);

            if (livro == null)
            {
                return BadRequest("O livro indicado não existe.");
            }

            if (livro.ExemplaresDisponiveis > 0)
            {
                return BadRequest("Este livro ainda tem exemplares disponíveis. Deve ser feito um empréstimo.");
            }

            bool reservaJaExiste = dc.Reservas.Any(r =>
                r.IdUtilizador == reserva.IdUtilizador &&
                r.IdLivro == reserva.IdLivro &&
                r.Ativa == true);

            if (reservaJaExiste)
            {
                return BadRequest("Este utilizador já tem uma reserva ativa deste livro.");
            }

            int reservasAtivas = dc.Reservas.Count(r =>
                r.IdUtilizador == reserva.IdUtilizador &&
                r.Ativa == true);

            if (reservasAtivas >= 3)
            {
                return BadRequest("O utilizador atingiu o limite de reservas ativas.");
            }

            int ordem = dc.Reservas.Count(r =>
                r.IdLivro == reserva.IdLivro &&
                r.Ativa == true) + 1;

            reserva.DataReserva = DateTime.Now;
            reserva.Ordem = ordem;
            reserva.Ativa = true;

            dc.Reservas.InsertOnSubmit(reserva);
            dc.SubmitChanges();

            return Ok(new
            {
                reserva.IdReserva,
                reserva.IdUtilizador,
                Utilizador = utilizador.Nome,
                reserva.IdLivro,
                Livro = livro.Titulo,
                reserva.DataReserva,
                reserva.Ordem,
                reserva.Ativa
            });
        }

        // PUT api/reservas/1
        public IHttpActionResult Put(int id)
        {
            Reserva reserva = dc.Reservas.FirstOrDefault(r => r.IdReserva == id);

            if (reserva == null)
            {
                return NotFound();
            }

            if (!reserva.Ativa)
            {
                return BadRequest("Esta reserva já foi cancelada.");
            }

            Utilizador utilizador = dc.Utilizadors.FirstOrDefault(u => u.IdUtilizador == reserva.IdUtilizador);

            if (utilizador == null)
            {
                return BadRequest("O utilizador associado à reserva não existe.");
            }

            Livro livro = dc.Livros.FirstOrDefault(l => l.IdLivro == reserva.IdLivro);

            if (livro == null)
            {
                return BadRequest("O livro associado à reserva não existe.");
            }

            reserva.Ativa = false;

            dc.SubmitChanges();

            return Ok(new
            {
                reserva.IdReserva,
                reserva.IdUtilizador,
                Utilizador = utilizador.Nome,
                reserva.IdLivro,
                Livro = livro.Titulo,
                reserva.DataReserva,
                reserva.Ordem,
                reserva.Ativa
            });
        }

        // DELETE api/reservas/1
        public IHttpActionResult Delete(int id)
        {
            Reserva reserva = dc.Reservas.FirstOrDefault(r => r.IdReserva == id);

            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.Ativa)
            {
                return BadRequest("Não é possível eliminar uma reserva ativa. Cancele a reserva primeiro.");
            }

            dc.Reservas.DeleteOnSubmit(reserva);
            dc.SubmitChanges();

            return Ok();
        }
    }
}