using DemoRESTService.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebLib;

namespace BitcoinService
{
    public partial class Service1 : ServiceBase
    {
        #region service vars
        private Timer timer;
        string connectionStringName = "Development";
        DAC_SYSSETTING sysSetting = new DAC_SYSSETTING();
        public bool BTCEnable { get; private set; } = DAC.GetBoolean(ConfigurationManager.AppSettings["BTCEnable"]);
        public string BTCServiceDates = "";
        public string BTCServiceTime = "";
        private string Logtitle = "";
        public string BaseLogPath = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\BTC Service\\";
        public string RESTLogPath = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\BTC Service\\REST_TEST\\";
        #endregion


        public Service1()
        {
            if (!Directory.Exists(BaseLogPath))
                Directory.CreateDirectory(BaseLogPath);

            CheckLogFolder(RESTLogPath);
            if (ConfigurationManager.AppSettings["ConnectionString"] != null)
                connectionStringName = ConfigurationManager.AppSettings["ConnectionString"];
            PublicVariable.ConnectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            Logger.Log(RESTLogPath, LogType.Day, "connection String Name = (" + connectionStringName + ")");
            InitializeComponent();

            timer = new Timer(1000);
            timer.Enabled = false;
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Second != 0)
            {
                return;
            }
            //else
            //    Logger.Log(BaseLogPath, LogType.Day, "[BTC Service] Timmer Trigged");

            if (BTCEnable)
            {
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                string nowTime = DateTime.Now.ToString("HH:mm");
                string dayOfWeek = ((int)DateTime.Now.DayOfWeek).ToString();

                Logtitle = "BTC Service";
                var SysItem = sysSetting.SelectByInitial("RESTT").FirstOrDefault();

                if (SysItem.FEQUENCY == "7")
                    BTCServiceDates = "0,1,2,3,4,5,6";
                else
                    BTCServiceDates = SysItem.FEQUENCY;

                if (SysItem.STARTTIME != "")
                    BTCServiceTime = SysItem.STARTTIME;
                else
                    BTCServiceTime = DAC.GetString(ConfigurationManager.AppSettings["BTCServiceTime"]);

                Logger.Log(BaseLogPath, LogType.Day, Logtitle + "(enabled)");
                if (BTCServiceDates.Contains(dayOfWeek) && string.Compare(BTCServiceTime, nowTime, StringComparison.Ordinal) == 0)
                {
                    Logger.Log(BaseLogPath, LogType.Day, "[ " + Logtitle + " ] : Started");

                    BTCBGroundWorker.RunWorkerAsync();
                }
                else
                    Logger.Log(BaseLogPath, LogType.Day, Logtitle + Logtitle + " ByPassed");
            }
        }

        public void CheckLogFolder(string LogPath)
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
                Logger.Log(BaseLogPath, LogType.Day, "Log Folder created (" + LogPath + ")");
            }
            else
                Logger.Log(BaseLogPath, LogType.Day, "Log Folder exists (" + LogPath + ")");
        }

        protected override void OnStart(string[] args)
        {
            MailService.Enabled = true;
            // 其他設定值請在這裡 log 下來
            DAC.ConnectionType = DAC.connTypeMSSQL;
            DAC.ConnectionString = PublicVariable.ConnectionString;
            Logger.Log(BaseLogPath, LogType.Day, "[ Bitcoin Service ] Started by Admin Manually");
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            Logger.Log(BaseLogPath, LogType.Day, "[ Bitcoin Service ] Stopped by Admin Manually");
        }

        private void BTCBGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //RestAddrURL
            var url = DAC.GetString(ConfigurationManager.AppSettings["RestAddrURL"]);
            var DATA = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/text";
            request.ContentLength = DATA.Length;
            using (Stream webStream = request.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, Encoding.UTF8))
            {
                requestWriter.Write(DATA);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            Console.Out.WriteLine(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(ex.Message);
            }
        }
    }
}
