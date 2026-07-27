namespace Biblioteca.API.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    public class EmprestimosController : ApiController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext(System.Configuration.ConfigurationManager
            .ConnectionStrings["BibliotecaDBConnectionString"].ConnectionString);

        // GET api/emprestimos
        public IHttpActionResult Get()
        {
            var emprestimos = dc.Emprestimos
                .Select(e => new
                {
                    e.IdEmprestimo,
                    e.IdUtilizador,
                    Utilizador = e.Utilizador.Nome,
                    e.IdLivro,
                    Livro = e.Livro.Titulo,
                    e.DataEmprestimo,
                    e.PrazoDevolucao,
                    e.DataDevolucao,
                    e.Devolvido,
                    e.Multa
                })
                .ToList();

            return Ok(emprestimos);
        }

        // GET api/emprestimos/1
        public IHttpActionResult Get(int id)
        {
            var emprestimo = dc.Emprestimos.Where(e => e.IdEmprestimo == id)
                .Select(e => new
                {
                    e.IdEmprestimo,
                    e.IdUtilizador,
                    Utilizador = e.Utilizador.Nome,
                    e.IdLivro,
                    Livro = e.Livro.Titulo,
                    e.DataEmprestimo,
                    e.PrazoDevolucao,
                    e.DataDevolucao,
                    e.Devolvido,
                    e.Multa
                })
                .FirstOrDefault();

            if (emprestimo == null)
            {
                return NotFound();
            }

            return Ok(emprestimo);
        }

        // POST api/emprestimos
        public IHttpActionResult Post([FromBody] Emprestimo emprestimo)
        {
            if (emprestimo == null)
            {
                return BadRequest("Dados do empréstimo inválidos.");
            }

            Utilizador utilizador = dc.Utilizadors.FirstOrDefault(u => u.IdUtilizador == emprestimo.IdUtilizador);

            if (utilizador == null)
            {
                return BadRequest("O utilizador indicado não existe.");
            }

            bool temEmprestimoEmAtraso = dc.Emprestimos.Any(e =>
                e.IdUtilizador == emprestimo.IdUtilizador &&
                e.Devolvido == false &&
                e.PrazoDevolucao < DateTime.Today);

            if (temEmprestimoEmAtraso)
            {
                return BadRequest("O utilizador possui um empréstimo em atraso.");
            }

            bool temPenalizacaoNaoPaga = dc.Penalizacoes.Any(p => 
                p.IdUtilizador == emprestimo.IdUtilizador && 
                p.Pago == false);

            if (temPenalizacaoNaoPaga)
            {
                return BadRequest("O utilizador possui uma penalização não paga.");
            }

            Livro livro = dc.Livros.FirstOrDefault(l => l.IdLivro == emprestimo.IdLivro);

            if (livro == null)
            {
                return BadRequest("O livro indicado não existe.");
            }

            Reserva primeiraReserva = dc.Reservas
                .Where(r => r.IdLivro == livro.IdLivro && r.Ativa == true)
                .OrderBy(r => r.Ordem).FirstOrDefault();

            if (primeiraReserva != null && primeiraReserva.IdUtilizador != utilizador.IdUtilizador)
            {
                return BadRequest("Este livro está reservado para o primeiro utilizador da fila.");
            }

            if (livro.ExemplaresDisponiveis <= 0)
            {
                return BadRequest("Não existem exemplares disponíveis deste livro.");
            }

            bool emprestimoJaExiste = dc.Emprestimos.Any(e =>
                e.IdUtilizador == emprestimo.IdUtilizador &&
                e.IdLivro == emprestimo.IdLivro &&
                e.Devolvido == false);

            if (emprestimoJaExiste)
            {
                return BadRequest("Este utilizador já tem um empréstimo ativo deste livro.");
            }

            int emprestimosAtivos = dc.Emprestimos.Count(e =>
                e.IdUtilizador == emprestimo.IdUtilizador &&
                e.Devolvido == false);

            if (emprestimosAtivos >= utilizador.LimiteEmprestimos)
            {
                return BadRequest("O utilizador atingiu o limite de empréstimos.");
            }

            emprestimo.DataEmprestimo = DateTime.Now;
            emprestimo.PrazoDevolucao = DateTime.Now.AddDays(ObterDiasEmprestimo(utilizador.TipoUtilizador));
            emprestimo.DataDevolucao = null;
            emprestimo.Devolvido = false;
            emprestimo.Multa = 0;

            if (primeiraReserva != null)
            {
                int ordemRemovida = primeiraReserva.Ordem;

                primeiraReserva.Ativa = false;

                var reservasSeguintes = dc.Reservas.Where(r =>
                    r.IdLivro == livro.IdLivro &&
                    r.Ativa == true &&
                    r.Ordem > ordemRemovida).ToList();

                foreach (Reserva reservaSeguinte in reservasSeguintes)
                {
                    reservaSeguinte.Ordem--;
                }
            }

            livro.ExemplaresDisponiveis--;

            dc.Emprestimos.InsertOnSubmit(emprestimo);
            dc.SubmitChanges();

            return Ok(new
            {
                emprestimo.IdEmprestimo,
                emprestimo.IdUtilizador,
                Utilizador = utilizador.Nome,
                emprestimo.IdLivro,
                Livro = livro.Titulo,
                emprestimo.DataEmprestimo,
                emprestimo.PrazoDevolucao,
                emprestimo.DataDevolucao,
                emprestimo.Devolvido,
                emprestimo.Multa
            });
        }

        private int ObterDiasEmprestimo(string tipoUtilizador)
        {
            if (tipoUtilizador == "Professor")
            {
                return 30;
            }

            if (tipoUtilizador == "Aluno")
            {
                return 15;
            }

            return 7;
        }

        // PUT api/emprestimos/1
        public IHttpActionResult Put(int id)
        {
            Emprestimo emprestimo = dc.Emprestimos.FirstOrDefault(e => e.IdEmprestimo == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            if (emprestimo.Devolvido)
            {
                return BadRequest("Este empréstimo já foi devolvido.");
            }

            Livro livro = dc.Livros.FirstOrDefault(l => l.IdLivro == emprestimo.IdLivro);

            if (livro == null)
            {
                return BadRequest("O livro associado ao empréstimo não existe.");
            }

            Utilizador utilizador = dc.Utilizadors.FirstOrDefault(u => u.IdUtilizador == emprestimo.IdUtilizador);

            if (utilizador == null)
            {
                return BadRequest("O utilizador associado ao empréstimo não existe.");
            }

            DateTime dataDevolucao = DateTime.Now;

            emprestimo.Devolvido = true;
            emprestimo.DataDevolucao = dataDevolucao;

            if (dataDevolucao.Date > emprestimo.PrazoDevolucao.Date)
            {
                int diasAtraso = (dataDevolucao.Date - emprestimo.PrazoDevolucao.Date).Days;

                decimal multa = diasAtraso * 0.5m;

                emprestimo.Multa = multa;
                utilizador.Atrasos++;

                bool penalizacaoJaExiste = dc.Penalizacoes.Any(p => p.IdEmprestimo == emprestimo.IdEmprestimo);

                if (!penalizacaoJaExiste)
                {
                    Penalizacoes penalizacao = new Penalizacoes
                    {
                        IdUtilizador = utilizador.IdUtilizador,
                        IdEmprestimo = emprestimo.IdEmprestimo,
                        Valor = multa,
                        DiasAtraso = diasAtraso,
                        Motivo = $"Atraso de {diasAtraso} dia(s) na devolução do livro \"{livro.Titulo}\".",
                        DataPenalizacao = DateTime.Now,
                        Pago = false,
                        DataPagamento = null
                    };

                    dc.Penalizacoes.InsertOnSubmit(penalizacao);
                }
            }

            livro.ExemplaresDisponiveis++;

            Reserva primeiraReserva = dc.Reservas
                .Where(r =>
                    r.IdLivro == emprestimo.IdLivro &&
                    r.Ativa == true &&
                    r.DataDisponivel == null)
                .OrderBy(r => r.Ordem)
                .FirstOrDefault();

            if (primeiraReserva != null)
            {
                primeiraReserva.DataDisponivel = DateTime.Now;
            }

            dc.SubmitChanges();

            return Ok(new
            {
                emprestimo.IdEmprestimo,
                emprestimo.IdUtilizador,
                Utilizador = utilizador.Nome,
                emprestimo.IdLivro,
                Livro = livro.Titulo,
                emprestimo.DataEmprestimo,
                emprestimo.PrazoDevolucao,
                emprestimo.DataDevolucao,
                emprestimo.Devolvido,
                emprestimo.Multa
            });
        }

        // DELETE api/emprestimos/1
        public IHttpActionResult Delete(int id)
        {
            Emprestimo emprestimo = dc.Emprestimos.FirstOrDefault(e => e.IdEmprestimo == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            if (!emprestimo.Devolvido)
            {
                return BadRequest("Não é possível eliminar um empréstimo ativo. Devolva o livro primeiro.");
            }

            bool possuiPenalizacao = dc.Penalizacoes.Any(p => p.IdEmprestimo == id);

            if (possuiPenalizacao)
            {
                return BadRequest("Não é possível eliminar um empréstimo com penalizações associadas.");
            }

            dc.Emprestimos.DeleteOnSubmit(emprestimo);
            dc.SubmitChanges();

            return Ok();
        }
    }
}