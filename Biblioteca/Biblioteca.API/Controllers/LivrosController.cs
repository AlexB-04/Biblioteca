namespace Biblioteca.API.Controllers
{
    using System.Linq;
    using System.Web.Http;
    public class LivrosController : ApiController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(
            System.Configuration.ConfigurationManager.
            ConnectionStrings["BibliotecaDBConnectionString"].
            ConnectionString);

        //GET api/livros
        public IHttpActionResult Get()
        {
            var livros = dc.Livros
            .Select(l => new
            {
                l.IdLivro,
                l.Titulo,
                l.Autor,
                l.Editora,
                l.AnoPublicacao,
                l.Genero,
                l.ExemplaresDisponiveis,
                l.IdCategoria,
                Categoria = l.Categoria.Nome
            })
            .ToList();

            return Ok(livros);
        }

        //GET api/livros/1
        public IHttpActionResult Get(int id)
        {
            var livro = dc.Livros
            .Where(l => l.IdLivro == id)
            .Select(l => new
            {
                l.IdLivro,
                l.Titulo,
                l.Autor,
                l.Editora,
                l.AnoPublicacao,
                l.Genero,
                l.ExemplaresDisponiveis,
                l.IdCategoria,
                Categoria = l.Categoria.Nome
            })
            .FirstOrDefault();

            if (livro == null)
            {
                return NotFound();
            }

            return Ok(livro);
        }

        //POST api/livros

        public IHttpActionResult Post([FromBody] Livro livro)
        {
            if (livro == null)
            {
                return BadRequest("Dados do livro inválidos.");
            }

            if (string.IsNullOrWhiteSpace(livro.Titulo))
            {
                return BadRequest("O título do livro é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(livro.Autor))
            {
                return BadRequest("O autor do livro é obrigatório.");
            }

            Categoria categoria = dc.Categorias.FirstOrDefault(c => c.IdCategoria == livro.IdCategoria);

            if (categoria == null)
            {
                return BadRequest("A categoria indicada não existe.");
            }

            dc.Livros.InsertOnSubmit(livro);
            dc.SubmitChanges();

            return Ok(new
            {
                livro.IdLivro,
                livro.Titulo,
                livro.Autor,
                livro.Editora,
                livro.AnoPublicacao,
                livro.Genero,
                livro.ExemplaresDisponiveis,
                livro.IdCategoria,
                Categoria = categoria.Nome
            });
        }
    }
}