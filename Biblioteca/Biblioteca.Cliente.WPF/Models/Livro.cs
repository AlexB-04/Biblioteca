using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Cliente.WPF.Models
{
    public class Livro
    {
        public int IdLivro { get; set; }
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Editora { get; set; }
        public int AnoPublicacao { get; set; }
        public string? Genero { get; set; }
        public int ExemplaresDisponiveis { get; set; }
        public int IdCategoria { get; set; }
        public string? Categoria { get; set; }
        public bool ShouldSerializeCategoria()
        {
            return false;
        }
    }
}