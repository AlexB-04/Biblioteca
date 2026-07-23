using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca.Cliente.WPF.Views
{
    /// <summary>
    /// Interaction logic for LivrosView.xaml
    /// </summary>
    public partial class LivrosView : UserControl
    {
        private readonly ApiService apiService = new ApiService();

        private Livro livroSelecionado;

        public LivrosView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CarregarCategoriasAsync();
            await CarregarLivrosAsync();
        }

        private async Task CarregarCategoriasAsync()
        {
            Response response = await apiService.GetCategorias("http://localhost:56363/", "api/categorias");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message,"Erro",MessageBoxButton.OK,MessageBoxImage.Error);

                return;
            }

            List<Categoria> categorias = (List<Categoria>)response.Result;
            cmbCategoria.ItemsSource = categorias;
        }

        private async Task CarregarLivrosAsync()
        {
            btnAtualizarLivros.IsEnabled = false;

            Response response = await apiService.GetLivros("http://localhost:56363/", "api/livros");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message,"Erro",MessageBoxButton.OK,MessageBoxImage.Error);

                btnAtualizarLivros.IsEnabled = true;
                return;
            }

            List<Livro> livros = (List<Livro>)response.Result;

            dgLivros.ItemsSource = livros;
            btnAtualizarLivros.IsEnabled = true;
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

            await CarregarCategoriasAsync();
            await CarregarLivrosAsync();
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
                MessageBox.Show( "O número de exemplares disponíveis é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

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

            Response response = await apiService.PostLivro( "http://localhost:56363/", "api/livros", livro);

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

            Response response = await apiService.PutLivro("http://localhost:56363/", "api/livros", livroAlterado);

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

            Response response = await apiService.DeleteLivro("http://localhost:56363/", "api/livros", livroSelecionado.IdLivro);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Livro eliminado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();
            await CarregarLivrosAsync();
        }
    }
}