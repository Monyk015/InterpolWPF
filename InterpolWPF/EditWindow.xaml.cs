using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private BadBoy current;

        public EditWindow(BadBoy badBoy)
        {
            InitializeComponent();
            current = badBoy;
            var properties = typeof(BadBoy).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name != "Accomplices" && property.Name != "BirthDate" && property.Name != "crimes" && property.Name != "Crimes")
                {
                    var textBox =
                        (TextBox)
                            (this.GetType()
                                .GetField(property.Name + "TextBox", BindingFlags.NonPublic | BindingFlags.Instance)?
                                .GetValue(this));
                    textBox.Text = property.GetValue(badBoy).ToString();
                }
            }
            AccomplicesListBox.ItemsSource = G.BadBoys;
            foreach (var accomplice in badBoy.Accomplices)
            {
                AccomplicesListBox.SelectedItems.Add(accomplice);
            }
            BirthDatePicker.DisplayDate = badBoy._birthDate;
            BirthDatePicker.Text = BirthDatePicker.DisplayDate.ToString();
            CrimesListBox.ItemsSource = current.Crimes;
            treeView.Items.Add(Crimes.AllCrimes);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            /*var target = (TextBox)e.Source;
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
            }*/
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ExtractBadBoy();
            MainWindow mainWindow = (MainWindow) App.Current.MainWindow;
            mainWindow.dataGrid.Items.Refresh();
            G.SaveToFile();
            Close();
        }

        public void ExtractBadBoy()
        {
            current.Name = Equals(NameTextBox.Foreground, Brushes.Black) ? NameTextBox.Text : "";
            current.Surname = Equals(SurnameTextBox.Foreground, Brushes.Black) ? SurnameTextBox.Text : "";
            current.Nickname = Equals(NicknameTextBox.Foreground, Brushes.Black) ? NicknameTextBox.Text : "";
            current.Height = Equals(HeightTextBox.Foreground, Brushes.Black) ? HeightTextBox.Text : "0";
            current.EyeColor = Equals(EyeColorTextBox.Foreground, Brushes.Black) ? EyeColorTextBox.Text : "";
            current.BirthDate = BirthDatePicker.DisplayDate.ToString();
            current.Citizenship = Equals(CitizenshipTextBox.Foreground, Brushes.Black)
                ? CitizenshipTextBox.Text
                : "";
            current.BirthPlace = Equals(BirthPlaceTextBox.Foreground, Brushes.Black) ? BirthPlaceTextBox.Text : "";
            current.DistinguishingTraits =
                Equals(DistinguishingTraitsTextBox.Foreground, Brushes.Black)
                    ? DistinguishingTraitsTextBox.Text
                    : "";
            current.Languages = Equals(LanguagesTextBox.Foreground, Brushes.Black) ? LanguagesTextBox.Text : "";
            current.HairColor = Equals(HairColorTextBox.Foreground, Brushes.Black) ? HairColorTextBox.Text : "";
            current.LastLivingPlace = Equals(LastLivingPlaceTextBox.Foreground, Brushes.Black)
                ? LastLivingPlaceTextBox.Text
                : "";
            current.Profession = Equals(ProfessionTextBox.Foreground, Brushes.Black) ? ProfessionTextBox.Text : "";
            current.LastCase = Equals(LastCaseTextBox.Foreground, Brushes.Black) ? LastCaseTextBox.Text : "";

            var accomplices = new List<BadBoy>();
            foreach (var i in AccomplicesListBox.SelectedItems)
            {
                accomplices.Add((BadBoy) i);
            }
            current.Accomplices = accomplices;
        }

        private void treeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            current.Crimes.Add((Crimes)(treeView.SelectedItem == Crimes.AllCrimes ? Crimes.AllCrimes[0] : treeView.SelectedItem));
            CrimesListBox.ItemsSource = null;
            CrimesListBox.ItemsSource = current.Crimes;
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CrimesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            current.Crimes.Remove((Crimes) CrimesListBox.SelectedItem);
            CrimesListBox.Items.Refresh();
        }
    }
}