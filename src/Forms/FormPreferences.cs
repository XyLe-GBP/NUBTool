using NUBTool.Localizable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NUBTool.Common;

namespace NUBTool
{
    public partial class FormPreferences : Form
    {
        public FormPreferences()
        {
            InitializeComponent();
        }

        private void FormPreferences_Load(object sender, EventArgs e)
        {
            Config.Load(Common.xmlpath);

            if (bool.Parse(Config.Entry["Check_Update"].Value))
            {
                checkBox_Updates.Checked = true;
            }
            else
            {
                checkBox_Updates.Checked = false;
            }
            if (bool.Parse(Config.Entry["SmoothSamples"].Value))
            {
                checkBox_SmoothSamples.Checked = true;
            }
            else
            {
                checkBox_SmoothSamples.Checked = false;
            }
            if (bool.Parse(Config.Entry["HideSplash"].Value))
            {
                checkBox_HideSplash.Checked = true;
            }
            else
            {
                checkBox_HideSplash.Checked = false;
            }
            if (bool.Parse(Config.Entry["SplashImage"].Value))
            {
                checkBox_SplashImage.Checked = true;
                button_SplashImage.Enabled = true;
                if (!string.IsNullOrWhiteSpace(Config.Entry["SplashImage_Path"].Value))
                {
                    textBox_SplashImage.Text = Config.Entry["SplashImage_Path"].Value;
                }
                else
                {
                    textBox_SplashImage.Text = string.Empty;
                }
            }
            else
            {
                checkBox_SplashImage.Checked = false;
                button_SplashImage.Enabled = false;
            }
            if (bool.Parse(Config.Entry["ShowFolder"].Value))
            {
                checkBox_showdirectory.Checked = true;
            }
            else
            {
                checkBox_showdirectory.Checked = false;
            }

            if (bool.Parse(Config.Entry["FixedConvert"].Value))
            {
                checkBox_ATWFix.Checked = true;
            }
            else
            {
                checkBox_ATWFix.Checked = false;
            }
            if (!string.IsNullOrWhiteSpace(Config.Entry["ConvertType"].Value))
            {
                switch (int.Parse(Config.Entry["ConvertType"].Value))
                {
                    case 0:
                        comboBox_ATWFix.Enabled = true;
                        comboBox_ATWFix.SelectedIndex = 0;
                        break;
                    case 1:
                        comboBox_ATWFix.Enabled = true;
                        comboBox_ATWFix.SelectedIndex = 1;
                        break;
                    default:
                        comboBox_ATWFix.Enabled = false;
                        comboBox_ATWFix.SelectedIndex = 0;
                        break;
                }
            }
            else
            {
                comboBox_ATWFix.Enabled = false;
                comboBox_ATWFix.SelectedIndex = 0;
            }
            if (bool.Parse(Config.Entry["ForceConvertWaveOnly"].Value))
            {
                checkBox_ForceWaveConvertion.Checked = true;
            }
            else
            {
                checkBox_ForceWaveConvertion.Checked = false;
            }
            if (bool.Parse(Config.Entry["LoopWarning"].Value))
            {
                checkBox_LoopWarning.Checked = true;
            }
            else
            {
                checkBox_LoopWarning.Checked = false;
            }
        }

        private void Button_SplashImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                FilterIndex = 3,
                Filter = "JPEG Image (*.jpg,*.jpeg)|*.jpg;*.jpeg|Bitmap Image (*.bmp)|*.bmp|PNG Image (*.png)|*.png|All Supported Files|*.jpg;*.jpeg;*.bmp;*.png",
                Multiselect = false,
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using var img = Image.FromFile(ofd.FileName);
                if (img.Width == 800 && img.Height == 400)
                {
                    textBox_SplashImage.Text = ofd.FileName;
                }
                else if (img.Width == 400 && img.Height == 200)
                {
                    textBox_SplashImage.Text = ofd.FileName;
                }
                else
                {
                    MessageBox.Show(Localization.CustomSplashSizeErrorCaption, Localization.MSGBoxErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox_SplashImage.Text = string.Empty;
                }
                return;
            }
            else
            {
                return;
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (checkBox_SplashImage.Checked && string.IsNullOrWhiteSpace(textBox_SplashImage.Text))
            {
                MessageBox.Show("The path to the splash screen image is not specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (checkBox_Updates.Checked)
            {
                Config.Entry["Check_Update"].Value = "true";
            }
            else
            {
                Config.Entry["Check_Update"].Value = "false";
            }
            if (checkBox_SmoothSamples.Checked)
            {
                Config.Entry["SmoothSamples"].Value = "true";
            }
            else
            {
                Config.Entry["SmoothSamples"].Value = "false";
            }
            if (checkBox_HideSplash.Checked)
            {
                Config.Entry["HideSplash"].Value = "true";
            }
            else
            {
                Config.Entry["HideSplash"].Value = "false";
            }
            if (checkBox_SplashImage.Checked)
            {
                Config.Entry["SplashImage"].Value = "true";
                if (!string.IsNullOrWhiteSpace(textBox_SplashImage.Text))
                {
                    Config.Entry["SplashImage_Path"].Value = textBox_SplashImage.Text;
                }
                else
                {
                    Config.Entry["SplashImage_Path"].Value = "";
                }
            }
            else
            {
                Config.Entry["SplashImage"].Value = "false";
                Config.Entry["SplashImage_Path"].Value = "";
            }
            if (checkBox_showdirectory.Checked)
            {
                Config.Entry["ShowFolder"].Value = "true";
            }
            else
            {
                Config.Entry["ShowFolder"].Value = "false";
            }

            if (checkBox_ATWFix.Checked)
            {
                Config.Entry["FixedConvert"].Value = "true";
                Config.Entry["ConvertType"].Value = comboBox_ATWFix.SelectedIndex.ToString();
            }
            else
            {
                Config.Entry["FixedConvert"].Value = "false";
                Config.Entry["ConvertType"].Value = "";
            }
            if (checkBox_ForceWaveConvertion.Checked)
            {
                Config.Entry["ForceConvertWaveOnly"].Value = "true";
            }
            else
            {
                Config.Entry["ForceConvertWaveOnly"].Value = "false";
            }
            if (checkBox_LoopWarning.Checked)
            {
                Config.Entry["LoopWarning"].Value = "true";
            }
            else
            {
                Config.Entry["LoopWarning"].Value = "false";
            }

            Config.Save(xmlpath);

            Close();
            return;
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
            return;
        }

        private void CheckBox_ATWFix_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ATWFix.Checked)
            {
                comboBox_ATWFix.Enabled = true;
            }
            else
            {
                comboBox_ATWFix.Enabled = false;
            }
        }

        private void CheckBox_LoopWarning_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_LoopWarning.Checked)
            {
                Generic.IsLoopWarning = true;
            }
            else
            {
                Generic.IsLoopWarning = false;
            }
        }

        private void CheckBox_SplashImage_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_SplashImage.Checked)
            {
                checkBox_SplashImage.Checked = false;
                textBox_SplashImage.Text = string.Empty;
                textBox_SplashImage.Enabled = false;
                button_SplashImage.Enabled = false;
            }
            else
            {
                checkBox_SplashImage.Checked = true;
                textBox_SplashImage.Text = string.Empty;
                textBox_SplashImage.Enabled = true;
                button_SplashImage.Enabled = true;
            }
        }
    }
}
