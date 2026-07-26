using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Cliente.WPF.Models
{
    public class Reserva
    {
        public int IdReserva { get; set; }

        public int IdUtilizador { get; set; }

        public string Utilizador { get; set; }

        public int IdLivro { get; set; }

        public string Livro { get; set; }

        public DateTime DataReserva { get; set; }

        public int Ordem { get; set; }

        public bool Ativa { get; set; }
    }
}