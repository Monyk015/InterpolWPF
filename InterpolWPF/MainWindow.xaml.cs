using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json;

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
            G.ReadFromFile();
            dataGrid.ItemsSource = G.BadBoys;
            SaveButton.Click += (object sender, RoutedEventArgs e) =>
            {
                
                G.SaveToFile();
            };
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text != "")
            {
                dataGrid.ItemsSource = new List<BadBoy>(G.BadBoys.Where(boy => boy.IsFound(SearchTextBox.Text)).ToList());
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
            Window editWindow = new AddWindow(false);
            editWindow.Show();
        }

        public void Add(BadBoy toAdd)
        {
            G.BadBoys.Add(toAdd);
            dataGrid.Items.Refresh();
        }


        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            Window findWindow = new AddWindow(true);
            findWindow.Show();
        }

        public void Find(BadBoy toFind, bool isDateChanged)
        {
            var properties = typeof(BadBoy).GetProperties();
            var toDisplay = new List<BadBoy>();
            foreach (var i in G.BadBoys)
            {
                bool isFine = true;
                foreach (var propertyInfo in properties)
                {
                    if (propertyInfo.Name != "Languages" && propertyInfo.Name != "DistinguishingTraits" &&
                        propertyInfo.Name != "BirthDate" && propertyInfo.Name != "Accomplices")
                    {
                        if (propertyInfo.GetValue(toFind).ToString() != "" && propertyInfo.GetValue(toFind).ToString() != "0" &&
                            !propertyInfo.GetValue(i).ToString().ToLower().Contains(propertyInfo.GetValue(toFind).ToString().ToLower()))
                        {
                            isFine = false;
                            break;
                        }
                    }
                    if (toFind._distinguishingTraits.Count > 0)
                        foreach (var trait in toFind._distinguishingTraits)
                        {
                            if (trait != "" && i._distinguishingTraits != null && !i._distinguishingTraits.Contains(trait))
                            {
                                isFine = false;
                                break;
                            }
                        }
                    if (toFind._languages.Count > 0)
                        foreach (var language in toFind._languages)
                        {
                            if (language != "" && i._languages != null && !i._languages.Contains(language))
                            {
                                isFine = false;
                                break;
                            }
                        }
                    if (toFind.Accomplices.Count > 0)
                        foreach (var accomplice in toFind.Accomplices)
                        {
                            if (!i.Accomplices.Contains(accomplice))
                            {
                                isFine = false;
                                break;
                            }
                        }
                    if (isDateChanged &&
                        i.BirthDate != toFind.BirthDate)
                    {
                        isFine = false;
                        break;
                    }

                    if(toFind.Crimes.Count > 0)
                        foreach (var crime in toFind.Crimes)
                        {
                            var query = from thisGuyCrime in i.Crimes
                                where crime.isSuccessor(thisGuyCrime)
                                select thisGuyCrime;
                            if (!query.Any())
                            {
                                isFine = false;
                            }
                        }
                }
                if (isFine)
                    toDisplay.Add(i);
            }
            dataGrid.ItemsSource = toDisplay;
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            dataGrid.CancelEdit();
            var editWindow = new EditWindow((BadBoy)dataGrid.SelectedItem);
            editWindow.Show();
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://sync.php/sync.php";
            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(url); request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";

            var json = JsonConvert.SerializeObject(new Package{Array = G.BadBoys, Deleted = G.Deleted});
            byte[] postBytes = Encoding.UTF8.GetBytes(json);

            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentLength = postBytes.Length;
            Stream requestStream = request.GetRequestStream();

            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch
            {
                return;
            }
            string result;
            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
            {
                result = rdr.ReadToEnd();
            }
            G.BadBoys = JsonConvert.DeserializeObject<Package>(result).Array;
            G.Deleted = new List<int>();
            this.dataGrid.ItemsSource = null;
            this.dataGrid.ItemsSource = G.BadBoys;
            G.SaveToFile();
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var toDelete = (BadBoy) (dataGrid.SelectedItem);
            if (G.Deleted != null)
                G.Deleted.Add(toDelete.id);
            G.BadBoys.RemoveAt(G.BadBoys.IndexOf(toDelete));
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = G.BadBoys;
        }

        /*private void dataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var toDelete = (BadBoy)(dataGrid.SelectedItem);
                G.Deleted.Add(toDelete.id);
                G.BadBoys.RemoveAt(G.BadBoys.IndexOf(toDelete));
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = G.BadBoys;
            }
        }*/

        private void MenuItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var toDelete = (BadBoy)(dataGrid.SelectedItem);
                G.Deleted.Add(toDelete.id);
                G.BadBoys.RemoveAt(G.BadBoys.IndexOf(toDelete));
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = G.BadBoys;
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new About();
            aboutWindow.Show();
        }

        private void SomeChartsButton_Click(object sender, RoutedEventArgs e)
        {
            var chartsWindow = new ChartsWindow();
            //WindowInteropHelper wih = new WindowInteropHelper(this);
            //wih.Owner = chartsForm.Handle;
            chartsWindow.Show();
        }
    }
}
