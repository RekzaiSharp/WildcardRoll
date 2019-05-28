using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace WildcardRoll
{
    public partial class frmMain : Form
    {
        static string VERSION = "v1.1.0a";

        Memory Mem = new Memory();
        PictureBox[] pbs;
        ToolTip[] tts;
        Set selectedSet;
        List<ToolTip> renderTTs = new List<ToolTip>();

        BindingList<Collection> Collections = new BindingList<Collection>();
        Collection CurrentCollection { get { return (Collection)collectionComboBox.SelectedItem; } }

        public frmMain()
        {
            InitializeComponent();
        }

        void LoadCollections()
        {
            if (File.Exists("collections.json"))
                Collections = JsonConvert.DeserializeObject<BindingList<Collection>>(File.ReadAllText("collections.json"));
        }

        void SaveCollections()
        {
            File.WriteAllText("collections.json", JsonConvert.SerializeObject(Collections, Formatting.Indented));
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
            Text += " " + VERSION;

            LoadCollections();
            collectionBindingSource.DataSource = Collections;
            collectionComboBox.DisplayMember = "ShowAs";
            collectionComboBox.ValueMember = "ShowAs";

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
            if (e.RowIndex == -1 || e.RowIndex == dataGridView1.Rows.Count - 1)
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
            SaveCollections();
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
            btnRoll.Text = "Roll!";
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
                var row = dataGridView1.Rows[e.RowIndex];
                var set = (Set)row.DataBoundItem;
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

        bool CheckForSetHit(List<int> ids, out Collection collectionRet, out Set setRet)
        {
            foreach (var collection in Collections)
                foreach (var set in collection.Sets)
                {
                    if (!set.Enabled || set.Spells.Count == 0)
                        continue;

                    var search = (from s in set.Spells select s.ID).ToArray();
                    var inter = search.Intersect(ids);
                    if (inter.Count() == search.Count())
                    {
                        collectionRet = collection;
                        setRet = set;
                        return true;
                    }
                }

            collectionRet = null;
            setRet = null;
            return false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var processes = Process.GetProcessesByName("Ascension");
                foreach (var client in processes)
                {
                    Mem.Open_pHandel(client.Handle);

                    // check if client is logged in
                    var loggedIn = Mem.ReadByte(0x00ACDBE8) == 1;
                    if (!loggedIn)
                        continue;

                    // only roll on level one characters
                    var level = Mem.ReadByte(0x00C79E90);
                    if (level != 1)
                        continue;

                    var ids = new List<int>();
                    for (var i = 0; i <= 100; ++i)
                    {
                        var address = 0x00BE6D88 + (i * 0x4);
                        var id = Mem.ReadUShort(address);
                        var flag = Mem.ReadUShort(address + 0x2);
                        if (id != 0 && flag == 0)
                            ids.Add(id);
                    }

                    ids = ids.Except(Database.Ignore).ToList();

                    var spells = from s in Database.Spells select s.Value.ID;
                    var diff = ids.Except(spells);
                    var diffWithNames = from id in diff select $"{id} (guess: {API.GetSpellName(id)})";
                    if (diff.Count() != 0)
                    {
                        Stop();
                        MessageBox.Show("Unknwon Spell ID(s): " + string.Join(", ", diffWithNames) + "\n\nPlease contact a developer with this message.");
                        return;
                    }

                    if (CheckForSetHit(ids, out Collection collection, out Set result))
                    {
                        Stop();
                        Native.FlashWindow(Handle, true);
                        MessageBox.Show($"HIT {collection.Name}: {string.Join(", ", from s in result.Spells select $"{s.Name} [{s.ID}]")}");
                        return;
                    }

                    Native.SendMessage(client.MainWindowHandle, Native.WM_KEYDOWN, (int)Keys.D0, IntPtr.Zero);
                    Native.SendMessage(client.MainWindowHandle, Native.WM_KEYUP, (int)Keys.D0, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                Stop();
                MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            SaveCollections();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveCollections();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/RekzaiSharp/WildcardRoll");
        }

        private void btnAddCollection_Click(object sender, EventArgs e)
        {
            var form = new frmAddCollection();
            var result = form.ShowDialog();
            if (result != DialogResult.OK)
                return;

            var collection = new Collection() { Name = form.Result };
            Collections.Add(collection);
            collectionComboBox.SelectedItem = collection;
            SaveCollections();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentCollection == null)
                return;

            if (MessageBox.Show($"Are you sure you want to delete the collection \"{CurrentCollection.Name}\"?",
                "Delete Collection",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            Collections.Remove(CurrentCollection);
            SaveCollections();
        }

        private void collectionComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentCollection == null)
            {
                dataGridView1.Enabled = false;
                setBindingSource.DataSource = null;
                return;
            }

            dataGridView1.Enabled = true;
            setBindingSource.DataSource = CurrentCollection.Sets;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Update data changes instantly to update the numbers in the collectionComboBox
            dataGridView1.EndEdit();
        }

        private void dataGridView1_EnabledChanged(object sender, EventArgs e)
        {
            var dgv = dataGridView1;
            dgv.DefaultCellStyle.BackColor = dgv.Enabled ? SystemColors.Window : SystemColors.Control;
            dgv.DefaultCellStyle.ForeColor = dgv.Enabled ? SystemColors.ControlText : SystemColors.GrayText;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = dgv.Enabled ? SystemColors.Window : SystemColors.Control;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = dgv.Enabled ? SystemColors.ControlText : SystemColors.GrayText;
            dgv.ReadOnly = !dgv.Enabled;
            dgv.EnableHeadersVisualStyles = dgv.Enabled;
            if (!dgv.Enabled)
                dgv.CurrentCell = null;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.D)
                return;

            if (dataGridView1.SelectedRows.Count != 1)
                return;

            var row = dataGridView1.SelectedRows[0];
            if (row.Index >= dataGridView1.RowCount - 1)
                return;

            var set = (Set)row.DataBoundItem;
            CurrentCollection.Sets.Insert(row.Index, set.Clone());
            SaveCollections();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // fix errors while disposing dataGridView1 and collectionComboBox, see:
            // https://social.msdn.microsoft.com/Forums/windows/en-US/d2e9cc04-4104-4154-b82f-74613a75f44a/problem-when-closing-form-containing-datagridview-index-0-does-not-have-a-value?forum=winforms
            dataGridView1.DataSource = null;
        }
    }
}