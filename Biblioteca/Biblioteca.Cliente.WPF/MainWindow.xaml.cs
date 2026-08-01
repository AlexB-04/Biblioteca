using Biblioteca.Cliente.WPF.Views;
using System.Windows;
using System.Windows.Controls;

namespace Biblioteca.Cliente.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MostrarPaginaInicial();
        }

        private void MostrarPaginaInicial()
        {
            StackPanel painelInicial = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            TextBlock titulo = new TextBlock
            {
                Text = "Sistema de Gestão de Biblioteca",
                FontSize = 30,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            TextBlock mensagem = new TextBlock
            {
                Text = "Selecione uma opção no menu lateral.",
                FontSize = 18,
                Margin = new Thickness(0, 15, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            painelInicial.Children.Add(titulo);
            painelInicial.Children.Add(mensagem);

            conteudoPrincipal.Content = painelInicial;

            LimparSelecaoMenu();
        }

        private void LimparSelecaoMenu()
        {
            barraCategorias.Background = System.Windows.Media.Brushes.Transparent;
            barraLivros.Background = System.Windows.Media.Brushes.Transparent;
            barraUtilizadores.Background = System.Windows.Media.Brushes.Transparent;
            barraEmprestimos.Background = System.Windows.Media.Brushes.Transparent;
            barraReservas.Background = System.Windows.Media.Brushes.Transparent;
            barraPenalizacoes.Background = System.Windows.Media.Brushes.Transparent;
            barraCreditos.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void SelecionarBarra(Border barraSelecionada)
        {
            LimparSelecaoMenu();

            barraSelecionada.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(25, 118, 210));
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new CategoriasView();

            SelecionarBarra(barraCategorias);
        }

        private void btnLivros_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new LivrosView();

            SelecionarBarra(barraLivros);
        }

        private void btnUtilizadores_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new UtilizadoresView();

            SelecionarBarra(barraUtilizadores);
        }

        private void btnEmprestimos_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new EmprestimosView();

            SelecionarBarra(barraEmprestimos);
        }

        private void btnReservas_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new ReservasView();

            SelecionarBarra(barraReservas);
        }

        private void btnPenalizacoes_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new PenalizacoesView();

            SelecionarBarra(barraPenalizacoes);
        }

        private void btnCreditos_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new CreditosView();

            SelecionarBarra(barraCreditos);
        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}