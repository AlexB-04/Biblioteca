using System;

namespace Biblioteca.Cliente.WPF.Models
{
    public class Penalizacao
    {
        public int IdPenalizacao { get; set; }

        public int IdUtilizador { get; set; }

        public string Utilizador { get; set; }

        public int? IdEmprestimo { get; set; }

        public decimal Valor { get; set; }

        public int DiasAtraso { get; set; }

        public string Motivo { get; set; }

        public DateTime DataPenalizacao { get; set; }

        public bool Pago { get; set; }

        public DateTime? DataPagamento { get; set; }
    }
}