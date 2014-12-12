using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnmpSharpNet;
using System.Configuration;
using System.Management;


namespace AirQuality
{
    public partial class Home : Form
    {

        Dictionary<String, String> oids = null;
        string russiaSensorAddress = "";
        string swedenSensorAddress = "";
        string finlandSensorAddress = "";
        string franceSensorAddress = "";
        
        //used to track the accumulation of sensor data into the global one
        int dataCount = 0;
        //A dictionary of string-keyed array of integer lists
        Dictionary<string, List<int>[]> dataValues = null;

        public Home()
        {
            InitializeComponent();
            setUp();
            initGlobalData();
            showData();
        }

        public void setUp() {
            oids = new Dictionary<string, string>();

            oids.Add("temperature", ".1.3.6.1.4.1.3854.1.2.2.1.16.1.3.0");
            oids.Add("humidity", ".1.3.6.1.4.1.3854.1.2.2.1.17.1.3.0");

            russiaSensorAddress = ConfigurationSettings.AppSettings["russiaSensorAddress"].ToString();
            swedenSensorAddress = ConfigurationSettings.AppSettings["swedenSensorAddress"].ToString();
            finlandSensorAddress = ConfigurationSettings.AppSettings["finlandSensorAddress"].ToString();
            franceSensorAddress = ConfigurationSettings.AppSettings["franceSensorAddress"].ToString();


            if (russiaSensorAddress == null)
            {
                MessageBox.Show("The IP address of Sensor in Russia Cabinet is not set!!");
            }
        }

        //sets up the dictionary to be used to store accumulated data from the sensor to compute averages each 30 seconds
        void initGlobalData() 
        {
            dataValues = new Dictionary<string, List<int>[]>();
            //just an initially empty array of integer lists to initilaize the dictionary with
            List<int>[] empty = new List<int>[2] {new List<int>(), new List<int>() };
            dataValues.Add("russia", empty);
            dataValues.Add("france", empty);
            dataValues.Add("finland", empty);
            dataValues.Add("sweden", empty);
        }

        //updates the global collection of sensor data
        void updateGlobalData(string _key, int _temp, int _humid) 
        {
            dataValues[_key][0].Add(_temp);
            dataValues[_key][1].Add(_humid);
        }


        //Shows data from the four Cabinets, and updates the global collection of sensor values with the data retrieved each time
        public void showData() {

            //mock data generated randomly
            Dictionary<String, String> russiaData = Utils.mockData(); //SnmpUtil.getData(oids, russiaSensorAddress);
            
            Dictionary<String, String> franceData = Utils.mockData(); //SnmpUtil.getData(oids, franceSensorAddress);

            Dictionary<String, String> finlandData = Utils.mockData(); //SnmpUtil.getData(oids, finlandSensorAddress);

            Dictionary<String, String> swedenData = Utils.mockData(); //SnmpUtil.getData(oids, swedenSensorAddress);


            //Russia Data
            if (russiaData != null) //if we are able to get some data
            {
                int temp = int.Parse(russiaData["temperature"]);
                int humidity = int.Parse(russiaData["humidity"]);

                //update global data collection
                updateGlobalData("russia", temp, humidity);

                lblTempRussia.Text = temp.ToString();
                lblHumidityRussia.Text = humidity.ToString();

                var goodAirQuality = Utils.EvaluateAirQuality(temp, humidity);

                if (goodAirQuality)
                {
                    lblRussiaAirQuality.Text = "Good";
                }
                else
                    lblRussiaAirQuality.Text = "Bad";
            }
            else
                MessageBox.Show("Russia Data not available");

            //Finalnd Data 
            if (finlandData != null) //if we are able to get soome data
            {
                int temp = int.Parse(finlandData["temperature"]);
                int humidity = int.Parse(finlandData["humidity"]);

                //int pmx = Utils.EstimatePMV(temp);

                //update global data collection
                updateGlobalData("finland", temp, humidity);

                lblTempFinland.Text = temp.ToString();
                lblHumidityFinland.Text = humidity.ToString();

                var goodAirQuality = Utils.EvaluateAirQuality(temp, humidity);

                if (goodAirQuality)
                {
                    lblFinlandAirQuality.Text = "Good";
                }
                else
                    lblFinlandAirQuality.Text = "Bad";
            }
            else
                MessageBox.Show("Finland Data not available");

            //France Data 
            if (franceData != null) //if we are able to get soome data
            {
                int temp = int.Parse(franceData["temperature"]);
                int humidity = int.Parse(franceData["humidity"]);

                //update global data collection
                updateGlobalData("france", temp, humidity);

                lblTempFrance.Text = temp.ToString();
                lblHumidityFrance.Text = humidity.ToString();

                var goodAirQuality = Utils.EvaluateAirQuality(temp, humidity);

                if (goodAirQuality)
                {
                    lblFranceAirQuality.Text = "Good";
                }
                else
                    lblFranceAirQuality.Text = "Bad";
            }
            else
                MessageBox.Show("France Data not available");

            //Sweden Data
            if (swedenData != null) //if we are able to get soome data
            {
                int temp = int.Parse(swedenData["temperature"]);
                int humidity = int.Parse(swedenData["humidity"]);

                //update global data collection
                updateGlobalData("sweden", temp, humidity);

                lblTemperatureSweden.Text = temp.ToString();
                lblHumiditySweden.Text = humidity.ToString();

                var goodAirQuality = Utils.EvaluateAirQuality(temp, humidity);

                if (goodAirQuality)
                {
                    lblSwedenAirQuality.Text = "Good";
                }
                else
                    lblSwedenAirQuality.Text = "Bad";
            }
            else
                MessageBox.Show("Sweden Data not available");
        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            showData();
        }

