using System;
using System.Windows.Forms;

namespace btl_lttq.FacebookLite
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenSettings_Click(object sender, EventArgs e)
        {
            using (var f = new SettingForm())
            {
                f.ShowDialog(this);
            }
        }

    }
}
