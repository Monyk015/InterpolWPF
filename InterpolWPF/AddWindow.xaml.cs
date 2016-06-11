using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace InterpolWPF
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private List<Crimes> crimesList;

        private bool dateChanged = false;

        public AddWindow(bool isSearch)
        {
            InitializeComponent();
            if (isSearch)
            {
                SaveButton.Content = "Find";
                SaveButton.Click -= SaveButton_Click;
                SaveButton.Click += FindButton_Click;
                this.Title = "Find...";
            }
            AccomplicesListBox.ItemsSource = G.BadBoys;
            treeView.ItemsSource = Crimes.AllCrimes;
            crimesList = new List<Crimes>();
            CrimesListBox.ItemsSource = crimesList;

        }

        private void FindButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            var toFind = ExtractBadBoy();
            ((MainWindow)App.Current.MainWindow).Find(toFind, dateChanged);
            Close();
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var target = (TextBox)e.Source;
            if (!Equals(target.Foreground, Brushes.Black))
            {
                var brush = target.Foreground;
                target.Foreground = Brushes.Black;
                string text = target.Text;
                target.Text = "";
                target.LostFocus += (o, args) =>
                {
                    if (target.Text == "")
                    {
                        target.Foreground = brush;
                        target.Text = text;
                    }
                };
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var toAdd = ExtractBadBoy();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.Add(toAdd);
            G.SaveToFile();
            Close();
            mainWindow.dataGrid.Items.Refresh();
        }

        public BadBoy ExtractBadBoy()
        {
            var badBoy = new BadBoy
            {
                Name = Equals(NameTextBox.Foreground, Brushes.Black) ? NameTextBox.Text : "",
                Surname = Equals(SurnameTextBox.Foreground, Brushes.Black) ? SurnameTextBox.Text : "",
                Nickname = Equals(NicknameTextBox.Foreground, Brushes.Black) ? NicknameTextBox.Text : "",
                Height = Equals(HeightTextBox.Foreground, Brushes.Black) ? HeightTextBox.Text : "0",
                EyeColor = Equals(EyeColorTextBox.Foreground, Brushes.Black) ? EyeColorTextBox.Text : "",
                BirthDate = dateChanged ? BirthDatePicker.SelectedDate.ToString() : DateTime.Now.ToString(),
                Citizenship = Equals(CitizenshipTextBox.Foreground, Brushes.Black) ? CitizenshipTextBox.Text : "",
                BirthPlace = Equals(BirthPlaceTextBox.Foreground, Brushes.Black) ? BirthPlaceTextBox.Text : "",
                DistinguishingTraits = Equals(DistinguishingTraitsTextBox.Foreground, Brushes.Black) ? DistinguishingTraitsTextBox.Text : "",
                Languages = Equals(LanguagesTextBox.Foreground, Brushes.Black) ? LanguagesTextBox.Text : "",
                HairColor = Equals(HairColorTextBox.Foreground, Brushes.Black) ? HairColorTextBox.Text : "",
                LastLivingPlace = Equals(LastLivingPlaceTextBox.Foreground, Brushes.Black) ? LastLivingPlaceTextBox.Text : "",
                Profession = Equals(ProfessionTextBox.Foreground, Brushes.Black) ? ProfessionTextBox.Text : "",
                LastCase = Equals(LastCaseTextBox.Foreground, Brushes.Black) ? LastCaseTextBox.Text : ""
            };
            var accomplices = new List<BadBoy>();
            foreach (var i in AccomplicesListBox.SelectedItems)
            {
               accomplices.Add((BadBoy) i);
            }
            badBoy.Accomplices = accomplices;
            badBoy.Crimes = crimesList;
            return badBoy;
        }

        private void BirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateChanged = true;
        }


        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
           
        }

        private void treeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            crimesList.Add((Crimes)(treeView.SelectedItem == Crimes.AllCrimes ? Crimes.AllCrimes[0] : treeView.SelectedItem));
            CrimesListBox.ItemsSource = null;
            CrimesListBox.ItemsSource = crimesList;
        }

        private void CrimesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            crimesList.Remove((Crimes)CrimesListBox.SelectedItem);
            CrimesListBox.Items.Refresh();
        }
    }

    
}
