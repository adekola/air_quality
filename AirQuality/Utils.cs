using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AirQuality
{
    public class Utils
    {
        public static Dictionary<String, String> mockData()
        {
            Random rand = new Random();

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("temperature", rand.Next(15,37).ToString());
            Thread.Sleep(100);//without this, you'd get just one value for each call to .Next()
            values.Add("humidity", rand.Next(15, 100).ToString());

            return values;
        }

        public static int parseData(string _data)
        {
            return int.Parse(_data);
        }


        ///
        /// Methods to implement
        /// 
        ///ComputePMV()
        ///ComputePPD()
        ///
        ///

        public static int ComputePMV(int temp) 
        {
            return 0;
        }

        public static int ComputePPD(int temp)
        {
            return 0;
        }


        public static int[] AverageSensorValues(int[] _tempValues, int[] _humidValues) //must be of the same length
        {
            if (_tempValues.Length != _humidValues.Length) 
            {
                return new int[] { 0, 0 };//return 0 values if both are not the same size
            }
            else
            {
                int tempCount = 0;
                int humidityCount = 0;
                int[] averages = new int[2];
                for (int i = 0; i < _tempValues.Length; i++)
                {
                    tempCount += _tempValues[i];
                    humidityCount += _humidValues[i];
                }

                averages[0] = (int)tempCount / _tempValues.Length;
                averages[1] = (int)humidityCount / _humidValues.Length;

                return averages; 
            }
        }

        public static bool EvaluateAirQuality(int _temp, int _humid) 
        {
            bool goodAirQuality = false;

            //Based on the ASHRAE standard for acceptable temeprature and humidity during winter...accessuble here
            //http://quickplace.aipl.uhp-nancy.fr/servlet/QuickrSupportUtil?type=quickrdownload&key=/LotusQuickr/perccom/PageLibraryC1257D650041F650.nsf/0/ACD5AEABD67F29E3C1257D78002BFC35/$file/Thermal%2520Comfort%2520for%2520Office%2520Work%2520_%2520OSH%2520Answers.pdf

            if (_humid >= 20 && _humid <= 70) //written in the standard as If Relative Humidity = 30%
            {
                if (_temp >= 20 && _temp <= 26)
                {
                    goodAirQuality = true;
                }
                else
                    goodAirQuality = false;
            }


            //if (_humid > 20 && _humid < 40) //written in the standard as If Relative Humidity = 30%
            //{
            //    if (_temp > 20 && _temp < 26)
            //    {
            //        goodAirQuality = true;
            //    }
            //    else
            //        goodAirQuality = false;
            //}

            //else if (_humid > 50 && _humid < 70) //written in the standard as If Relative Humidity = 60%
            //{
            //    if (_temp > 20 && _temp < 24)
            //    {
            //        goodAirQuality = true;
            //    }
            //    else
            //        goodAirQuality = false;
            //}

            return goodAirQuality;
        }




    }
}
