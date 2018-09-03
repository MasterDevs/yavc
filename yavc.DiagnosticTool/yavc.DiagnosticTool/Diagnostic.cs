using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using yavc.Base;
using yavc.Base.Data;
using yavc.Base.Models;
using yavc.WPF.Imp;

namespace yavc.DiagnosticTool
{
    public class Diagnostic : DependencyObject
    {
        private static bool IsStarted;
        private static List<Fiddler.Session> oAllSessions = new List<Fiddler.Session>();

        public static void Startup()
        {
            Factory.Initialize(new WPFFactory());
            Fiddler.FiddlerApplication.BeforeRequest += sesh =>
            {
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(sesh);
                Monitor.Exit(oAllSessions);
            };

            FiddlerApplication.Startup(8080, FiddlerCoreStartupFlags.Default);
            IsStarted = true;
        }

        public static void Shutdown()
        {
            FiddlerApplication.Shutdown();
            IsStarted = false;
        }

        #region Properties
        public double PercentageComplete
        {
            get { return (double)GetValue(PercentageCompleteProperty); }
            set { Set(PercentageCompleteProperty, value); }
        }
        public static readonly DependencyProperty PercentageCompleteProperty =
            DependencyProperty.Register("PercentageComplete", typeof(double), typeof(Diagnostic), new PropertyMetadata(0D));


        public bool IsIndeterminite
        {
            get { return (bool)GetValue(IsIndeterminiteProperty); }
            set { Set(IsIndeterminiteProperty, value); }
        }
        public static readonly DependencyProperty IsIndeterminiteProperty =
            DependencyProperty.Register("IsIndeterminite", typeof(bool), typeof(Diagnostic), new PropertyMetadata(true));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { Set(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Diagnostic), new PropertyMetadata(string.Empty));



        public bool IsNotRunning
        {
            get { return (bool)GetValue(IsNotRunningProperty); }
            set { Set(IsNotRunningProperty, value); }
        }
        public static readonly DependencyProperty IsNotRunningProperty =
            DependencyProperty.Register("IsNotRunning", typeof(bool), typeof(Diagnostic), new PropertyMetadata(true));
        #endregion

        public void Run(string ipOrHostname, string deviceName, Action<bool> OnFinished)
        {
            ThreadPool.QueueUserWorkItem(state => 
                {
                    try
                    {
                        RunnerImp(ipOrHostname, deviceName, OnFinished);
                    }
                    catch (Exception exp)
                    {
                        SimpleMailer.SendMail("Joe@MasterDevs.com", "Error Running Diagnostics", GenBody(exp));
                        UI.Invoke(OnFinished, false);
                    }
                }
                );
        }

        #region Helpers

        private static string GenBody(Exception exp)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Error:");
            if (exp == null)
            {
                builder.AppendLine("NULL EXCEPTION OBJECT");
                return builder.ToString();
            }

            builder.AppendLine("Message: ");
            builder.AppendLine(exp.Message);

            builder.AppendLine("Stack Trace: ");
            builder.AppendLine(exp.StackTrace);

            builder.AppendLine("Source: ");
            builder.AppendLine(exp.Source);

            builder.AppendLine("Exception To String:");
            builder.AppendLine(exp.ToString());

            return builder.ToString();
        }

        private void RunnerImp(string ipOrHostname, string deviceName, Action<bool> OnFinished)
        {
            if (!IsStarted)
                Startup();

            IsNotRunning = false;

            var d = new Device(ipOrHostname, deviceName);
            var c = new Controller(d);


            Message = "Trying to connect to the device . . . ";
            //-- Ensure the IP is accurate
            c.TrySetDevice(d, result =>
            {

                if (!result.Success)
                {
                    Message = @"There was a problem connecting to the device. Please ensure the IP or Hostname is accurate, you're connected to the same network as the device and Network Standby on the device is set to On.";
                    UI.Invoke(OnFinished, false);
                    IsNotRunning = true;
                    return;
                }

                //-- Attempt To Load
                var start = new VMStart(new Device[] { d });
                var dvm = start.Devices[0];

                Message = "Attempting to load device . . .";

                start.Refresh();

                IsIndeterminite = false;

                Message = "Loading . . . ";
                while (dvm.IsLoading)
                {
                    PercentageComplete = dvm.PercentageLoaded;
                    Message = dvm.LoadingMessage;
                    Thread.Sleep(100);
                }

                Message = "Device loaded. Refreshing zone.";
                IsIndeterminite = true;

                //- Attempt to Refresh Selected Zone
                var mainVM = new VMMain(c);
                mainVM.SelectedZone.Refresh(() =>
                {
                    Message = "Device refresh success. Saving results . . .";

                    var data = SaveSessions(deviceName);

                    Message = "Save results success. Sending results . . .";

                    SimpleMailer.SendMail("Joe@MasterDevs.com", "Diagnostics", "Diagnostics for " + deviceName, data);

                    File.Delete(data); //-- If we successfully sent the e-mail, no reason to hold on to this file on the desktop.

                    Shutdown();

                    PercentageComplete = 1;
                    IsIndeterminite = false;

                    Message = "Finished.";
                    UI.Invoke(OnFinished, true);
                    IsNotRunning = true;
                });
            });
        }

        private string SaveSessions(string deviceName)
        {
            var thisProcId = System.Diagnostics.Process.GetCurrentProcess().Id;

            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), string.Format("Results for {0}.saz", deviceName));
            SAZFormat.WriteSessionArchive(fileName,
                oAllSessions.Where(s => s.LocalProcessID == thisProcId).ToArray(), string.Empty, false);

            return fileName;
        }

        /// <summary>
        /// Ensures property is set on the UI thread.
        /// </summary>
        private void Set(DependencyProperty prop, object val)
        {
            UI.Invoke(() => SetValue(prop, val));
        }
        #endregion
    }
}
