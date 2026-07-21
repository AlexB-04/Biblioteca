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
        public IHttpActionResult Get()
        {
            var categorias = dc.Categorias
                .Select(c => new
                {
                    c.IdCategoria,
                    c.Nome,
                    c.Descricao
                })
                .ToList();

            return Ok(categorias);
        }

        // GET api/categorias/1
        public IHttpActionResult Get(int id)
        {
            var categoria = dc.Categorias
                .Where(c => c.IdCategoria == id)
                .Select(c => new
                {
                    c.IdCategoria,
                    c.Nome,
                    c.Descricao
                })
                .FirstOrDefault();

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

            bool categoriaJaExiste = dc.Categorias.Any(c => c.Nome == categoria.Nome);

            if (categoriaJaExiste)
            {
                return BadRequest("Já existe uma categoria com esse nome.");
            }

            dc.Categorias.InsertOnSubmit(categoria);
            dc.SubmitChanges();

            return Ok(new
            {
                categoria.IdCategoria,
                categoria.Nome,
                categoria.Descricao
            });
        }

        // PUT api/categorias/1
        public IHttpActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest("Dados da categoria inválidos.");
            }

            if (string.IsNullOrWhiteSpace(categoria.Nome))
            {
                return BadRequest("O nome da categoria é obrigatório.");
            }

            Categoria categoriaExistente = dc.Categorias.FirstOrDefault(c => c.IdCategoria == id);

            if (categoriaExistente == null)
            {
                return NotFound();
            }

            bool categoriaJaExiste = dc.Categorias.Any(c =>
                c.Nome == categoria.Nome &&
                c.IdCategoria != id);

            if (categoriaJaExiste)
            {
                return BadRequest("Já existe outra categoria com esse nome.");
            }

            categoriaExistente.Nome = categoria.Nome;
            categoriaExistente.Descricao = categoria.Descricao;

            dc.SubmitChanges();

            return Ok(new
            {
                categoriaExistente.IdCategoria,
                categoriaExistente.Nome,
                categoriaExistente.Descricao
            });
        }

        // DELETE api/categorias/1
        public IHttpActionResult Delete(int id)
        {
            Categoria categoria = dc.Categorias.FirstOrDefault(c => c.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            bool possuiLivros = dc.Livros.Any(l =>l.IdCategoria == id);

            if (possuiLivros)
            {
                return BadRequest("Não é possível eliminar uma categoria com livros associados.");
            }

            dc.Categorias.DeleteOnSubmit(categoria);
            dc.SubmitChanges();

            return Ok();
        }
    }
}