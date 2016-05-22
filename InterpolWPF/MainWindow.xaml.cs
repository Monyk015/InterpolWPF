using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterpolWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BadBoy b = new BadBoy
            {
                Profession = "Comrade Sniper",
                Surname = "Kozlov",
                Name = "Vasiliy",
                Nickname = "Padliva"
            };

            BadBoy c = new BadBoy
            {
                Profession = "Comrade Sniper",
                Surname = "Kozlov",
                Name = "Asiliy",
                Nickname = "Azazaz"
            };
            G.BadBoys.Add(b);
            G.BadBoys.Add(c);
            dataGrid.ItemsSource = G.BadBoys;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text != "")
            {
                dataGrid.ItemsSource = new List<BadBoy>(G.BadBoys.Where(boy => boy.isFound(SearchTextBox.Text)).ToList());
            }
            else
            {
                dataGrid.ItemsSource = G.BadBoys;
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.TextChanged -= SearchTextBox_TextChanged;
            SearchTextBox.Text = "Global Search...";
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Window editWindow = new Window1();
            editWindow.Show();
        }
    }
}
