using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AirQuality
{
    public partial class Graph : Form
    {
        string _ipAddress = "";
        Dictionary<String, String> _oids;

        //for the general graph 
        public Graph(string text) 
        {
            this.Text = text;
        }

        //for the country specific graph
        public Graph(string ipAddress, Dictionary<String, String> oids, string text, Color color)
        {
            _ipAddress = ipAddress;
            _oids = oids;
            InitializeComponent();
            this.BackColor = color;
            updateData();
            lblText.Text = text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //every tick get new data and augment the series collection of the chart
            updateData();
        }

        void updateData()
        {
            // Two items in the dictionary per request
            Dictionary<String, String> data = Utils.mockData(); //SnmpUtil.getData(_oids, _ipAddress);
            int temp = Utils.parseData(data["temperature"]);
            double pmvIndex = Utils.calculatePMV(temp);
            lblPMV.Text = pmvIndex.ToString();
            lblPPD.Text = Utils.calculatePPD(pmvIndex).ToString();
            russianChart.Series["temperature"].Points.AddY(temp);
            russianChart.Series["humidity"].Points.AddY(Utils.parseData(data["humidity"]));
        }

      }
}
