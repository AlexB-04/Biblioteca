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

        // POST api/utilizadores
        public IHttpActionResult Post([FromBody] Utilizador utilizador)
        {
            if (utilizador == null)
            {
                return BadRequest("Dados do utilizador inválidos.");
            }

            if (string.IsNullOrWhiteSpace(utilizador.Nome))
            {
                return BadRequest("O nome do utilizador é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(utilizador.Email))
            {
                return BadRequest("O email do utilizador é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(utilizador.TipoUtilizador))
            {
                return BadRequest("O tipo de utilizador é obrigatório.");
            }

            if (utilizador.LimiteEmprestimos < 0)
            {
                return BadRequest("O limite de empréstimos não pode ser negativo.");
            }

            if (utilizador.Atrasos < 0)
            {
                return BadRequest("O número de atrasos não pode ser negativo.");
            }

            bool emailExiste = dc.Utilizadors.Any(u => u.Email == utilizador.Email);

            if (emailExiste)
            {
                return BadRequest("Já existe um utilizador com esse email.");
            }

            dc.Utilizadors.InsertOnSubmit(utilizador);
            dc.SubmitChanges();

            return Ok(new
            {
                utilizador.IdUtilizador,
                utilizador.Nome,
                utilizador.Contacto,
                utilizador.Email,
                utilizador.TipoUtilizador,
                utilizador.LimiteEmprestimos,
                utilizador.Atrasos
            });
        }

        // PUT api/utilizadores/1
        public IHttpActionResult Put(int id, [FromBody] Utilizador utilizador)
        {
            if (utilizador == null)
            {
                return BadRequest("Dados do utilizador inválidos.");
            }

            if (string.IsNullOrWhiteSpace(utilizador.Nome))
            {
                return BadRequest("O nome do utilizador é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(utilizador.Email))
            {
                return BadRequest("O email do utilizador é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(utilizador.TipoUtilizador))
            {
                return BadRequest("O tipo de utilizador é obrigatório.");
            }

            if (utilizador.LimiteEmprestimos < 0)
            {
                return BadRequest("O limite de empréstimos não pode ser negativo.");
            }

            if (utilizador.Atrasos < 0)
            {
                return BadRequest("O número de atrasos não pode ser negativo.");
            }

            Utilizador utilizadorExistente = dc.Utilizadors.FirstOrDefault(u => u.IdUtilizador == id);

            if (utilizadorExistente == null)
            {
                return NotFound();
            }

            bool emailExiste = dc.Utilizadors.Any(u =>
                u.Email == utilizador.Email &&
                u.IdUtilizador != id);

            if (emailExiste)
            {
                return BadRequest("Já existe outro utilizador com esse email.");
            }

            utilizadorExistente.Nome = utilizador.Nome;
            utilizadorExistente.Contacto = utilizador.Contacto;
            utilizadorExistente.Email = utilizador.Email;
            utilizadorExistente.TipoUtilizador = utilizador.TipoUtilizador;
            utilizadorExistente.LimiteEmprestimos = utilizador.LimiteEmprestimos;
            utilizadorExistente.Atrasos = utilizador.Atrasos;

            dc.SubmitChanges();

            return Ok(new
            {
                utilizadorExistente.IdUtilizador,
                utilizadorExistente.Nome,
                utilizadorExistente.Contacto,
                utilizadorExistente.Email,
                utilizadorExistente.TipoUtilizador,
                utilizadorExistente.LimiteEmprestimos,
                utilizadorExistente.Atrasos
            });
        }

        // DELETE api/utilizadores/1
        public IHttpActionResult Delete(int id)
        {
            Utilizador utilizador = dc.Utilizadors.FirstOrDefault(u => u.IdUtilizador == id);

            if (utilizador == null)
            {
                return NotFound();
            }

            bool possuiEmprestimos = dc.Emprestimos.Any(e =>e.IdUtilizador == id);

            if (possuiEmprestimos)
            {
                return BadRequest("Não é possível eliminar um utilizador com histórico de empréstimos.");
            }

            bool possuiReservas = dc.Reservas.Any(r => r.IdUtilizador == id);

            if (possuiReservas)
            {
                return BadRequest("Não é possível eliminar um utilizador com histórico de reservas.");
            }

            bool possuiPenalizacoes = dc.Penalizacoes.Any(p => p.IdUtilizador == id);

            if (possuiPenalizacoes)
            {
                return BadRequest("Não é possível eliminar um utilizador com penalizações associadas.");
            }

            dc.Utilizadors.DeleteOnSubmit(utilizador);
            dc.SubmitChanges();

            return Ok();
        }
    }
}