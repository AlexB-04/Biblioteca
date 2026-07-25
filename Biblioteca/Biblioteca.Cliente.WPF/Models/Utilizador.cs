using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Cliente.WPF.Models
{
    public class Utilizador
    {
        public int IdUtilizador { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoUtilizador { get; set; } = string.Empty;
        public int LimiteEmprestimos { get; set; }
        public int Atrasos { get; set; }
        public override string ToString()
        {
            return $"{Nome} - {TipoUtilizador}";
        }
    }
}