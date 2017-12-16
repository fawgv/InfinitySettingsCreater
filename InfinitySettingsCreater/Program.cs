using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySettingsCreater
{
    class Program
    {
        internal static string[] Args { get; set; }


        static void Main(string[] args)
        {


            Args = args;

            #region вывод аудио-устройств

            // ManagementObjectSearcher objSearcher = new ManagementObjectSearcher(
            //"SELECT * FROM Win32_SoundDevice");

            // ManagementObjectCollection objCollection = objSearcher.Get();

            // foreach (ManagementObject obj in objCollection)
            // {
            //     foreach (PropertyData property in obj.Properties)
            //     {
            //         Console.Out.WriteLine(String.Format("{0}:{1}", property.Name, property.Value));
            //     }
            // }Динамики (USB PnP Sound Device)




            #endregion

            #region Вывод ммедиа устройств

            string renderDevice = "";
            string captureDevice = "";

            var enumerator = new MMDeviceEnumerator();
            foreach (var endpoint in
                     enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                //if (endpoint.FriendlyName.ToLower().Contains("usb"))
                //{
                //    renderDevice = (endpoint.FriendlyName.Count() > 31 ? endpoint.FriendlyName.Remove(31) : endpoint.FriendlyName);
                //}
                Console.WriteLine(endpoint.FriendlyName);

            }

            foreach (var endpoint in
                     enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                //if (endpoint.FriendlyName.ToLower().Contains("usb"))
                //{
                //    captureDevice = (endpoint.FriendlyName.Count() > 31 ? endpoint.FriendlyName.Remove(31) : endpoint.FriendlyName);
                //}
                Console.WriteLine(endpoint.FriendlyName);
            }

            #endregion

            //LogicalClass bl = new LogicalClass();
            //Console.WriteLine(bl.MyHost);
            //Console.WriteLine(LogicalClass.SipHost);
            //Console.WriteLine(LogicalClass.GetIPHost());


            //SipPassClass dict = new SipPassClass();

            //try
            //{
            //    Console.WriteLine(dict.GetPassForSip(int.Parse(LogicalClass.SipHost)));
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("Необходимо использовать на ПК ЦВЗ с именем шаблона cvzr-xxxx");
            //}

            FileMethods fileMethods = new FileMethods();
            //Console.WriteLine(fileMethods.FilePath);

            //fileMethods.PrintFile();
            //Console.WriteLine(fileMethods.FindString("SIP", "Host="));

            #region рабочий вариант
            fileMethods.ReplaceFieldsInNewFile();
            #endregion


        }





    }
}
