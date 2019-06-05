using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace Updater
{
    static class Program
    {
        public const string Executable = "FireBot.zip";
        public const string UpdateLink = "http://23.254.132.151/Fv2/update.php"; //UHHHH. Replace this with your own php file

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if !DEBUG
            if (IsAdministrator() == false)
                MessageBox.Show("Please run as administrator");
            else
#endif
                Application.Run(new MainForm());
        }

        static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}