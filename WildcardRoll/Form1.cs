using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WildcardRoll
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        private string savePath = Directory.GetCurrentDirectory() + "/";

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x105;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        Memory Mem = new Memory();
        List<Set> Sets = new List<Set>();
        PictureBox[] pbs;
        ToolTip[] tts;
        Set selectedSet;
        List<ToolTip> renderTTs = new List<ToolTip>();

        static string settings_file = "settings.json";

        public Form1()
        {
            InitializeComponent();
        }

        void LoadSettings()
        {
            if (File.Exists(settings_file))
                Sets = JsonConvert.DeserializeObject<List<Set>>(File.ReadAllText(settings_file));
        }

        void SaveSettings()
        {
            File.WriteAllText(settings_file, JsonConvert.SerializeObject(Sets));
        }

        void Render(string search = null)
        {
            panel1.SuspendLayout();

            foreach (var tt in renderTTs)
                tt.Dispose();
            renderTTs.Clear();

            foreach (var ctrl in panel1.Controls)
                ((PictureBox)ctrl).Dispose();
            panel1.Controls.Clear();

            if (search != null)
                search = search.ToLower();

            int i = 0;
            foreach (var item in Database.Spells)
            {
                var id = item.Key;
                var spell = item.Value;

                if (search != null && !spell.Name.ToLower().Contains(search))
                    continue;

                var row = 7;

                var pb = new PictureBox();
                pb.Image = spell.Icon;
                pb.Width = 50;
                pb.Height = 50;
                pb.Location = new Point(pb.Width * (i % row), pb.Height * (int)Math.Floor(0.0 + i / row));
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Click += (sender, e) =>
                {
                    selectedSet.AddSpell(spell);
                    UpdateSet();
                };

                var toolTip = new ToolTip();
                toolTip.SetToolTip(pb, spell.Name);
                renderTTs.Add(toolTip);

                panel1.Controls.Add(pb);

                ++i;
            }

            panel1.ResumeLayout();
        }

        void UpdateSet()
        {
            for (int i = 0; i < 5; i++)
            {
                var spell = i < selectedSet.Spells.Count ? selectedSet.Spells[i] : null;
                pbs[i].Image = spell != null ? spell.Icon : Properties.Resources.inv_misc_questionmark;
                tts[i].SetToolTip(pbs[i], spell != null ? spell.Name : "Anything");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();
            setBindingSource.DataSource = Sets;

            pbs = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            tts = new ToolTip[] { new ToolTip(), new ToolTip(), new ToolTip(), new ToolTip(), new ToolTip() };

            int i = 0;
            foreach (var pb in pbs)
            {
                int index = i;
                pb.Click += (a, b) =>
                {
                    selectedSet.RemoveSpellByIndex(index);
                    UpdateSet();
                };
                i++;
            }

            Render();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.Rows.Count - 1)
                return;

            textBox1.Text = "";
            selectedSet = (Set)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            UpdateSet();
            tabControl1.SelectedIndex = 1;
            textBox1.Select();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            selectedSet = null;
            tabControl1.SelectedIndex = 0;
            SaveSettings();
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                Stop();
            else
                Run();
        }

        void Stop()
        {
            timer1.Stop();
            btnRoll.Text = "Run";
        }

        void Run()
        {
            timer1.Start();
            btnRoll.Text = "Stop";
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                var elem = dataGridView1[e.ColumnIndex, e.RowIndex];
                var set = (Set)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                if (set == null)
                {
                    e.Value = Properties.Resources.inv_misc_questionmark;
                    elem.ToolTipText = "Anything";
                    return;
                }

                var spell = e.ColumnIndex <= set.Spells.Count ? set.Spells[e.ColumnIndex - 1] : null;
                if (spell == null)
                {
                    e.Value = Properties.Resources.inv_misc_questionmark;
                    elem.ToolTipText = "Anything";
                    return;
                }

                e.Value = spell.Icon;
                dataGridView1[e.ColumnIndex, e.RowIndex].ToolTipText = spell.Name;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Render(textBox1.Text);
        }

        bool CheckForSetHit(List<int> ids, out Set result)
        {
            foreach (var set in Sets)
            {
                if (!set.Enabled || set.Spells.Count == 0)
                    continue;

                var search = (from s in set.Spells select s.ID).ToArray();
                var inter = search.Intersect(ids);
                if (inter.Count() == search.Count())
                {
                    result = set;
                    return true;
                }
            }

            result = null;
            return false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var processes = Process.GetProcessesByName("Ascension");
                foreach (var client in processes)
                {
                    var handle = client.Handle;
                    Mem.Open_pHandel(handle);

                    var ids = new List<int>();
                    for (var i = 0; i <= 100; ++i)
                    {
                        var id = Mem.ReadInt(0x00BE6D88 + (i * 0x4));
                        if (id != 0 && id <= 200000)
                            ids.Add(id);
                    }

                    ids = ids.Except(Database.Ignore).ToList();

                    var spells = from s in Database.Spells select s.Value.ID;
                    var diff = ids.Except(spells);
                    var diffWithNames = from id in diff select $"{id} (guess: {Program.getSpellName(id)})";
                    if (diff.Count() != 0)
                    {
                        Stop();
                        MessageBox.Show("Unknwon Spell ID(s): " + string.Join(", ", diffWithNames) + "\n\nPlease contact a developer with this message.");
                        BringToFront();
                        return;
                    }

                    if (CheckForSetHit(ids, out Set result))
                    {
                        Stop();
                        MessageBox.Show("HIT");
                        BringToFront();
                        return;
                    }

                    SendMessage(handle, WM_KEYDOWN, (int)Keys.D0, IntPtr.Zero);
                    SendMessage(handle, WM_KEYUP, (int)Keys.D0, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                Stop();
                MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
