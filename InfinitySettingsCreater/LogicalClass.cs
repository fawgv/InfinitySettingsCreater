using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySettingsCreater
{
    internal class LogicalClass
    {
        private static string myHost = System.Net.Dns.GetHostName();

        public static List<System.Net.IPAddress> MyIPArray { get { return System.Net.Dns.GetHostEntry(myHost).AddressList.ToList(); } set { } }

        private string pathApplication = AppDomain.CurrentDomain.BaseDirectory;

        public string MyHost
        {
            get
            {
                return myHost;
            }

        }

        public static string IPHost
        {
            get
            {
                return GetIPHost();
            }
        }

        public static string SipHost
        {
            get
            {
                return $"1{myHost.Remove(0, 5)}";
            }
        }

        public static int SipHostInt
        {
            get
            {
                try
                {
                    return int.Parse($"1{myHost.Remove(0, 5)}");
                }
                catch (Exception)
                {
                    return 99999;
                }
                
            }
        }

        internal static string GetIPHost()
        {
            for (int curIP = 0; curIP < MyIPArray.Count; curIP++)
            {
                try
                {
                    var txtIP = MyIPArray[curIP].ToString();
                    var tmpPer = txtIP.Split('.');
                    if (tmpPer.Count() == 4)
                    {
                        return txtIP;
                    }

                }
                catch (Exception)
                {
                }
            }

            return "";

        }
    }
}
