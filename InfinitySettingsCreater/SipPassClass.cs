using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySettingsCreater
{
    internal class SipPassClass
    {
        public Dictionary<int,string> DictSipPass { get; set; }

        public SipPassClass()
        {
            DictSipPass = new Dictionary<int, string>();
            string[] text = Properties.Resources.arraySipPass.Split('\n');

            foreach (var item in text)
            {
                string[] dictItem = item.Split(' ');
                DictSipPass.Add(int.Parse(dictItem[0]), dictItem[1]);
                
            }

        }

        public string GetPassForSip(int sip)
        {
            foreach (var item in DictSipPass)
            {
                try
                {
                    if (item.Key == sip)
                    {
                        return item.Value;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Необходимо использовать на ПК ЦВЗ с именем шаблона cvzr-xxxx");
                }
            }

            return "";
        }


    }
}
