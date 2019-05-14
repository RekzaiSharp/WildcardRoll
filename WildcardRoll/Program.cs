using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WildcardRoll
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // new Updater();
            //updateSpellicons();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void updateSpellicons()
        {
            foreach (var item in Database.Spells)
            {
                var web = new HtmlWeb();
                var doc = web.Load("https://wotlk.evowow.com/?spell=" + item.Value.ID);
                var ele = doc.DocumentNode.SelectNodes("//body//script")[6];
                var match = Regex.Match(ele.InnerText, @"Icon\.create\('([a-zA-Z0-9_]*)',").Groups[1].ToString().ToLower();
                var link = $"https://wotlk.evowow.com/static/images/wow/icons/large/{match}.jpg";

                using (var wc = new WebClient())
                using (var stream = wc.OpenRead(link))
                {
                    var bmp = new Bitmap(stream);
                    bmp.Save($"../../Resources/_{item.Value.ID}.jpg");
                }
            }
        }

        internal static string getSpellName(int id)
        {
            try
            {
                var web = new HtmlWeb();
                var doc = web.Load("https://wotlk.evowow.com/?spell=" + id);
                var ele = doc.DocumentNode.SelectNodes("//body//h1")[0];
                var str = ele.InnerText;
                return str;
            } catch
            {
                return "<none>";
            }
        }
    }
}
