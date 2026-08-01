using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca.Cliente.WPF.Views
{
    /// <summary>
    /// Interaction logic for EmprestimosView.xaml
    /// </summary>
    public partial class EmprestimosView : UserControl
    {
        private readonly ApiService apiService = new ApiService();

        private Emprestimo emprestimoSelecionado;
        public EmprestimosView()
        {
            InitializeComponent();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CarregarDadosAsync();
        }
        private async Task CarregarDadosAsync()
        {
            await CarregarEmprestimosAsync();
            await CarregarUtilizadoresAsync();
            await CarregarLivrosAsync();
        }
        private async Task CarregarEmprestimosAsync()
        {
            Response response = await apiService.GetEmprestimos(Config.ApiUrl, "api/emprestimos");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            dgEmprestimos.ItemsSource = response.Result as List<Emprestimo>;
        }
        private async Task CarregarUtilizadoresAsync()
        {
            Response response = await apiService.GetUtilizadores(Config.ApiUrl, "api/utilizadores");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            cbUtilizadores.ItemsSource = response.Result as List<Utilizador>;
        }
        private async Task CarregarLivrosAsync()
        {
            Response response = await apiService.GetLivros(Config.ApiUrl, "api/livros");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            cbLivros.ItemsSource = response.Result as List<Livro>;
        }
        private async void btnAtualizarEmprestimos_Click(object sender, RoutedEventArgs e)
        {
            await CarregarDadosAsync();
        }
        private async void btnRegistarEmprestimo_Click(object sender, RoutedEventArgs e)
        {
            if (cbUtilizadores.SelectedValue == null)
            {
                MessageBox.Show("Selecione um utilizador.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (cbLivros.SelectedValue == null)
            {
                MessageBox.Show("Selecione um livro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            Emprestimo emprestimo = new Emprestimo
            {
                IdUtilizador = (int)cbUtilizadores.SelectedValue,
                IdLivro = (int)cbLivros.SelectedValue
            };

            Response response = await apiService.PostEmprestimo(Config.ApiUrl, "api/emprestimos", emprestimo);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Empréstimo registado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            cbUtilizadores.SelectedIndex = -1;
            cbLivros.SelectedIndex = -1;

            await CarregarDadosAsync();
        }
        private async void btnDevolverEmprestimo_Click(object sender, RoutedEventArgs e)
        {
            if (emprestimoSelecionado == null)
            {
                MessageBox.Show("Selecione um empréstimo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (emprestimoSelecionado.Devolvido)
            {
                MessageBox.Show("Este empréstimo já foi devolvido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show("Deseja registar a devolução deste livro?", "Confirmar devolução", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.PutEmprestimo(Config.ApiUrl, "api/emprestimos", emprestimoSelecionado);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Devolução registada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            emprestimoSelecionado = null;
            dgEmprestimos.SelectedItem = null;

            await CarregarDadosAsync();
        }
        private async void btnEliminarEmprestimo_Click(object sender, RoutedEventArgs e)
        {
            if (emprestimoSelecionado == null)
            {
                MessageBox.Show("Selecione um empréstimo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!emprestimoSelecionado.Devolvido)
            {
                MessageBox.Show("Não é possível eliminar um empréstimo ativo. Devolva o livro primeiro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show("Deseja eliminar este empréstimo?", "Confirmar eliminação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.DeleteEmprestimo(Config.ApiUrl, "api/emprestimos", emprestimoSelecionado.IdEmprestimo);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Empréstimo eliminado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            emprestimoSelecionado = null;
            dgEmprestimos.SelectedItem = null;

            await CarregarDadosAsync();
        }
        private void dgEmprestimos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            emprestimoSelecionado = dgEmprestimos.SelectedItem as Emprestimo;
        }
    }
}