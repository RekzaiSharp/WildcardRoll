﻿using System;
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
            if (new Updater().Run())
                return;

            // API.UpdateSpellicons();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
