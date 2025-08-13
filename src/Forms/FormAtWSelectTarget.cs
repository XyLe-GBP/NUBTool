using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NUBTool.src.Forms
{
    public partial class FormAtWSelectTarget : Form
    {
        public FormAtWSelectTarget()
        {
            InitializeComponent();
        }

        private void FormAtWSelectTarget_Load(object sender, EventArgs e)
        {
            Text = "Select convert target";
            Common.Generic.WTAmethod = 0;
            comboBox_method.SelectedIndex = 0;
        }

        private void comboBox_method_SelectedIndexChanged(object sender, EventArgs e)
        {
            Common.Generic.WTAmethod = comboBox_method.SelectedIndex switch
            {
                0 => 0,
                1 => 1,
                _ => 0,
            };
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
