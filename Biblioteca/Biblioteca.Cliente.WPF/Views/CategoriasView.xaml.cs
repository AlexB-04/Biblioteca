using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca.Cliente.WPF.Views
{
    /// <summary>
    /// Interaction logic for CategoriasView.xaml
    /// </summary>
    public partial class CategoriasView : UserControl
    {
        private readonly ApiService apiService = new ApiService();

        private Categoria categoriaSelecionada;

        public CategoriasView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CarregarCategoriasAsync();
        }

        private async Task CarregarCategoriasAsync()
        {
            btnAtualizarCategorias.IsEnabled = false;

            Response response = await apiService.GetCategorias(Config.ApiUrl, "api/categorias");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                btnAtualizarCategorias.IsEnabled = true;
                return;
            }

            List<Categoria> categorias = (List<Categoria>)response.Result;

            dgCategorias.ItemsSource = categorias;
            btnAtualizarCategorias.IsEnabled = true;
        }

        private void LimparFormulario()
        {
            txtNome.Clear();
            txtDescricao.Clear();

            categoriaSelecionada = null;
            dgCategorias.SelectedItem = null;
        }

        private void dgCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoriaSelecionada = dgCategorias.SelectedItem as Categoria;

            if (categoriaSelecionada == null)
            {
                return;
            }

            txtNome.Text = categoriaSelecionada.Nome;
            txtDescricao.Text = categoriaSelecionada.Descricao;
        }

        private async void btnAtualizarCategorias_Click(object sender, RoutedEventArgs e)
        {
            await CarregarCategoriasAsync();
        }

        private async void btnGuardarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome da categoria é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            Categoria categoria = new Categoria
            {
                Nome = txtNome.Text,
                Descricao = txtDescricao.Text
            };

            Response response = await apiService.PostCategoria(Config.ApiUrl, "api/categorias", categoria);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Categoria criada com sucesso.");

            LimparFormulario();

            await CarregarCategoriasAsync();
        }

        private async void btnAlterarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (categoriaSelecionada == null)
            {
                MessageBox.Show("Selecione uma categoria para alterar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome da categoria é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            categoriaSelecionada.Nome = txtNome.Text;
            categoriaSelecionada.Descricao = txtDescricao.Text;

            Response response = await apiService.PutCategoria(Config.ApiUrl, "api/categorias", categoriaSelecionada);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Categoria alterada com sucesso.");

            LimparFormulario();

            await CarregarCategoriasAsync();
        }

        private async void btnEliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (categoriaSelecionada == null)
            {
                MessageBox.Show("Selecione uma categoria para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show($"Tem a certeza que pretende eliminar a categoria \"{categoriaSelecionada.Nome}\"?", "Confirmar eliminação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.DeleteCategoria(Config.ApiUrl, "api/categorias", categoriaSelecionada.IdCategoria);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Categoria eliminada com sucesso.");

            LimparFormulario();

            await CarregarCategoriasAsync();
        }
    }
}