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

            if (livro.AnoPublicacao <= 0)
            {
                return BadRequest("O ano de publicação é inválido.");
            }

            if (livro.ExemplaresDisponiveis < 0)
            {
                return BadRequest("O número de exemplares disponíveis não pode ser negativo.");
            }

            bool livroJaExiste = dc.Livros.Any(l => l.Titulo == livro.Titulo && l.Autor == livro.Autor);

            if (livroJaExiste)
            {
                return BadRequest("Já existe um livro com esse título e autor.");
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

        //PUT api/livros/1
        public IHttpActionResult Put(int id, [FromBody] Livro livro)
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
            
            if (livro.AnoPublicacao <= 0)
            {
                return BadRequest("O ano de publicação é inválido.");
            }

            if (livro.ExemplaresDisponiveis < 0)
            {
                return BadRequest("O número de exemplares disponíveis não pode ser negativo.");
            }

            Categoria categoria = dc.Categorias.FirstOrDefault(c => c.IdCategoria == livro.IdCategoria);

            if (categoria == null)
            {
                return BadRequest("A categoria indicada não existe.");
            }

            Livro livroExistente = dc.Livros.FirstOrDefault(l => l.IdLivro == id);

            if (livroExistente == null)
            {
                return NotFound();
            }

            livroExistente.Titulo = livro.Titulo;
            livroExistente.Autor = livro.Autor;
            livroExistente.Editora = livro.Editora;
            livroExistente.AnoPublicacao = livro.AnoPublicacao;
            livroExistente.Genero = livro.Genero;
            livroExistente.ExemplaresDisponiveis = livro.ExemplaresDisponiveis;
            livroExistente.IdCategoria = livro.IdCategoria;

            dc.SubmitChanges();

            return Ok(new
            {
                livroExistente.IdLivro,
                livroExistente.Titulo,
                livroExistente.Autor,
                livroExistente.Editora,
                livroExistente.AnoPublicacao,
                livroExistente.Genero,
                livroExistente.ExemplaresDisponiveis,
                livroExistente.IdCategoria,
                Categoria = categoria.Nome
            });
        }

        //DELETE api/livros/1
        public IHttpActionResult Delete(int id)
        {
            Livro livro = dc.Livros.FirstOrDefault(l => l.IdLivro == id);

            if (livro == null)
            {
                return NotFound();
            }

            dc.Livros.DeleteOnSubmit(livro);
            dc.SubmitChanges();

            return Ok();
        }
    }
}