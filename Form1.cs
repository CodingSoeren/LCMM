using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;
using System.Security.AccessControl;

namespace Lethal_Company_Mod_Manager
{
    public partial class Form1 : Form
    {
        string Gamedir = @"C:\Program Files (x86)\Steam\steamapps\common\Lethal Company\";
        public Form1()
        {
            CheckManifest();
            InitializeComponent();
        }

        public void DownloadMod(string address)
        {
            if (Directory.Exists(@"C:\Temp")) Directory.Delete(@"C:\Temp", true);

            if (!Directory.Exists(@"C:\Temp"))
            {
                Directory.CreateDirectory(@"C:\Temp");
            }

            WebClient webClient = new WebClient();

            webClient.DownloadFile(address, @"C:\Temp\DownloadedMod.zip");

            if (!File.Exists(@"C:\Temp\DownloadedMod.zip"))
            {
                Console.WriteLine("webClient Fehler, Internet verbindung prüfen \n State(); -> state.MenuState();");
                return;
            }

            ZipFile.ExtractToDirectory(@"C:\Temp\DownloadedMod.zip", @"C:\Temp\Mod\");
            
            if (Directory.Exists(@"C:\Temp\Mod\BepInEx\plugins"))
            {
                var files = Directory.EnumerateFiles(@"C:\Temp\Mod\BepInEx\plugins");

                foreach ( var file in files)
                {
                    string[] salat = file.Split(new[] { '\\' });

                    if (File.Exists(Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1])) return;
                    File.Move(@"C:\Temp\Mod\BepInEx\plugins\" + salat[salat.Length - 1], Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]);

                    Console.WriteLine($"Datei verschoben:\n {file} -> {Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]}");
                }

                var dirs = Directory.EnumerateDirectories(@"C:\Temp\Mod\BepInEx\plugins");

                foreach( var dir in dirs)
                {
                    string[] salat = dir.Split(new[] { '\\' });

                    Directory.Move(dir, Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]);

                    Console.WriteLine($"Verzeichnis verschoben:\n {dir} -> {Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]}");
                }

                Console.WriteLine("Import abgeschlossen");

                return;
            }

            if (Directory.Exists(@"C:\Temp\Mod\plugins"))
            {
                var files = Directory.EnumerateFiles(@"C:\Temp\Mod\plugins");

                foreach (var file in files)
                {
                    string[] salat = file.Split(new[] { '\\' });

                    if (File.Exists(Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1])) return;
                    File.Move(@"C:\Temp\Mod\plugins\" + salat[salat.Length - 1], Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]);

                    Console.WriteLine($"Datei verschoben:\n {@"C:\Temp\Mod\plugins\" + salat[salat.Length - 1]} -> {Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]}");
                }


                var dirs = Directory.EnumerateDirectories(@"C:\Temp\Mod\plugins");
                foreach (var dir in dirs)
                {
                    string[] salat = dir.Split(new[] { '\\' });

                    Directory.Move(dir, Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]);

                    Console.WriteLine($"Verzeichnis verschoben:\n {dir} -> {Gamedir + @"BepInEx\Plugins\" + salat[salat.Length - 1]}");
                }

                Console.WriteLine("Import abgeschlossen");

                return;
            }

            Console.WriteLine("Unbekannter Fehler");
        }

        public void CheckManifest()
        {
            if (!File.Exists(Gamedir + @"LCMMmanifest.json"))
            {
                using (StreamWriter sw = File.CreateText(Gamedir + @"LCMMmanifest.json"))
                {
                    sw.WriteLine("{");
                    sw.WriteLine("  " + '"' + "LCMM Default Mod" + '"' + ": {");
                    sw.WriteLine("      " + '"' + "name" + '"' + ": " + '"' + "LCMM Default Mod" + '"');
                    sw.WriteLine("  }");
                    sw.WriteLine("}");
                    sw.Close();
                }
            }
        }

        public void WriteModToManifest(Mod mod)
        {
            
            
        }

        public void Test()
        {
            /*
             * Test für convertieren zu JSON von Klasse Mod
             * 
            var mod = new Mod
            {
                name = "TestMod123",
                version_number = "1.0",
                website_url = "https://fortnite.net",
                description = "Fortnite Battle pass free",
                dependencies = new[] { "Fortnite", "Minecraft", "3 Kilo Kot"}
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(mod, options);

            Console.WriteLine(jsonString);
            */

            StreamReader sr = File.OpenText(@"C:\Program Files (x86)\Steam\steamapps\common\Lethal Company\manifest.json");
            string JSONstringChache;
            string jsonString = "";
            while ((JSONstringChache = sr.ReadLine()) != null)
            {
                jsonString += JSONstringChache;
            }
            
            var doc = JsonDocument.Parse(jsonString);
            JsonElement root = doc.RootElement;

            var mods = root.EnumerateArray();

            while (mods.MoveNext())
            {
                var mod = mods.Current;
                
                var attributes = mod.EnumerateObject();

                while (attributes.MoveNext())
                {
                    var attribute = attributes.Current;
                    Console.WriteLine($"{attribute.Name}: {attribute.Value}");
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DownloadMod(@"https://thunderstore.io/package/download/anormaltwig/LateCompany/1.0.9/");
            ///<summary>
            /// BackgroundWorker1 = Find Installed Mods worker
            /// </summary>

            // Test (Vergessen was das soll)
            string[] gamedirfiles = Directory.GetFiles(Gamedir);
            Console.WriteLine("Datein gefunden:");
            foreach (string file in gamedirfiles)
            {
                Console.WriteLine(file + "\n");
            }

            StreamReader sr = File.OpenText(@"C:\Program Files (x86)\Steam\steamapps\common\Lethal Company\manifest.json");
            string JSONstring;
            while ((JSONstring = sr.ReadLine()) != null)
            {
                Console.Write(JSONstring);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DownloadMod(richTextBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CheckManifest();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Test();
        }
    }

    public class Mod
    {

            public string name { get; set; }
            public string version_number { get; set; }
            public string website_url { get; set; }
            public string description { get; set; }
            public string[] dependencies { get; set; }
        
    }

    /*
     * LCMMmanifest.json >
     * Struktur 
     * Klasse Modinfo > name: string name (Von der Manifest der rohen Mod)
     * string name
     * string version_number
     * string website_url 
     * string description
     * string[] dependencies
     */
}
