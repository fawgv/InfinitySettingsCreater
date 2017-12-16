using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinitySettingsCreater
{
    internal class FileMethods
    {
        internal string FilePath { get; set; }

        private string curGroup = "";

        private SipPassClass sipPass = new SipPassClass();

        internal FileMethods()
        {
            try
            {
                if (!string.IsNullOrEmpty(Program.Args.FirstOrDefault()))
                {
                    FilePath = Program.Args.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                FilePath = "";
                Console.WriteLine("Нет аргументов");
            }

        }

        internal void PrintFile()
        {
            if (!string.IsNullOrEmpty(FilePath) && System.IO.File.Exists(FilePath))
            {
                using (StreamReader streamReader = new StreamReader(FilePath, Encoding.Default))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                    while (!streamReader.EndOfStream)
                    {
                        Console.WriteLine(streamReader.ReadLine());
                    }
                }
            }
        }

        internal void ReplaceFieldsInNewFile()
        {
            string outString = "";


            string renderDevice = "";
            string captureDevice = "";

            var enumerator = new MMDeviceEnumerator();
            foreach (var endpoint in
                     enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                if (endpoint.FriendlyName.ToLower().Contains("USB PnP"))
                {
                    renderDevice = (endpoint.FriendlyName.Count() > 31 ? endpoint.FriendlyName.Remove(31) : endpoint.FriendlyName);
                }

                if (endpoint.FriendlyName.ToLower().Contains("Lync USB"))
                {
                    renderDevice = (endpoint.FriendlyName.Count() > 31 ? endpoint.FriendlyName.Remove(31) : endpoint.FriendlyName);
                }

            }

            foreach (var endpoint in
                     enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                if (endpoint.FriendlyName.ToLower().Contains("USB PnP"))
                {
                    renderDevice = (endpoint.FriendlyName.Count() > 31 ? endpoint.FriendlyName.Remove(31) : endpoint.FriendlyName);
                }

                if (endpoint.FriendlyName.ToLower().Contains("Lync USB"))
                {
                    renderDevice = (endpoint.FriendlyName.Count() > 31 ? endpoint.FriendlyName.Remove(31) : endpoint.FriendlyName);
                }
            }


            if (!string.IsNullOrEmpty(FilePath) && System.IO.File.Exists(FilePath))
            {
                using (StreamWriter streamWriter = new StreamWriter($"{FilePath}_", false, Encoding.GetEncoding(1251)))
                {
                    using (StreamReader streamReader = new StreamReader(FilePath, Encoding.GetEncoding(1251)))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            string tempLine = streamReader.ReadLine();

                            if (tempLine.Contains($"["))
                            {
                                curGroup = tempLine;
                            }

                            #region Проработка Audio

                            tempLine = AddCorrectedDoubleString("Audio", "Mic=", "SpeakerMic=", $"Mic=\'{captureDevice}\'", tempLine);
                            tempLine = AddCorrectedDoubleString("Audio", "Headset=", "SpeakerHeadset=", $"Headset=\'{renderDevice}\'", tempLine);
                            tempLine = AddCorrectedString("Audio", "SpeakerMic=", $"SpeakerMic=\'{captureDevice}\'", tempLine);
                            tempLine = AddCorrectedString("Audio", "SpeakerHeadset=", $"SpeakerHeadset=\'{renderDevice}\'", tempLine);
                            tempLine = AddCorrectedDoubleString("Audio", "Ring=", "PlayBackRing=", $"Ring =\'{renderDevice}\'", tempLine);
                            tempLine = AddCorrectedString("Audio", "Tones=", $"Tones=\'{renderDevice}\'", tempLine);

                            #endregion


                            #region Проработка Main

                            tempLine = AddCorrectedString("Main", "Number=", $"Number={LogicalClass.SipHostInt}", tempLine);
                            tempLine = AddCorrectedString("Main", "Gateway=", "Gateway=call.dengisrazy.ru", tempLine);
                            tempLine = AddCorrectedString("Main", "Interface=", $"Interface={LogicalClass.GetIPHost()}", tempLine);

                            #endregion

                            tempLine = AddCorrectedString("SIP", "Host=", "Host=call.dengisrazy.ru", tempLine);
                            tempLine = AddCorrectedString("SIP", "Login=", $"Login={LogicalClass.SipHostInt}", tempLine);
                            tempLine = AddCorrectedString("SIP", "Password=", $"Password={sipPass.GetPassForSip(LogicalClass.SipHostInt)}", tempLine);

                            streamWriter.WriteLine(tempLine);
                        }
                    }
                }

                var pathNew = $"{FilePath}_";
                var pathArch = $"{FilePath}_arh";
                if (System.IO.File.Exists(FilePath) && (System.IO.File.Exists(pathNew)))
                {
                    System.IO.File.Replace(pathNew, FilePath, pathArch);
                }
            }
        }

        private string AddCorrectedString(string group, string field, string replaceField, string tempLine)
        {
            if (tempLine.Contains(field))
                if ($"[{group}]" == curGroup)
                    return replaceField;

            return tempLine;
        }

        private string AddCorrectedDoubleString(string group, string field, string notField, string replaceField, string tempLine)
        {
            if (tempLine.Contains(field) && !tempLine.Contains(notField))
                if ($"[{group}]" == curGroup)
                    return replaceField;

            return tempLine;
        }



        internal string FindString(string group, string field)
        {
            string outString = "";
            string curGroup = "";

            if (!string.IsNullOrEmpty(FilePath) && System.IO.File.Exists(FilePath))
            {
                using (StreamReader streamReader = new StreamReader(FilePath, Encoding.Default))
                {
                    //Console.WriteLine(streamReader.ReadToEnd());
                    while (!streamReader.EndOfStream)
                    {
                        string tempLine = streamReader.ReadLine();
                        if (tempLine.Contains($"["))
                        {
                            curGroup = tempLine;
                        }

                        if (tempLine.Contains(field))
                        {
                            if ($"[{group}]" == curGroup)
                            {
                                outString = tempLine;
                            }
                        }
                    }
                }
            }
            return outString;
        }

        void test()
        {
            var path = System.IO.Path.Combine("folder", @"AppData\Roaming\MicroSIP", "microsip.ini");
            var pathNew = System.IO.Path.Combine("folder", @"AppData\Roaming\MicroSIP", "microsip_new.ini");
            if (System.IO.File.Exists(path))
            {
                using (StreamWriter streamWriter = new StreamWriter(pathNew, false, Encoding.Default) /*File.Create(pathNew)*/)
                {
                    using (StreamReader fileRead = new StreamReader(path, Encoding.Default))
                    {
                        while (!fileRead.EndOfStream)
                        {
                            var tempLine = fileRead.ReadLine();
                            if (tempLine.Contains("audioCodecs"))
                            {
                                tempLine = "audioCodecs=G729/8000/1 GSM/8000/1 speex/8000/1 iLBC/8000/1 PCMA/8000/1";
                            }
                            if (tempLine.Contains("crashReport"))
                            {
                                tempLine = "crashReport=0";
                            }
                            streamWriter.WriteLine(tempLine);
                        }

                    }
                }

            }
        }
    }
}
