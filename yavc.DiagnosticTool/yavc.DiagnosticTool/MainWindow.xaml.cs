using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace yavc.DiagnosticTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Diagnostic Diag;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Diag = new Diagnostic();

        }

        protected override void OnClosed(EventArgs e)
        {
            Diagnostic.Shutdown();
            base.OnClosed(e);
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var deviceName = tbDeviceModelNumber.Text;
            var ip = tbHostNameOrIp.Text;

            if (string.IsNullOrEmpty(deviceName))
            {
                MessageBox.Show("Please enter a device name");
                return;
            }
            else if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("Please enter an Hostname or IP");
                return;
            }

            pbRunning.Visibility = Visibility.Visible;

            Diag.Run(tbHostNameOrIp.Text, tbDeviceModelNumber.Text, success =>
            {
                pbRunning.Visibility = Visibility.Hidden;
                if (success)
                {
                    MessageBox.Show("Success. Thank you.");
                }
                else
                {
                    MessageBox.Show(@"Error. 

Please ensure you have the right IP or hostname and you're connected to the same network as the device. 
Also ensure the device's standby feature is set to on.

If there's a file on your desktop called Results.saz, please e-mail that to Joe@MasterDevs.com.

Thank you very much.

");
                }
            });
        }
    }
}
