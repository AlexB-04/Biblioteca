using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;

namespace Biblioteca.Cliente.WPF.Views
{
    /// <summary>
    /// Interaction logic for LivrosView.xaml
    /// </summary>
    public partial class LivrosView : UserControl
    {
        private readonly ApiService apiService = new ApiService();

        private Livro livroSelecionado;

        private List<Livro> todosLivros = new List<Livro>();
        private List<Emprestimo> todosEmprestimos = new List<Emprestimo>();
        private List<Reserva> todasReservas = new List<Reserva>();

        public LivrosView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbEstadoLivro.ItemsSource = new List<string>
            {
                "Todos",
                "Disponíveis",
                "Emprestados",
                "Reservados"
            };

            cmbEstadoLivro.SelectedIndex = 0;

            await CarregarCategoriasAsync();
            await CarregarLivrosAsync();
            await CarregarEmprestimosAsync();
            await CarregarReservasAsync();
        }

        private async Task CarregarCategoriasAsync()
        {
            Response response = await apiService.GetCategorias(Config.ApiUrl, "api/categorias");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            List<Categoria> categorias = (List<Categoria>)response.Result;

            cmbCategoria.ItemsSource = categorias;

            List<Categoria> categoriasFiltro = new List<Categoria>();

            categoriasFiltro.Add(new Categoria
            {
                IdCategoria = 0,
                Nome = "Todas"
            });

            categoriasFiltro.AddRange(categorias);

            cmbFiltroCategoria.ItemsSource = categoriasFiltro;
            cmbFiltroCategoria.SelectedIndex = 0;
        }

        private async Task CarregarLivrosAsync()
        {
            btnAtualizarLivros.IsEnabled = false;

            Response response = await apiService.GetLivros(Config.ApiUrl, "api/livros");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                btnAtualizarLivros.IsEnabled = true;

                return;
            }

            todosLivros = (List<Livro>)response.Result;

