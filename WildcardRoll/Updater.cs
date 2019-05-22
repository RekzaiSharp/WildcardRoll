using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NGit;
using NGit.Api;
using NGit.Transport;

namespace WildcardRoll
{
    class Updater
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        static string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        static string repo_dir = Environment.CurrentDirectory + "\\repo";
        static string remote = "https://github.com/RekzaiSharp/WildcardRoll.git";
        static string executable = AppDomain.CurrentDomain.FriendlyName;

        public Updater()
        {

        }

        public bool Run()
        {
            Clone();
            if (!Update())
                return false;

            AllocConsole();
            Build();
            Console.ReadKey(true);
            FreeConsole();

            return true;
        }

        void Clone()
        {
            Directory.CreateDirectory(repo_dir);
            Git.CloneRepository().SetDirectory(repo_dir).SetURI(remote).Call();
        }

        bool Update()
        {
            var repo = Git.Open(repo_dir);
            var refUpdate = repo.Fetch().Call().GetTrackingRefUpdates();
            if (refUpdate.Count > 0)
            {
                repo.BranchCreate().SetForce(true).SetName("master").SetStartPoint("origin/master").Call();
                repo.Checkout().SetName("master").Call();
            } else
            {
                // already up2date
                return false;
            }
            repo.GetRepository().Close();
            repo.GetRepository().ObjectDatabase.Close();
            return true;
        }

        void Build()
        {
            var dir = Environment.CurrentDirectory + "\\bin";
            var net_dir = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Microsoft.NET\Framework";
            if (Directory.Exists(net_dir + "64"))
                net_dir += "64";

            string[] subdir = Directory.GetDirectories(net_dir, "v4.*");
            string msbuild = subdir.Length != 0 ? subdir[subdir.Length - 1] : null;
            if (msbuild == null)
            {
                // net framework 4.0 not installed
                return;
            }

            msbuild += @"\msbuild.exe";
            var buildProc = new Process();
            buildProc.StartInfo.FileName = msbuild;
            buildProc.StartInfo.CreateNoWindow = true;
            buildProc.StartInfo.UseShellExecute = false;
            buildProc.StartInfo.RedirectStandardOutput = true;
            buildProc.StartInfo.RedirectStandardError = true;
            buildProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            buildProc.StartInfo.Arguments = "WildcardRoll.csproj /t:Build";
            buildProc.StartInfo.WorkingDirectory = repo_dir + @"\WildcardRoll";
            buildProc.Start();

            while (!buildProc.StandardOutput.EndOfStream)
                Console.WriteLine(buildProc.StandardOutput.ReadLine());

            buildProc.WaitForExit();
        }

        static void Copy()
        {

        }
    }
}
