using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Biblioteca.API.Controllers
{
    public class CategoriasController : ApiController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(
            System.Configuration.ConfigurationManager
            .ConnectionStrings["BibliotecaDBConnectionString"]
            .ConnectionString);

        // GET api/categorias
        public List<Categoria> Get()
        {
            return dc.Categorias.ToList();
        }

        // GET api/categorias/1
        public IHttpActionResult Get(int id)
        {
            Categoria categoria = dc.Categorias.FirstOrDefault(c => c.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        // POST api/categorias
        public IHttpActionResult Post([FromBody] Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest("Dados da categoria inválidos.");
            }

            if (string.IsNullOrWhiteSpace(categoria.Nome))
            {
                return BadRequest("O nome da categoria é obrigatório.");
            }

            dc.Categorias.InsertOnSubmit(categoria);
            dc.SubmitChanges();

            return Ok(categoria);
        }

        // PUT api/categorias/1
        public IHttpActionResult Put (int id, [FromBody] Categoria categoria)
        {
            Categoria categoriaExistente = dc.Categorias.FirstOrDefault(c => c.IdCategoria == id);

            if (categoriaExistente == null)
            {
                return NotFound();
            }

            categoriaExistente.Nome = categoria.Nome;
            categoriaExistente.Descricao = categoria.Descricao;

            dc.SubmitChanges();

            return Ok(categoriaExistente);
        }

        // DELETE api/categorias/1
        public IHttpActionResult Delete (int id)
        {
            Categoria categoria = dc.Categorias.FirstOrDefault(c => c.IdCategoria == id); 

            if (categoria == null)
            {
                return NotFound();
            }

            dc.Categorias.DeleteOnSubmit(categoria);
            dc.SubmitChanges();

            return Ok();
        }
    }
}