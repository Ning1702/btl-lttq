using System.Windows.Forms;
using System.Drawing;

namespace btl_lttq.ChatClient
{
    partial class EmojiPickerForm
    {
        private System.ComponentModel.IContainer components = null;
        private FlowLayoutPanel flowEmojis;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.flowEmojis = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowEmojis
            // 
            this.flowEmojis.AutoScroll = true;
            this.flowEmojis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowEmojis.Location = new System.Drawing.Point(0, 0);
            this.flowEmojis.Name = "flowEmojis";
            this.flowEmojis.Padding = new System.Windows.Forms.Padding(4);
            this.flowEmojis.Size = new System.Drawing.Size(240, 210);
            this.flowEmojis.TabIndex = 0;
            // 
            // EmojiPickerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(240, 210);
            this.Controls.Add(this.flowEmojis);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EmojiPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chọn emoji";
            this.Load += new System.EventHandler(this.EmojiPickerForm_Load_1);
            this.ResumeLayout(false);

        }

    }
}
