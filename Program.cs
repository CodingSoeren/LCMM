using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lethal_Company_Mod_Manager
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string execPath = @"C:\Program Files (x86)\Steam\steamapps\common\Lethal Company\Lethal Company.exe";
            if (File.Exists(execPath))
            {
                MessageBox.Show("Lethal Company Executable gefunden");
            }
            else
            {
                MessageBox.Show("Lethal Company nicht gefunden", "Fehler 0x000001A1");
                Application.Exit();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
