using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca.Cliente.WPF.Views
{
    /// <summary>
    /// Interaction logic for ReservasView.xaml
    /// </summary>
    public partial class ReservasView : UserControl
    {
        private readonly ApiService apiService = new ApiService();
        private Reserva reservaSelecionada;

        public ReservasView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            await CarregarReservasAsync();
            await CarregarUtilizadoresAsync();
            await CarregarLivrosAsync();
        }

        private async Task CarregarReservasAsync()
        {
            Response response = await apiService.GetReservas(Config.ApiUrl, "api/reservas");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dgReservas.ItemsSource = response.Result as List<Reserva>;
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

        private async void btnAtualizarReservas_Click(object sender, RoutedEventArgs e)
        {
            reservaSelecionada = null;
            dgReservas.SelectedItem = null;

            await CarregarDadosAsync();
        }
        private async void btnRegistarReserva_Click(object sender, RoutedEventArgs e)
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

            Reserva reserva = new Reserva
            {
                IdUtilizador = (int)cbUtilizadores.SelectedValue,
                IdLivro = (int)cbLivros.SelectedValue
            };

            Response response = await apiService.PostReserva(Config.ApiUrl, "api/reservas", reserva);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Reserva registada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            cbUtilizadores.SelectedIndex = -1;
            cbLivros.SelectedIndex = -1;

            await CarregarDadosAsync();
        }

        private async void btnCancelarReserva_Click(object sender, RoutedEventArgs e)
        {
            if (reservaSelecionada == null)
            {
                MessageBox.Show("Selecione uma reserva.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!reservaSelecionada.Ativa)
            {
                MessageBox.Show("Esta reserva já foi cancelada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show("Deseja cancelar esta reserva?", "Confirmar cancelamento", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.PutReserva(Config.ApiUrl, "api/reservas", reservaSelecionada);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Reserva cancelada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            reservaSelecionada = null;
            dgReservas.SelectedItem = null;

            await CarregarDadosAsync();
        }

        private async void btnEliminarReserva_Click(object sender, RoutedEventArgs e)
        {
            if (reservaSelecionada == null)
            {
                MessageBox.Show("Selecione uma reserva.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (reservaSelecionada.Ativa)
            {
                MessageBox.Show("Não é possível eliminar uma reserva ativa. Cancele a reserva primeiro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show("Deseja eliminar esta reserva?", "Confirmar eliminação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.DeleteReserva(Config.ApiUrl, "api/reservas", reservaSelecionada.IdReserva);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Reserva eliminada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            reservaSelecionada = null;
            dgReservas.SelectedItem = null;

            await CarregarDadosAsync();
        }

        private async void dgReservas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reservaSelecionada = dgReservas.SelectedItem as Reserva;
        }
    }
}