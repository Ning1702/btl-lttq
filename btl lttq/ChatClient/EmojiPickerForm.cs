using System;
using System.Drawing;
using System.Windows.Forms;

namespace btl_lttq.ChatClient
{
    public partial class EmojiPickerForm : Form
    {
        public event Action<string> EmojiSelected;

        public EmojiPickerForm()
        {
            InitializeComponent();
            this.Load += EmojiPickerForm_Load;
        }

        // sự kiện chung cho tất cả emoji button
        private void EmojiButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                EmojiSelected?.Invoke(btn.Text);
                this.Close();
            }
        }
        private void EmojiPickerForm_Load(object sender, EventArgs e)
        {
            string[] emojis = new string[]
            {
        "😀","😁","😂","🤣","😃","😄","😅","😆","😉","😊",
        "😍","😘","😗","😙","😚","🙂","🤗","🤔","😐","😑",
        "😶","🙄","😏","😣","😥","😮","🤐","😯","😪","😫",
        "😴","😌","😛","😜","😝","🤤","😒","😓","😔","😕",
        "🙃","🤑","😲","☹","🙁","😖","😞","😟","😤","😢",
        "😭","😦","😧","😨","😩","😬","😰","😱","😳","🤪",
        "😡","😠","🤬","👍","👎","👌","🙏","👏","💪","❤️",
        "💙","💚","💛","💜","🧡","🤍","🤎","💔","🎉","🔥"
            };

            foreach (var em in emojis)
            {
                var btn = new Button
                {
                    Text = em,
                    Font = new Font("Segoe UI Emoji", 14f, FontStyle.Regular),
                    Width = 40,
                    Height = 40,
                    Margin = new Padding(2),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += EmojiButton_Click;
                flowEmojis.Controls.Add(btn);
            }
        }

        private void EmojiPickerForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
