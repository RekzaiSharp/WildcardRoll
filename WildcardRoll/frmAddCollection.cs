using System;
using System.Windows.Forms;

namespace WildcardRoll
{
    public partial class frmAddCollection : Form
    {
        public string Result { get { return textBox1.Text; } }

        public frmAddCollection()
        {
            InitializeComponent();
        }

        private void frmAddCollection_Load(object sender, EventArgs e)
        {

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnCreate.PerformClick();
        }
    }
}
