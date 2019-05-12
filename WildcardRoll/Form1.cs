using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace WildcardRoll
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        private Process Client;
        private IntPtr Handle;
        private int rollCount = 1;
        private int keyPress;
        private string savePath = Directory.GetCurrentDirectory() + "/";

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x105;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        //Spellbook Begin: 0x00BE6D88 - 0x4

        public Database Data = new Database();
        public Memory Mem = new Memory();
        public Stopwatch watch;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           Data.Initialize();

           if(!File.Exists(savePath + "sets.json"))
               File.Create(savePath + "sets.json").Close();
           else
           {
               var text = File.ReadAllText(savePath + "sets.json");
               if (text.Length > 1)
               {

                   var _Set = JsonConvert.DeserializeObject<Set>(text);
                   checkedListBox1.Items.Add(_Set.Category);
                }
           }

           try
           {
               Process[] processes = Process.GetProcessesByName("Ascension");
               Client = processes[0];
               Handle = Client.MainWindowHandle;
               listBox1.Items.Add("Found Ascension Client: " + Client.Id + " - " + "0x" + Handle);
               Mem.Open_pHandel("Ascension");
               watch = Stopwatch.StartNew();
               timer2.Enabled = true;
               timer2.Start();

               lblName.Text = Mem.ReadString(0x00C79D18, 24);
               lblRace.Text = Mem.ReadString(0x00DCE9F2, 24);
           }
           catch (Exception exception)
           {
               Console.WriteLine(exception);
               throw;
           }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
            try
            {
                PostMessage(Handle, WM_KEYDOWN, keyPress, 0);
                Thread.Sleep(10);
                PostMessage(Handle, WM_KEYUP, keyPress, 0);
                listBox1.Items.Add("Roll Count: " + rollCount + ", Time: " + DateTime.Now.ToString().Split(' ')[1]);
                rollCount += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                Thread.Sleep(300);
                CheckSpells();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Could not reroll: " + exception);
                throw;
            }
            
        }

        private void CheckSpells()
        {
            listBox3.Items.Clear();
            for (var i = 0; i <= 100; ++i)
            {
                var spellID = Mem.ReadInt(0x00BE6D88 + (i * 0x4));
                if (Data.SpellList.ContainsKey(spellID))
                    listBox3.Items.Add(Data.SpellList[spellID].ToString());
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox2.Text);
            timer1.Enabled = true;
            timer1.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Press Key";
        }

        private void Button1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.Text = e.KeyCode.ToString();
            keyPress = (int)e.KeyCode;
            button1.Text = "Select Key";
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add(textBox3.Text);
        }

        private void AddSet()
        {
            foreach (var item in checkedListBox1.Items)
            {
                Set s = new Set();
                s.Category = textBox3.Text;

                string json = JsonConvert.SerializeObject(s);
                System.IO.File.WriteAllText(savePath + "sets.json", json);
            }
        }

        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Enabled = true;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length > 1)
                listBox2.Items.Add(comboBox1.Text);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            lblRuntime.Text = watch.Elapsed.Seconds.ToString() + " seconds";
        }
    }
}
