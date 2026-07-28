using Biblioteca.Cliente.WPF.Views;
using System.Windows;

namespace Biblioteca.Cliente.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            conteudoPrincipal.Content = new CategoriasView();
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new CategoriasView();
        }

        private void btnLivros_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new LivrosView();
        }

        private void btnUtilizadores_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new UtilizadoresView();
        }

        private void btnEmprestimos_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new EmprestimosView();
        }

        private void btnReservas_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new ReservasView();
        }

        private void btnPenalizacoes_Click(object sender, RoutedEventArgs e)
        {
            conteudoPrincipal.Content = new PenalizacoesView();
        }
    }
}