        private void btnPlotData_Click(object sender, EventArgs e)
        {
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dataCount++;
            showData(); //method which shows new sensor data and accumulats it into the global data collection to be compute averages every 30seconds
            if (dataCount == 10) 
            {
                computeGlobalAverages();
                //reset my dear!!!
                dataCount = 0;
                initGlobalData();
            }
        }

        void computeGlobalAverages() 
        {
            //first average the values from each cabinet
            int[] avgRussia = Utils.AverageSensorValues(dataValues["russia"][0].ToArray(), dataValues["russia"][1].ToArray());
            int[] avgFinland = Utils.AverageSensorValues(dataValues["finland"][0].ToArray(), dataValues["finland"][1].ToArray());
            int[] avgSweden = Utils.AverageSensorValues(dataValues["sweden"][0].ToArray(), dataValues["sweden"][1].ToArray());
            int[] avgFrance = Utils.AverageSensorValues(dataValues["france"][0].ToArray(), dataValues["france"][1].ToArray());

            //then array the averages of each cabinet
            int[] avgTemperatures = new int[] { avgRussia[0], avgFinland[0], avgSweden[0], avgFrance[0] };
            int[] avgHumidities = new int[] { avgRussia[1], avgFinland[1], avgSweden[1], avgFrance[1] };

            //now, compute the global average
            int[] globalAverages = Utils.AverageSensorValues(avgTemperatures, avgHumidities);

            showGlobalAverages(globalAverages);
        }

        void showGlobalAverages(int[] _averages) 
        {
            lblAvgTemperature.Text = _averages[0].ToString();
            lblAvgHumidity.Text = _averages[1].ToString();

            var goodAir = Utils.EvaluateAirQuality(_averages[0], _averages[1]);

            if (goodAir)
            {
                lblGlobalAirQuality.BackColor = Color.ForestGreen;
                lblGlobalAirQuality.Text = "Good :) ";
            }
            else
            {
                lblGlobalAirQuality.BackColor = Color.Red;
                lblGlobalAirQuality.Text = "Bad :( ";
            }
        }

        private void lblRussiaAirQuality_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            graphData(russiaSensorAddress, oids, "Air Quality Graph - Russia", Color.IndianRed);
        }

        void graphData() 
        {
            Graph gra = new Graph("Air Quality Graph - Global");
            gra.ShowDialog();
        }
        private void graphData(string _ipAddress, Dictionary<string, string> _oids, string _text, Color _color )
        {
            Graph gra = new Graph(_ipAddress, _oids, _text, _color);
            gra.ShowDialog();
        }

        private void lblSwedenAirQuality_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            graphData(swedenSensorAddress, oids, "Air Quality Graph - Sweden", Color.Yellow);
        }

        private void lblFranceAirQuality_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            graphData(franceSensorAddress, oids, "Air Quality Graph - France", Color.LightSkyBlue);
        }

        private void lblFinlandAirQuality_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            graphData(finlandSensorAddress, oids, "Air Quality Graph - Finland", Color.White);
        }


    }
}
