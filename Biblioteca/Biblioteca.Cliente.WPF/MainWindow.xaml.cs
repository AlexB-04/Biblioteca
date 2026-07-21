using Biblioteca.Cliente.WPF.Models;
using Biblioteca.Cliente.WPF.Services;
using System.Collections.Generic;
using System.Windows;

namespace Biblioteca.Cliente.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiService apiService = new ApiService();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnCarregarCategorias_Click(object sender, RoutedEventArgs e)
        {
            btnCarregarCategorias.IsEnabled = false;

            Response response = await apiService.GetCategorias("http://localhost:56363/", "api/categorias");

            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);

                btnCarregarCategorias.IsEnabled = true;
                return;
            }

            List<Categoria> categorias = (List<Categoria>)response.Result;

            dgCategorias.ItemsSource = categorias;
            btnCarregarCategorias.IsEnabled = true;
        }
    }
}