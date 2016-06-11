using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InterpolWPF
{
    /// <summary>
    /// Логика взаимодействия для ChartsWindow.xaml
    /// </summary>
    public partial class ChartsWindow : Window
    {
        public List<Crimes> crimes;

        public ChartsWindow()
        {
            InitializeComponent();
            this.chart1.Size = new System.Drawing.Size(655, 289);
            this.chart2.Size = new System.Drawing.Size(500, 289);
            chart1.ChartAreas.Add("MainArea");
            chart2.ChartAreas.Add("MainArea");
            var data =
            G.BadBoys.GroupBy(b => DateTime.Now.Year - b._birthDate.Year).Where(boys => boys.Key != 0)
            .Select(boys => new { Key = boys.Key, Value = boys.Count() });
            foreach (var i in data)
            {
                Series series = this.chart1.Series.Add(i.Key.ToString());
                series.Points.AddXY(i.Key, i.Value);
            }
            crimes = new List<Crimes>();
            G.BadBoys.ForEach(x => x.Crimes.ForEach(y => crimes.Add(y)));
            chart2.Series.Add(getCounts(Crimes.AllCrimes[0], crimes));
            chart2.Legends.Add("MainLegend");
            // chart2.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            treeView.Items.Add(Crimes.AllCrimes);
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selected =
                (Crimes) (treeView.SelectedItem == Crimes.AllCrimes ? Crimes.AllCrimes[0] : treeView.SelectedItem);
            if (selected.Children == null)
                return;
            chart2.Series[0] = getCounts(selected, crimes);
        }

        private Series getCounts(Crimes section, List<Crimes> crimes)
        {
            var counts =
                Crimes.AllCrimes[0].findWithCode(section).Children.Select(
                    crime => new {Name = crime.Name, Count = crimes.Where(crime.isSuccessor).Count()}).ToList();
            counts.Add(new {Name = "Not determined", Count = crimes.Count(crime => crime.Name == section.Name)});
            Series toReturn = new Series();
            toReturn.ChartType = SeriesChartType.Pie;
            foreach (var i in counts)
            {
                if(i.Count == 0)
                    continue;
                toReturn.Points.Add(i.Count);
                toReturn.Points.Last().AxisLabel = i.Count.ToString();
                toReturn.Points.Last().LegendText = i.Name;
            }
            return toReturn;
        }
    }
}
