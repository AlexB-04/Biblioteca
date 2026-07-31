using System.Linq;
using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca.Cliente.WPF.Views
{
    /// <summary>
    /// Interaction logic for UtilizadoresView.xaml
    /// </summary>
    public partial class UtilizadoresView : UserControl
    {
        private readonly ApiService apiService = new ApiService();

        private Utilizador utilizadorSelecionado;

        private List<Utilizador> todosUtilizadores = new List<Utilizador>();

        public UtilizadoresView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await CarregarUtilizadoresAsync();
        }

        private async Task CarregarUtilizadoresAsync()
        {
            btnAtualizarUtilizadores.IsEnabled = false;

            Response response = await apiService.GetUtilizadores("http://localhost:56363/", "api/utilizadores");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                btnAtualizarUtilizadores.IsEnabled = true;

                return;
            }

            todosUtilizadores = (List<Utilizador>)response.Result;

            dgUtilizadores.ItemsSource = todosUtilizadores;

            btnAtualizarUtilizadores.IsEnabled = true;
        }

        private void AplicarPesquisa()
        {
            IEnumerable<Utilizador> utilizadoresFiltrados = todosUtilizadores;

            string pesquisa = txtPesquisaUtilizador.Text.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                utilizadoresFiltrados = utilizadoresFiltrados
                    .Where(u =>
                        u.IdUtilizador.ToString().Contains(pesquisa) ||
                        u.Nome.ToLower().Contains(pesquisa) ||
                        u.Email.ToLower().Contains(pesquisa) ||
                        u.Contacto.ToLower().Contains(pesquisa) ||
                        u.TipoUtilizador.ToLower().Contains(pesquisa));
            }

            dgUtilizadores.ItemsSource = utilizadoresFiltrados.ToList();
        }

        private void btnPesquisarUtilizador_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();

            AplicarPesquisa();
        }

        private void btnLimparPesquisa_Click(object sender, RoutedEventArgs e)
        {
            txtPesquisaUtilizador.Clear();

            LimparFormulario();

            dgUtilizadores.ItemsSource = todosUtilizadores;
        }

        private void LimparFormulario()
        {
            txtNome.Clear();
            txtContacto.Clear();
            txtEmail.Clear();
            txtTipoUtilizador.Clear();
            txtLimiteEmprestimos.Clear();
            txtAtrasos.Clear();

            utilizadorSelecionado = null;
            dgUtilizadores.SelectedItem = null;
        }

        private async void btnAtualizarUtilizadores_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();

            txtPesquisaUtilizador.Clear();

            await CarregarUtilizadoresAsync();
        }

        private void dgUtilizadores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            utilizadorSelecionado = dgUtilizadores.SelectedItem as Utilizador;

            if (utilizadorSelecionado == null)
            {
                return;
            }

            txtNome.Text = utilizadorSelecionado.Nome;
            txtContacto.Text = utilizadorSelecionado.Contacto;
            txtEmail.Text = utilizadorSelecionado.Email;
            txtTipoUtilizador.Text = utilizadorSelecionado.TipoUtilizador;
            txtLimiteEmprestimos.Text = utilizadorSelecionado.LimiteEmprestimos.ToString();
            txtAtrasos.Text = utilizadorSelecionado.Atrasos.ToString();
        }

        private async void btnGuardarUtilizador_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome do utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtContacto.Text))
            {
                MessageBox.Show("O contacto do utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("O email do utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtTipoUtilizador.Text))
            {
                MessageBox.Show("O tipo de utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtLimiteEmprestimos.Text, out int limiteEmprestimos) || limiteEmprestimos < 0)
            {
                MessageBox.Show("O limite de empréstimos é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtAtrasos.Text, out int atrasos) || atrasos < 0)
            {
                MessageBox.Show("O número de atrasos é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            Utilizador utilizador = new Utilizador
            {
                Nome = txtNome.Text,
                Contacto = txtContacto.Text,
                Email = txtEmail.Text,
                TipoUtilizador = txtTipoUtilizador.Text,
                LimiteEmprestimos = limiteEmprestimos,
                Atrasos = atrasos
            };

            Response response = await apiService.PostUtilizador("http://localhost:56363/", "api/utilizadores", utilizador);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Utilizador criado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();

            await CarregarUtilizadoresAsync();
        }

        private async void btnAlterarUtilizador_Click(object sender, RoutedEventArgs e)
        {
            if (utilizadorSelecionado == null)
            {
                MessageBox.Show("Selecione um utilizador para alterar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome do utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtContacto.Text))
            {
                MessageBox.Show("O contacto do utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("O email do utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(txtTipoUtilizador.Text))
            {
                MessageBox.Show("O tipo de utilizador é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtLimiteEmprestimos.Text, out int limiteEmprestimos) || limiteEmprestimos < 0)
            {
                MessageBox.Show("O limite de empréstimos é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtAtrasos.Text, out int atrasos) || atrasos < 0)
            {
                MessageBox.Show("O número de atrasos é inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            Utilizador utilizadorAlterado = new Utilizador
            {
                IdUtilizador = utilizadorSelecionado.IdUtilizador,
                Nome = txtNome.Text,
                Contacto = txtContacto.Text,
                Email = txtEmail.Text,
                TipoUtilizador = txtTipoUtilizador.Text,
                LimiteEmprestimos = limiteEmprestimos,
                Atrasos = atrasos,
                BloqueadoAte = utilizadorSelecionado.BloqueadoAte
            };

            Response response = await apiService.PutUtilizador("http://localhost:56363/", "api/utilizadores", utilizadorAlterado);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Utilizador alterado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();

            await CarregarUtilizadoresAsync();
        }

        private async void btnEliminarUtilizador_Click(object sender, RoutedEventArgs e)
        {
            if (utilizadorSelecionado == null)
            {
                MessageBox.Show("Selecione um utilizador para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            MessageBoxResult confirmacao = MessageBox.Show($"Tem a certeza que pretende eliminar o utilizador \"{utilizadorSelecionado.Nome}\"?", "Confirmar eliminação", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao != MessageBoxResult.Yes)
            {
                return;
            }

            Response response = await apiService.DeleteUtilizador("http://localhost:56363/", "api/utilizadores", utilizadorSelecionado.IdUtilizador);

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("Utilizador eliminado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

            LimparFormulario();
            await CarregarUtilizadoresAsync();
        }
    }
}