            dgLivros.ItemsSource = todosLivros;
            btnAtualizarLivros.IsEnabled = true;
        }

        private async Task CarregarEmprestimosAsync()
        {
            Response response = await apiService.GetEmprestimos(Config.ApiUrl, "api/emprestimos");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            todosEmprestimos = (List<Emprestimo>)response.Result;
        }

        private async Task CarregarReservasAsync()
        {
            Response response = await apiService.GetReservas(Config.ApiUrl, "api/reservas");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            todasReservas = (List<Reserva>)response.Result;
        }

        private void AplicarFiltros()
        {
            IEnumerable<Livro> livrosFiltrados = todosLivros;

            string pesquisa = txtPesquisaLivro.Text.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                livrosFiltrados = livrosFiltrados.Where(l =>
                    l.Titulo.ToLower().Contains(pesquisa) ||
                    l.Autor.ToLower().Contains(pesquisa) ||
                    l.Genero.ToLower().Contains(pesquisa) ||
                    l.Categoria.ToLower().Contains(pesquisa) ||
                    l.AnoPublicacao.ToString().Contains(pesquisa));
            }

            if (cmbFiltroCategoria.SelectedValue != null)
            {
                int idCategoria = Convert.ToInt32(cmbFiltroCategoria.SelectedValue);

                if (idCategoria != 0)
                {
                    livrosFiltrados = livrosFiltrados.Where(l => l.IdCategoria == idCategoria);
                }
            }

            string estado = cmbEstadoLivro.SelectedItem as string;

            if (estado == "Disponíveis")
            {
                livrosFiltrados = livrosFiltrados.Where(l => l.ExemplaresDisponiveis > 0);
            }
            else if (estado == "Emprestados")
            {
                livrosFiltrados = livrosFiltrados.Where(l => todosEmprestimos.Any(e => e.IdLivro == l.IdLivro && e.Devolvido == false));
            }
            else if (estado == "Reservados")
            {
                livrosFiltrados = livrosFiltrados.Where(l => todasReservas.Any(r => r.IdLivro == l.IdLivro && r.Ativa == true));
            }

            dgLivros.ItemsSource = livrosFiltrados.ToList();
        }

        private void dgLivros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            livroSelecionado = dgLivros.SelectedItem as Livro;

            if (livroSelecionado == null)
            {
                return;
            }

            txtTitulo.Text = livroSelecionado.Titulo;
            txtAutor.Text = livroSelecionado.Autor;
            txtEditora.Text = livroSelecionado.Editora;
            txtAnoPublicacao.Text = livroSelecionado.AnoPublicacao.ToString();
            txtGenero.Text = livroSelecionado.Genero;
            txtExemplaresDisponiveis.Text = livroSelecionado.ExemplaresDisponiveis.ToString();

            cmbCategoria.SelectedValue = livroSelecionado.IdCategoria;
        }

        private void LimparFormulario()
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            txtEditora.Clear();
            txtAnoPublicacao.Clear();
            txtGenero.Clear();
            txtExemplaresDisponiveis.Clear();

            cmbCategoria.SelectedItem = null;

            livroSelecionado = null;
            dgLivros.SelectedItem = null;
        }

        private async void btnAtualizarLivros_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();

            txtPesquisaLivro.Clear();
            cmbEstadoLivro.SelectedIndex = 0;

            await CarregarCategoriasAsync();
            await CarregarLivrosAsync();
            await CarregarEmprestimosAsync();
            await CarregarReservasAsync();

            dgLivros.ItemsSource = todosLivros;
        }

        private async void btnGuardarLivro_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("O título do livro é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                MessageBox.Show("O autor do livro é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtAnoPublicacao.Text, out int anoPublicacao) || anoPublicacao <= 0)
            {
                MessageBox.Show("O ano de publicação é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtExemplaresDisponiveis.Text, out int exemplaresDisponiveis) || exemplaresDisponiveis < 0)
            {
                MessageBox.Show("O número de exemplares disponíveis é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Selecione uma categoria.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            int idCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);

            Livro livro = new Livro
            {
                Titulo = txtTitulo.Text,
                Autor = txtAutor.Text,
                Editora = txtEditora.Text,
                AnoPublicacao = anoPublicacao,
                Genero = txtGenero.Text,
                ExemplaresDisponiveis = exemplaresDisponiveis,
                IdCategoria = idCategoria
            };

            Response response = await apiService.PostLivro(Config.ApiUrl, "api/livros", livro);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Livro criado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();

            await CarregarLivrosAsync();
        }

        private async void btnAlterarLivro_Click(object sender, RoutedEventArgs e)
        {
            if (livroSelecionado == null)
            {
                MessageBox.Show("Selecione um livro para alterar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("O título do livro é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtAutor.Text))
            {
                MessageBox.Show("O autor do livro é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtAnoPublicacao.Text, out int anoPublicacao) || anoPublicacao <= 0)
            {
                MessageBox.Show("O ano de publicação é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtExemplaresDisponiveis.Text, out int exemplaresDisponiveis) || exemplaresDisponiveis < 0)
            {
                MessageBox.Show("O número de exemplares disponíveis é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Selecione uma categoria.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            int idCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);

            Livro livroAlterado = new Livro
            {
                IdLivro = livroSelecionado.IdLivro,
                Titulo = txtTitulo.Text,
                Autor = txtAutor.Text,
                Editora = txtEditora.Text,
                AnoPublicacao = anoPublicacao,
                Genero = txtGenero.Text,
                ExemplaresDisponiveis = exemplaresDisponiveis,
                IdCategoria = idCategoria
            };

            Response response = await apiService.PutLivro(Config.ApiUrl, "api/livros", livroAlterado);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Livro alterado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();

            await CarregarLivrosAsync();
        }

        private async void btnEliminarLivro_Click(object sender, RoutedEventArgs e)
        {
            if (livroSelecionado == null)
            {
                MessageBox.Show("Selecione um livro para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show($"Tem a certeza que pretende eliminar o livro \"{livroSelecionado.Titulo}\"?", "Confirmar eliminação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.DeleteLivro(Config.ApiUrl, "api/livros", livroSelecionado.IdLivro);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Livro eliminado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();

            await CarregarLivrosAsync();
        }

        private void btnPesquisarLivro_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();

            AplicarFiltros();
        }

        private void btnLimparPesquisa_Click(object sender, RoutedEventArgs e)
        {
            txtPesquisaLivro.Clear();

            cmbFiltroCategoria.SelectedIndex = 0;
            cmbEstadoLivro.SelectedIndex = 0;

            LimparFormulario();

            dgLivros.ItemsSource = todosLivros;
        }
    }
}