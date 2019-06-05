using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            const int DisplayTime = 3000;

            int start = Environment.TickCount;

            await Task.Factory.StartNew(Work);

            int end = Environment.TickCount;

            int interval = end - start;

            if(interval < DisplayTime)
            {
                int sleep = DisplayTime - interval;
                await Task.Delay(sleep);
            }

            Close();
        }

        private void Work()
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Proxy = null;

                string[] lines = wc.DownloadString(Program.UpdateLink).Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string ServerVersion = lines[0];
                string DownloadLink = lines[1];

                if (File.Exists(Program.Executable))
                {
                    string ApplicationVersion = FileVersionInfo.GetVersionInfo(Program.Executable).ProductVersion;

                    if (ApplicationVersion != ServerVersion)
                    {
                        File.Delete(Program.Executable);
                        wc.DownloadFile(DownloadLink, Program.Executable);
                    }
                }
                else
                {
                    wc.DownloadFile(DownloadLink, Program.Executable);
                    MessageBox.Show("Successfully downloaded new version!");
                }
            }
            catch (Exception ex)
            {
                string message = string.Concat("Updating error:", Environment.NewLine, ex);

                Invoke(new Action(() =>
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
