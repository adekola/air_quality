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

        //public static double ComputePMV() 
        //{
        //    double PA, CLO, MET, WME, TA, TR, VEL, RH, ICL, M, W, MW, FCL, HCF, TCL, PMV;

        //    if (PA == 0) 
        //    {
        //        PA = RH * 10 * fnps(TA);
        //    }


        //    ICL = 0.155 * CLO;
        //    M = MET * 58.15;
        //    W = WME * 58.15;
        //    MW = M - W;

        //    if (ICL <= .078)
        //    {
        //        FCL = 1;
        //    }
        //    else
        //        FCL = 1.05 + 0.0645 * ICL;

        //    HCF = 12.1 * Math.Sqrt(VEL);

        //    double TAA = TA + 273;
        //    double TRA = TR + 273;


        //    //calculate surface temperature of clothing by iteration
        //    int counter = 0;
        //    double XN, XF,HC;
        //    while (counter < 150) 
        //    {
        //        counter++;

        //        double TCLA = TAA + (35.5 - TA) / (3.5 * ICL + 0.1);
        //        double P1 = ICL * FCL;
        //        double P2 = P1 * 3.96;
        //        double P3 = P1 * 100;
        //        double P4 = P1 * TAA;
        //        double P5 = 308.7 - 0.28 * MW + P2 * (TRA / 100) * 4;
        //        XN = TCLA / 100;
        //        XF = XN;
        //    recompute:
        //        XF = (XF + XN) / 2;
        //        double HCN = 2.38 * Math.Pow((Math.Abs(100 * XF - TAA)), 0.25);
        //        if (HCF > HCN)
        //            HC = HCF;
        //        else
        //            HC = HCN;

        //        XN = (P5 + P4 * HC - P2 * Math.Pow(XF, 4)) / (100 + P3 * HC);

        //        if (Math.Abs(XN - XF) > 0.00015)
        //            goto recompute;

        //        //this may never execute...implement a way to set the value for TCL when counter > 150
        //        if (counter > 150){
        //            TCL = 99999;
        //        }
                    
        //        TCL = 100 * XN - 273;
        //    }


        //    //COMPUTE HEAT los components
        //    double HL2 = 0;
        //    double HL1 = 3.05 * 0.001 * (5733 - 6.99 * MW - PA);
        //    if (MW > 58.15)
        //        HL2 = 0.42 * (MW - 58.15);
        //    else
        //        HL2 = 0;

        //    double HL3 = 1.7 * 0.00001 * M * (5867 - PA);
        //    double HL4 = 0.0014 * M * (34 - TA);
        //    double HL5 = 3.96 * FCL * (Math.Pow(XN, 4) - TRA / Math.Pow(100, 4));

        //    //Now, to the proper PMV calculation
        //    double TS = .303 * Math.Exp(-.036 * M) + .028;
        //    PMV = TS * (MW - HL1 - HL2 - HL3 - HL4 - HL5);
            


            
        //    return PMV = 0;
        //}



        public static double calculatePMV(int temp)
        {
            //air velocity = 0.10, metabolic rate = 1.2, clothing = 1.0; relative humidity doesnot affect the PMV
            double pmvIndex = 0.00;
            if (temp < 16)
                pmvIndex = -2;
            else if (temp > 30)
                pmvIndex = 2;
            else
            {
                switch (temp)
                {

                    case 16://(temp < 16):
                        pmvIndex = -1.18;
                        break;
                    case 30:
                        pmvIndex = 1.86;
                        break;
                    case 17:
                        pmvIndex = -0.965;
                        break;
                    case 18:
                        pmvIndex = -0.75;
                        break;
                    case 19:
                        pmvIndex = -0.54;
                        break;
                    case 20:
                        pmvIndex = -0.33;
                        break;
                    case 21:
                        pmvIndex = -0.215;
                        break;
                    case 22:
                        pmvIndex = 0.1;
                        break;
                    case 23:
                        pmvIndex = 0.32;
                        break;
                    case 24:
                        pmvIndex = 0.54;
                        break;
                    case 25:
                        pmvIndex = 0.76;
                        break;
                    case 26:
                        pmvIndex = 0.98;
                        break;
                    case 27:
                        pmvIndex = 1.2;
                        break;
                    case 28:
                        pmvIndex = 1.42;
                        break;
                    case 29:
                        pmvIndex = 1.64;
                        break;
                }
            }
            return pmvIndex;


        }
        public static double calculatePPD(double pmvIndex)
        {
            double ppdindex = 100 - 95 * Math.Exp(-0.03353 * Math.Pow(pmvIndex, 4) - 0.2179 * pmvIndex * pmvIndex);
            return Math.Round(ppdindex, 2);

        }
        static double fnps(double t) 
        {
            return Math.Exp((16.6536 - 4030.183) / (t + 235));
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
