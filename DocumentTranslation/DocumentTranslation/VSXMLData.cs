using Newtonsoft.Json.Linq;
using Renlen.BaseLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

namespace DocumentTranslation
{
    internal class VSXMLData : XmlDocument
    {
        public event ProgressChangeEventHandler ProgressChange;
        public delegate void ProgressChangeEventHandler(VSXMLData sender, (bool IsCompleted, int Progress, int MaxValue, string Member) e);
        public event EventHandler Canceled;
        public bool Pause { get; set; }
        private bool Cancel { get; set; }
        public bool IsUpSuccess { get; private set; }

        public int MaxValue { get; private set; }
        public int CurrentValue { get; private set; }
        public long Length { get; private set; }

        private int ZWFCount = 0;
        /// <summary>
        /// 占位符
        /// </summary>
        private Dictionary<string, string> ZWF = new Dictionary<string, string>();
        private string GetZWF(string name)
        {
            return ZWF.TryGetValue(name, out string value) ? value : null;
        }
        private void SetZWF(string name, string value)
        {
            if (ZWF.ContainsKey(name))
            {
                ZWF[name] = value;
            }
            else
            {
                ZWF.Add(name, value);
            }
        }
        private bool AddToZWF(IEnumerable<string> arr)
        {
            ZWFCount = 0;
            foreach (string item in arr)
            {
                if (ZWFCount > 999)
                {
                    ZWFCount = 0;
                    return false;
                }
                SetZWF("@" + ZWFCount.ToString("D3"), item);
                ZWFCount++;
            }
            return true;
        }
        private string ToZWF(string str)
        {
            string key;
            for (int i = 0; i < ZWFCount; i++)
            {
                key = "@" + i.ToString("D3");
                str = str.Replace(GetZWF(key) ?? "&noneexists;", key);
            }
            return str;
        }
        private string ToValue(string str)
        {
            string key;
            for (int i = 0; i < ZWFCount; i++)
            {
                key = "@" + i.ToString("D3");
                str = str.Replace(key, GetZWF(key));
            }
            return str;
        }
        private IEnumerable<XmlElement> GetMembersAll(OutputConfig oc)
        {
            var node = DocumentElement["members"];
            if (node == null)
            {
                ProgressChange?.Invoke(this, (true, CurrentValue, MaxValue, ""));
                yield break;
            }
            MaxValue = node.ChildNodes.Count;
            CurrentValue = 0;
            Length = 0;
            string name;
            foreach (XmlElement member in node.ChildNodes)
            {
                name = member.GetAttribute("name") ?? "-";
                CurrentValue++;
                ProgressChange?.Invoke(this, (false, CurrentValue, MaxValue, name));
                if (TP != null && TP.CheckMember(name))
                {
                    continue;
                }
                yield return member;
                if (IsUpSuccess)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        try { this.Save(TP.TempPath); break; }
                        catch (Exception ex) { GlobalData.GData.Log.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} : ({i}) {ex.Message} ( At {TP.TempPath} )"); }
                    }
                    TP.Stream.Position = TP.Position;
                    using (BinaryWriter bw = new BinaryWriter(TP.Stream, Encoding.UTF8, true))
                    {
                        bw.Write(name);
                        TP.Position = TP.Stream.Position;
                        bw.Write("");
                    }
                }
                //XmlElement xe = member["summary"];
                //if (xe == null) continue;
                //yield return xe;
                ////yield return xe.OuterXml;
                //foreach (XmlElement param in member.SelectNodes("param"))
                //{
                //  yield return param;
                //}
                //foreach (XmlElement exception in member.SelectNodes("exception"))
                //{
                //  yield return exception;
                //}
            }
            oc.Check(this.BaseURI, out bool isbackup, out string outpath, out string backuppath);
            if (isbackup)
            {
                File.Move(BaseURI, backuppath);
            }
            Save(outpath);
            TP.Dispose();
            if (File.Exists(TP.TPFPath))
                File.Delete(TP.TPFPath);
            if (File.Exists(TP.TempPath))
                File.Delete(TP.TempPath);
            TP = null;
            //GlobalData.GData.Form_Main.ShowDialog(Length.ToString());
            ProgressChange?.Invoke(this, (true, CurrentValue, MaxValue, ""));
        }

        private TranslationProgress TP = null;
        public void CreateTranslationProgress()
        {
            if (GlobalData.GData.Baidu == null)
                throw new Exception();
            XmlElement xe = this.DocumentElement["assembly"];
            TP = new TranslationProgress(this.BaseURI, xe.OuterXml.GetMD5(), GlobalData.GData.Baidu.TargetLanguage);
        }
        public void StartTranslation(OutputConfig oc)
        {
            //if (GlobalData.GData.Baidu == null)
            //  throw new Exception();
            GlobalData.GData.Baidu.Canceled += (sender, e) =>
              {
                  Cancel = true;
              };
            Directory.CreateDirectory("Temp");
            if (TP != null)
            {
                if (File.Exists(TP.TPFPath) && File.Exists(TP.TempPath))
                {
                    TP.Start();
                    Load(TP.TempPath);
                }
                else
                {
                    File.WriteAllText(TP.TPFPath, "\0");
                    TP.Start();
                }
            }
            //Regex regexXMLone = new Regex(@"<\s*([a-zA-Z]+)\s*(?:\s+.+=.+)*\s*/s*>");
            //Regex regexXMLtwo = new Regex(@"<\s*([a-zA-Z]+)\s*(?:\s+.+=.+)*\s*>[.\n\r]*<\s*/\s*\1\s*>");

            Task task = Task.Run(() =>
             {
                 foreach (XmlElement member in GetMembersAll(oc))
                 {
                     try
                     {
                         TranslationMember(member);
                         IsUpSuccess = true;
                     }
                     catch (Exception ex)
                     {
                         IsUpSuccess = false;
                         GlobalData.GData.Log.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} : {ex.Message} ( At {member.GetAttribute("name") ?? "-"} )");
                     }
                     if (Cancel)
                     {
                         Canceled?.Invoke(this, new EventArgs());
                         Cancel = false;
                         TP?.Dispose();
                         TP = null;
                         break;
                     }
                 }
             });
            //await task.ContinueWith(t => Save(@"Test\ttt.xml"));
            //task.Start();
        }

        private void TranslationMember(XmlElement member)
        {
            (ResultCode ResultCode, string Value) result;
            XmlNode xn;
            int num;
            foreach (XmlElement element in GetElements())
            {
                if (Cancel) return;
                num = 0;
            retry:
                num++;
                //if (num > 1)
                //{
                //  
                //}
                while (Pause)
                {
                    Thread.Sleep(1000);
                }
                result = TranslationElement(element);
                if (result.ResultCode == ResultCode.Success)
                {
                    xn = element.CloneNode(false);
                    xn.InnerXml = result.Value.Replace('；', ';');
                    //xn = CreateElement(element.Name);
                    member.ReplaceChild(xn, element);
                }
                else
                {
                    switch (result.ResultCode)
                    {
                        //case ResultCode.NetworkError:
                        //case ResultCode.SignatureError:
                        //case ResultCode.ClientIPIsIllegal:
                        //case ResultCode.LanguageIsNonSupported:
                        //case ResultCode.ServiceIsCurrentlyClosed:
                        //  break;
                        case ResultCode.QequestTimedOut:
                            goto retry;
                        case ResultCode.LimitedAccessFrequency:
                            Thread.Sleep(num * 1000);
                            goto retry;
                        //break;
                        case ResultCode.LongRequestFrequent:
                            Thread.Sleep(num * 1000);
                            goto retry;
                        default:
                            //DialogResult dr = GlobalData.GData.Form_Main.ShowDialog
                            //  ($"出现错误。是否立即结束？\r\n\r\n{result.Value}"
                            //  , buttons: MessageBoxButtons.AbortRetryIgnore
                            //  );
                            //if (dr == DialogResult.Abort)
                            //{
                            //  Cancel = true;
                            //}
                            //else if (dr == DialogResult.Retry)
                            //{
                            //  goto retry;
                            //}
                            GlobalData.GData.Log.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} : {result.Value} ( At {member.GetAttribute("name") ?? "-"} )");
                            //GlobalData.GData.Form_Main.Pause();
                            break;
                    }
                }
            }

            IEnumerable<XmlElement> GetElements()
            {
                XmlElement xe = member["summary"];
                if (xe != null) yield return xe;
                foreach (XmlElement param in member.SelectNodes("param"))
                {
                    if (param != null) yield return param;
                }
                foreach (XmlElement exception in member.SelectNodes("exception"))
                {
                    if (exception != null) yield return exception;
                }
                foreach (XmlElement returns in member.SelectNodes("returns"))
                {
                    if (returns != null) yield return returns;
                }
            }
        }
        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        private (ResultCode ResultCode, string Value) TranslationElement(XmlElement xe)
        {
            Translation translation = GlobalData.GData.Baidu;
            JObject result;
            string xml = xe.InnerXml;
            if (xe.ChildNodes.Count > 1 || (xe.ChildNodes.Count == 1 && xe.ChildNodes[0].Name != "#text"))
            {
                var v = from XmlNode d in xe.ChildNodes
                        where d.Name != "#text"
                        //where d.Name == "see" || d.Name == "seealso" || d.Name == "paramref"
                        select d;
                AddToZWF(v.Select(d => d.OuterXml));
                //string s = ToZWF(xml);
                //s = ToValue(s);
                //yield return (true, s);
                //continue;
                //result = translation.TranslationString(ToZWF(xml));
                return GetResult(true);
            }
            else
            {
                //yield return (true, "test");
                //continue;
                //result = translation.TranslationString(xml);
                return GetResult(false);
            }
            (ResultCode, string) GetResult(bool isZWF)
            {
                string txml = isZWF ? ToZWF(xml) : xml;
                Length += txml.Length;
                result = JsonDocument.LoadJson(translation.TranslationString(txml));
                if (result == null) return (ResultCode.SystemError, GlobalData.GData.Data["Dim:System.Caption", "Caption_MAX", "每月翻译字数将要达到上限，用户选择停止。"]);
                if (Check(result, out var error_code, out var src, out var dst))
                {
                    return (ResultCode.Success, $"{(isZWF ? ToValue(dst) : dst)}<para>{GlobalData.GData.Data["Dim:System.Other", "Original", "原文："]} {xml}</para>");
                }
                else
                {
                    string error = error_code.ToString();
                    return (error_code, GlobalData.GData.Data["Enum:Renlen.BaseLibrary.ResultCode", error, error]);
                }
            }
        }


        public ResultCode Check(JObject obj)
        {
            //var obj = TranslationString("结果", Language.zh, Language.en);
            if (obj.TryGetValue("error_code", out var value))
            {
                return (ResultCode)(int.Parse(value.ToString()));
            }
            else return ResultCode.Success;
        }

        private bool Check(JObject obj, out ResultCode error_code, out string src, out string dst)
        {
            error_code = Check(obj);
            if (error_code == ResultCode.Success)
            {
                src = obj.Last.Last.Last["src"].ToString();
                dst = obj.Last.Last.Last["dst"].ToString();
                return true;
            }
            else
            {
                src = null;
                dst = null;
                return false;
            }
        }

        public int GetSize()
        {
            return 0;
        }
    }

    internal class TranslationProgress : IDisposable
    {
        public string SoursePath { get; set; }
        public string TPFPath { get; set; }
        public string TempPath { get; set; }

        public FileStream Stream { get; private set; } = null;

        public TranslationProgress(string soursePath, string md5, Language language)
        {
            SoursePath = soursePath;
            string filename = Path.GetFileName(SoursePath);
            string tpffilename = $"{filename}_{md5}_{language.ToString()}.tpf";
            TPFPath = Path.Combine("Temp", tpffilename);
            TempPath = Path.Combine("Temp", filename);
        }

        public void Start()
        {
            Stream = new FileStream(TPFPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            Stream.Seek(0, SeekOrigin.Begin);
        }

        public void Dispose()
        {
            Stream?.Dispose();
        }

        public long Position { get; set; } = 0;
        private List<string> MemberList = new List<string>();
        private bool IsEnd;
        public bool CheckMember(string member)
        {
            if (IsEnd)
            {
                if (MemberList.Count == 0) return false;
                if (MemberList.Contains(member))
                {
                    MemberList.Remove(member);
                    return true;
                }
                return false;
            }
            Stream.Position = Position;
            using (BinaryReader br = new BinaryReader(Stream, Encoding.UTF8, true))
            {
                string stringValue;
                stringValue = br.ReadString();
                if (stringValue == "")
                {
                    IsEnd = true;
                    return false;
                }
                else if (stringValue == member)
                {
                    Position = br.BaseStream.Position;
                    IsEnd = false;
                    return true;
                }
                else
                {
                    MemberList.Add(stringValue);
                    while (true)
                    {
                        Position = br.BaseStream.Position;
                        stringValue = br.ReadString();
                        if (stringValue == "")
                        {
                            IsEnd = true;
                            if (MemberList.Contains(member))
                            {
                                MemberList.Remove(member);
                                return true;
                            }
                            return false;
                        }
                        if (stringValue == "-") continue;
                        MemberList.Add(stringValue);
                    }
                }
            }
        }
    }
}
