using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca.Cliente.WPF.Views
{
    /// <summary>
    /// Interaction logic for PenalizacoesView.xaml
    /// </summary>
    public partial class PenalizacoesView : UserControl
    {
        private readonly ApiService apiService = new ApiService();

        private Penalizacao penalizacaoSelecionada;

        public PenalizacoesView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            await CarregarPenalizacoesAsync();
            await CarregarUtilizadoresAsync();
            await CarregarEmprestimosAsync();
        }

        private async Task CarregarPenalizacoesAsync()
        {
            Response response = await apiService.GetPenalizacoes(Config.ApiUrl, "api/penalizacoes");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            dgPenalizacoes.ItemsSource = response.Result as List<Penalizacao>;
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

        private async Task CarregarEmprestimosAsync()
        {
            Response response = await apiService.GetEmprestimos(Config.ApiUrl, "api/emprestimos");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            cbEmprestimos.ItemsSource = response.Result as List<Emprestimo>;
        }

        private async void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();

            await CarregarDadosAsync();
        }

        private async void btnRegistar_Click(object sender, RoutedEventArgs e)
        {
            if (cbUtilizadores.SelectedValue == null)
            {
                MessageBox.Show("Selecione um utilizador.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtDiasAtraso.Text, out int diasAtraso))
            {
                MessageBox.Show("Introduza um número válido de dias de atraso.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (diasAtraso <= 0)
            {
                MessageBox.Show("Os dias de atraso devem ser superiores a zero.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Introduza o motivo da penalização.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            int idUtilizador = (int)cbUtilizadores.SelectedValue;

            int? idEmprestimo = null;

            if (cbEmprestimos.SelectedValue != null)
            {
                Emprestimo emprestimoSelecionado = cbEmprestimos.SelectedItem as Emprestimo;

                if (emprestimoSelecionado != null && emprestimoSelecionado.IdUtilizador != idUtilizador)
                {
                    MessageBox.Show("O empréstimo selecionado não pertence ao utilizador.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                idEmprestimo = (int)cbEmprestimos.SelectedValue;
            }

            Penalizacao penalizacao = new Penalizacao
            {
                IdUtilizador = idUtilizador,
                IdEmprestimo = idEmprestimo,
                DiasAtraso = diasAtraso,
                Motivo = txtMotivo.Text.Trim()
            };

            Response response = await apiService.PostPenalizacao(Config.ApiUrl, "api/penalizacoes", penalizacao);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Penalização registada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparCampos();

            await CarregarDadosAsync();
        }

        private async void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            if (penalizacaoSelecionada == null)
            {
                MessageBox.Show("Selecione uma penalização.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (penalizacaoSelecionada.Pago)
            {
                MessageBox.Show("Esta penalização já foi paga.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show("Deseja registar o pagamento desta penalização?", "Confirmar pagamento", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.PutPenalizacao(Config.ApiUrl, "api/penalizacoes", penalizacaoSelecionada);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Pagamento registado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparCampos();

            await CarregarDadosAsync();
        }

        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (penalizacaoSelecionada == null)
            {
                MessageBox.Show("Selecione uma penalização.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!penalizacaoSelecionada.Pago)
            {
                MessageBox.Show("Não é possível eliminar uma penalização não paga.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show("Deseja eliminar esta penalização?", "Confirmar eliminação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.DeletePenalizacao(Config.ApiUrl, "api/penalizacoes", penalizacaoSelecionada.IdPenalizacao);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Penalização eliminada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparCampos();

            await CarregarDadosAsync();
        }

        private void dgPenalizacoes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            penalizacaoSelecionada = dgPenalizacoes.SelectedItem as Penalizacao;
        }

        private void LimparCampos()
        {
            penalizacaoSelecionada = null;

            dgPenalizacoes.SelectedItem = null;

            cbUtilizadores.SelectedIndex = -1;
            cbEmprestimos.SelectedIndex = -1;

            txtDiasAtraso.Clear();
            txtMotivo.Clear();
        }
    }
}