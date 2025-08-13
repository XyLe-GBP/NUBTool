using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using NUBTool.src;

namespace NUBTool
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                pictureBox_Icon.ImageLocation = "https://avatars.githubusercontent.com/u/59692068?v=4";
            }
            else
            {
                pictureBox_Icon.Image = null;
            }
        }

        private void linkLabel_github_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Common.Utils.OpenURI("https://github.com/XyLe-GBP");
        }

        private void linkLabel_web_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Common.Utils.OpenURI("https://xyle-official.com");
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox_Icon_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
                Bitmap canvas = new Bitmap(pictureBox_Icon.Width, pictureBox_Icon.Height);
                Graphics g = Graphics.FromImage(canvas);
                GraphicsPath gp = new();
                gp.AddEllipse(g.VisibleClipBounds);
                Region rgn = new(gp);
                pictureBox_Icon.Region = rgn;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error has occurred.\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }
    }
}
