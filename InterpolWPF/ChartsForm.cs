using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

namespace InterpolWPF
{
    public partial class ChartsForm : Form
    {
        public ChartsForm()
        {
            InitializeComponent();
            var data =
                G.BadBoys.GroupBy(b => DateTime.Now.Year - b._birthDate.Year).Where(boys => boys.Key != 0)
                    .Select(boys => new {Key = boys.Key, Value = boys.Count()});
            foreach (var i in data)
            {
                Series series = this.chart1.Series.Add(i.Key.ToString());
                series.Points.AddXY(i.Key, i.Value);
            }
            var crimes = new List<Crimes>();
            G.BadBoys.ForEach(x => x.Crimes.ForEach(y => crimes.Add(y)));
            var countsOfCrimes = crimes.GroupBy(x => x.Name).Select(crime => new {Key = crime.Key, Value = crime.Count()});
            foreach (var i in countsOfCrimes)
            {
                Series series = this.chart2.Series.Add(i.Key);
                series.Points.AddY(i.Value);
            }
            chart2.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
