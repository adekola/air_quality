using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnmpSharpNet;
using System.Net;

namespace AirQuality
{
    class SnmpUtil
    {
        public static Dictionary<String, String> dataOIDs = null;

        //public static Dictionary<String, String> getData()
        //{

        //    dataOIDs = new Dictionary<string, string>();

        //    // SNMP community name
        //    OctetString community = new OctetString("perccom");

        //    // Define agent parameters class
        //    AgentParameters param = new AgentParameters(community);
        //    // Set SNMP version to 1 (or 2)
        //    param.Version = SnmpVersion.Ver2;
        //    // Construct the agent address object
        //    // IpAddress class is easy to use here because
        //    //  it will try to resolve constructor parameter if it doesn't
        //    //  parse to an IP address
        //    IpAddress agent = new IpAddress("127.0.0.1");

        //    // Construct target
        //    UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 3);


        //    dataOIDs.Add("ComputerName", "1.3.6.1.2.1.1.5.0");
        //    dataOIDs.Add("CurrentDateTime" , "1.3.6.1.2.1.25.1.2.0");
        //    dataOIDs.Add("SystemUpTime", "1.3.6.1.2.1.1.3.0");

        //    // Pdu class used for all requests
        //    Pdu pdu = new Pdu(PduType.Get);

        //    string val = null;
        //    foreach (var k in dataOIDs.Keys) 
        //    {
        //        dataOIDs.TryGetValue(k, out val );
        //        pdu.VbList.Add(val);
        //    }

        //    try
        //    {

        //        // Make SNMP request
        //        SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);

        //        Dictionary<String, String> values = new Dictionary<string, string>();
        //        // If result is null then agent didn't reply or we couldn't parse the reply.
        //        if (result != null)
        //        {
        //            // ErrorStatus other then 0 is an error returned by 
        //            // the Agent - see SnmpConstants for error definitions
        //            if (result.Pdu.ErrorStatus != 0)
        //            {
        //                // agent reported an error with the request
        //                Console.WriteLine("Error in SNMP reply. Error {0} index {1}",
        //                    result.Pdu.ErrorStatus,
        //                    result.Pdu.ErrorIndex);
        //            }
        //            else
        //            {
        //                string[] keysArray = dataOIDs.Keys.ToArray<String>(); //enumerate the keys as an array
        //                for (int x = 0; x < result.Pdu.VbList.Count; x++ )
        //                {
        //                    values[keysArray[x]] = result.Pdu.VbList[x].ToString(); // get the key name from the array and match to what was returned from the OID request
        //                }
        //                //for (int x = 0; x < result.Pdu.VbList.Count; x++)
        //                //{

                            
        //                //    //values[x] = result.Pdu.VbList[x].ToString();
        //                //}
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No response received from SNMP agent.");
        //        }
        //        target.Close();

        //        return values;
        //    }
        //    catch (SnmpException ex)
        //    {
        //        throw ex;
        //    }
        //}


        public static Dictionary<String, String> getData(Dictionary<String, String> OIDs, string ipAddress)
        {

            // SNMP community name
            OctetString community = new OctetString("perccom");

            // Define agent parameters class
            AgentParameters param = new AgentParameters(community);
            // Set SNMP version to 1 (or 2)
            param.Version = SnmpVersion.Ver2;
            // Construct the agent address object
            // IpAddress class is easy to use here because
            //  it will try to resolve constructor parameter if it doesn't
            //  parse to an IP address
            IpAddress agent = new IpAddress(ipAddress);

            // Construct target
            UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 3);

            // Pdu class used for all requests
            Pdu pdu = new Pdu(PduType.Get);

            string val = null;
            foreach (var k in OIDs.Keys)
            {
                OIDs.TryGetValue(k, out val);
                pdu.VbList.Add(val);
            }

            try
            {

                // Make SNMP request
                SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);

                Dictionary<String, String> values = new Dictionary<string, string>();
                // If result is null then agent didn't reply or we couldn't parse the reply.
                if (result != null)
                {
                    // ErrorStatus other then 0 is an error returned by 
                    // the Agent - see SnmpConstants for error definitions
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        // agent reported an error with the request
                        Console.WriteLine("Error in SNMP reply. Error {0} index {1}",
                            result.Pdu.ErrorStatus,
                            result.Pdu.ErrorIndex);
                    }
                    else
                    {
                        string[] keysArray = OIDs.Keys.ToArray<String>(); //enumerate the keys as an array
                        for (int x = 0; x < result.Pdu.VbList.Count; x++)
                        {
                            values[keysArray[x]] = result.Pdu.VbList[x].ToString(); // get the key name from the array and match to what was returned from the OID request
                        }
                        //for (int x = 0; x < result.Pdu.VbList.Count; x++)
                        //{


                        //    //values[x] = result.Pdu.VbList[x].ToString();
                        //}
                    }
                }
                else
                {
                    Console.WriteLine("No response received from SNMP agent.");
                }
                target.Close();

                return values;
            }
            catch (SnmpException ex)
            {
                throw ex;
            }
        }
    
    }
}
