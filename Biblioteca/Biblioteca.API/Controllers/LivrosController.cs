using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Biblioteca.API.Controllers
{
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
            l.CodigoLivro,
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
                l.CodigoLivro,
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
    }
}