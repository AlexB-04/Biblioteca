using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Cliente.WPF.Models
{
    public class Emprestimo
    {
        public int IdEmprestimo { get; set; }
        public int IdUtilizador { get; set; }
        public string Utilizador { get; set; }
        public int IdLivro { get; set; }
        public string Livro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime PrazoDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public bool Devolvido { get; set; }
        public decimal Multa { get; set; }
        public string Descricao
        {
            get
            {
                return $"{IdEmprestimo} - {Utilizador} - {Livro}";
            }
        }
    }
}