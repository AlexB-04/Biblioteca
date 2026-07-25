using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Cliente.WPF.Models
{
    public class Utilizador
    {
        public int IdUtilizador { get; set; }
        public string Nome { get; set; }
        public string Contacto { get; set; }
        public string Email { get; set; }
        public string TipoUtilizador { get; set; }
        public int LimiteEmprestimos { get; set; }
        public int Atrasos { get; set; }
        public override string ToString()
        {
            return $"{Nome} - {TipoUtilizador}";
        }
    }
}