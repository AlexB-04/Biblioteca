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
    }
}