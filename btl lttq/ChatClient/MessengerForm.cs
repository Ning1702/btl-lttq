﻿using btl_lttq.ChatClient;
using btl_lttq.Data;
using btl_lttq.Data;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace btl_lttq.ChatClient
{
    public partial class MessengerForm : Form
    {
        // ─── state ───────────────────────────────────────────
        private AppTheme _theme = AppTheme.Light;
        private Palette _palette = Palette.For(AppTheme.Light);

        private Guid _currentUserId = Guid.Empty;         // user đang đăng nhập
        private Guid _currentConversationId = Guid.Empty; // hội thoại đang mở

        private bool _infoVisible = true;
        private int _rightPanelWidth = 260;

        // nhớ background theo từng hội thoại
        private readonly System.Collections.Generic.Dictionary<Guid, string> _convBack =
            new System.Collections.Generic.Dictionary<Guid, string>();

        public enum AppTheme { Light = 0, Dark = 1 }

        private class ConvItem
        {
            public Guid Id;
            public string Title;
            public override string ToString() => Title;
        }
        private readonly System.Collections.Generic.List<ConvItem> _allConversations
            = new System.Collections.Generic.List<ConvItem>();

        // danh sách ảnh nền có sẵn
        private readonly string[] _chatBackgrounds =
        {
            @"Background\Backgroundbear.jpg",
            @"Background\Backgrounddoodle.jpg",
            @"Background\BackgroundDoodle2.jpg",
            @"Background\Backgroundlove.jpg",
            @"Background\BackgroundSnoopy.jpg",
            @"Background\Backgroundtrochuyen.jpg"
        };

        // ─── ctor ────────────────────────────────────────────
        // ctor nhận sẵn userId từ form login
        public MessengerForm(Guid userId) : this()
        {
            _currentUserId = userId;
        }

        // ctor mặc định – dùng user "anninh" trong DB mẫu
        public MessengerForm()
        {
            InitializeComponent();

            _rightPanelWidth = panelRight.Width > 0 ? panelRight.Width : 260;

            panelRight.Visible = false;
            splitterRight.Visible = false;
            _infoVisible = false;
            btnInfo.Text = "<";


            // combobox nền
            if (cboBackground != null)
            {
                cboBackground.DropDownStyle = ComboBoxStyle.DropDownList;
                cboBackground.Items.Add("Mặc định (trắng)");
                foreach (var bg in _chatBackgrounds)
                    cboBackground.Items.Add(Path.GetFileNameWithoutExtension(bg));
                cboBackground.SelectedIndex = 0;
                cboBackground.SelectedIndexChanged += cboBackground_SelectedIndexChanged;
            }

            // load dữ liệu
            this.Load += async (s, e) => await InitDataAsync();

            txtGlobalSearch.TextChanged += (s, e) => ApplyConversationFilter();
            txtSearchInChat.KeyDown += txtSearchInChat_KeyDown;

            // mở hội thoại
            lvConversations.ItemActivate += async (s, e) =>
            {
                if (lvConversations.SelectedItems.Count == 0) return;

                _currentConversationId = (Guid)lvConversations.SelectedItems[0].Tag;

                // lấy nền đã lưu trong DB
                var savedBg = await GetConversationBackgroundAsync(_currentConversationId);
                _convBack[_currentConversationId] = savedBg;
                ApplyBackgroundToMessagesPanel(savedBg);

                // sync combobox
                if (cboBackground != null)
                {
                    if (string.IsNullOrEmpty(savedBg))
                        cboBackground.SelectedIndex = 0;
                    else
                    {
                        int idx = Array.FindIndex(_chatBackgrounds,
                            p => string.Equals(p, savedBg, StringComparison.OrdinalIgnoreCase));
                        cboBackground.SelectedIndex = idx >= 0 ? idx + 1 : 0;
                    }
                }

                await Task.WhenAll(
                    LoadConversationHeaderAsync(_currentConversationId),
                    LoadMembersAsync(_currentConversationId),
                    LoadMessagesAsync(_currentConversationId),
                    LoadSharedFilesAsync(_currentConversationId)
                );
            };

            // gửi tin
            btnSend.Click += async (s, e) => await SendCurrentMessageAsync();
            txtMessage.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && !e.Shift)
                {
                    e.SuppressKeyPress = true;
                    await SendCurrentMessageAsync();
                }
            };

            // gửi file
            btnFile.Click += async (s, e) => await SendFileAsync();

            // nút info
            btnInfo.Click += (s, e) => ToggleInfoPanel();

            // emoji
            btnEmoji.Click += btnEmoji_Click;

            // placeholder
            SetupPlaceholder(txtGlobalSearch, "Tìm kiếm (bạn bè, nhóm, tin nhắn)");
            SetupPlaceholder(txtSearchInChat, "Tìm trong hội thoại");

            // menu chuột phải cho list thành viên
            var cm = new ContextMenuStrip();
            var miAdd = new ToolStripMenuItem("Thêm thành viên...");
            var miRemove = new ToolStripMenuItem("Xóa thành viên");
            cm.Items.Add(miAdd);
            cm.Items.Add(miRemove);
            lstMembers.ContextMenuStrip = cm;
            miAdd.Click += async (s, e) => await PromptAddMemberAsync();
            miRemove.Click += async (s, e) => await RemoveSelectedMemberAsync();

            // test kết nối
            try { using (var c = Db.OpenConn()) { } }
            catch (Exception ex) { MessageBox.Show("Không thể kết nối SQL!\n" + ex.Message); }

            ToggleTheme(_theme);

            // ───────────────────────────────────────────
            // Nút "Bạn bè" → mở danh sách bạn bè
            btnFriends.Click += (s, e) =>
            {
                var f = new Friendprofile.FriendListForm(); // namespace chính xác của bạn
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            };

            // Nút "Hồ sơ" → mở thông tin cá nhân
            btnProfile.Click += (s, e) =>
            {
                var f = new Friendprofile.ProfileForm();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            };

            // Nút "Cài đặt" → mở form cài đặt
            btnSettings.Click += (s, e) =>
            {
                var f = new FacebookLite.SettingForm(); // form nằm trong namespace FacebookLite
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            };

        }

        // ───────────────────────────────────────────
        // 1. load user / hội thoại ban đầu
        private async Task InitDataAsync()
        {
            try
            {
                // nếu form gọi không truyền user → dùng 'anninh'
                if (_currentUserId == Guid.Empty)
                {
                    using (var conn = await Task.Run(() => Db.OpenConn()))
                    using (var cmd = new SqlCommand("SELECT TOP 1 Id FROM Users WHERE UserName=@u", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", "anninh");
                        var obj = await cmd.ExecuteScalarAsync();
                        if (obj == null)
                        {
                            MessageBox.Show("Không tìm thấy user 'anninh'. Hãy đổi lại tên user trong code.");
                            return;
                        }
                        _currentUserId = (Guid)obj;
                    }
                }

                await LoadConversationsAsync();

                if (lvConversations.Items.Count > 0)
                {
                    lvConversations.Items[0].Selected = true;
                    _currentConversationId = (Guid)lvConversations.Items[0].Tag;

                    await Task.WhenAll(
                        LoadConversationHeaderAsync(_currentConversationId),
                        LoadMembersAsync(_currentConversationId),
                        LoadMessagesAsync(_currentConversationId),
                        LoadSharedFilesAsync(_currentConversationId)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo: " + ex.Message);
            }
        }

        // ───────────────────────────────────────────
        // 2. combobox nền
        private async void cboBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentConversationId == Guid.Empty) return;

            string relPath = null;
            if (cboBackground.SelectedIndex > 0)
            {
                int idx = cboBackground.SelectedIndex - 1;
                relPath = _chatBackgrounds[idx];
            }

            _convBack[_currentConversationId] = relPath;
            ApplyBackgroundToMessagesPanel(relPath);

            // lưu DB
            await SaveConversationBackgroundAsync(_currentConversationId, relPath);
        }

        private async Task SaveConversationBackgroundAsync(Guid conversationId, string bgPath)
        {
            using (var conn = await Task.Run(() => Db.OpenConn()))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE Conversations SET BackgroundPath = @bg WHERE Id = @cid";
                cmd.Parameters.AddWithValue("@bg", (object)bgPath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cid", conversationId);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task<string> GetConversationBackgroundAsync(Guid conversationId)
        {
            using (var conn = await Task.Run(() => Db.OpenConn()))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT BackgroundPath FROM Conversations WHERE Id = @cid";
                cmd.Parameters.AddWithValue("@cid", conversationId);
                var v = await cmd.ExecuteScalarAsync();
                return v == DBNull.Value ? null : v as string;
            }
        }

        private void ApplyBackgroundToMessagesPanel(string relativePath)
        {
            if (panelMessages.BackgroundImage != null)
            {
                var old = panelMessages.BackgroundImage;
                panelMessages.BackgroundImage = null;
                old.Dispose();
            }

            if (string.IsNullOrEmpty(relativePath))
            {
                panelMessages.BackColor = Color.White;
                panelMessages.BackgroundImage = null;
                panelMessages.BackgroundImageLayout = ImageLayout.None;

                _palette = Palette.For(_theme);
                ApplyPalette(_palette);
                return;
            }

            try
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                if (!File.Exists(fullPath))
                {
                    panelMessages.BackColor = Color.White;
                    return;
                }

                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                using (var img = Image.FromStream(fs))
                {
                    panelMessages.BackgroundImage = new Bitmap(img);
                }

                panelMessages.BackgroundImageLayout = ImageLayout.Stretch;

                _palette = Palette.ForBackground(relativePath);
                ApplyPalette(_palette);
            }
            catch
            {
                panelMessages.BackColor = Color.White;
            }
        }

        // ───────────────────────────────────────────
        private async Task LoadConversationsAsync()
        {
            lvConversations.BeginUpdate();
            lvConversations.Items.Clear();
            _allConversations.Clear();

            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT c.Id,
       COALESCE(c.Title, (
           SELECT TOP 1 u.DisplayName
           FROM ConversationMembers m2
           JOIN Users u ON u.Id = m2.UserId
           WHERE m2.ConversationId = c.Id AND m2.UserId <> @uid
       )) AS Title,
       c.IsGroup,
       c.LastMessageAt
FROM Conversations c
JOIN ConversationMembers m ON m.ConversationId = c.Id AND m.UserId = @uid
ORDER BY c.LastMessageAt DESC, c.CreatedAt DESC;";
                    cmd.Parameters.AddWithValue("@uid", _currentUserId);

                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            Guid id = rd.GetGuid(0);
                            string title = rd.IsDBNull(1) ? "(Cuộc trò chuyện)" : rd.GetString(1);

                            _allConversations.Add(new ConvItem { Id = id, Title = title });

                            var item = new ListViewItem(title) { Tag = id };
                            lvConversations.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hội thoại: " + ex.Message);
            }
            finally
            {
                lvConversations.EndUpdate();
            }
        }

        // ───────────────────────────────────────────
        private async Task LoadConversationHeaderAsync(Guid conversationId)
        {
            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT 
    c.Title,
    c.IsGroup,
    COALESCE(c.Title, (
        SELECT TOP 1 COALESCE(NULLIF(u.DisplayName,''), u.UserName)
        FROM ConversationMembers m2
        JOIN Users u ON u.Id = m2.UserId
        WHERE m2.ConversationId = c.Id AND m2.UserId <> @uid
    )) AS DisplayTitle,
    (SELECT COUNT(*) FROM ConversationMembers m WHERE m.ConversationId = c.Id) AS MemberCount
FROM Conversations c
WHERE c.Id = @cid;";
                    cmd.Parameters.AddWithValue("@cid", conversationId);
                    cmd.Parameters.AddWithValue("@uid", _currentUserId);

                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        if (await rd.ReadAsync())
                        {
                            bool isGroup = rd.GetBoolean(1);
                            string title = rd.IsDBNull(2) ? "(Cuộc trò chuyện)" : rd.GetString(2);
                            int memberCount = rd.IsDBNull(3) ? 0 : rd.GetInt32(3);

                            lblName.Text = title;
                            lblStatus.Text = isGroup ? ("Nhóm · " + memberCount + " thành viên") : "online";
                            picAvatar.Image = isGroup ? SystemIcons.Application.ToBitmap()
                                                      : SystemIcons.Information.ToBitmap();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblName.Text = "(Lỗi)";
                lblStatus.Text = ex.Message;
            }
        }

        // ───────────────────────────────────────────
        // LOAD tin nhắn (có lọc IsDeleted = 0 cho khớp DB)
        private async Task LoadMessagesAsync(Guid conversationId)
        {
            panelMessages.SuspendLayout();
            panelMessages.Controls.Clear();

            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT Id, ConversationId, SenderId, Body, CreatedAt
FROM Messages
WHERE ConversationId = @cid AND (IsDeleted = 0 OR IsDeleted IS NULL)
ORDER BY CreatedAt ASC;";
                    cmd.Parameters.AddWithValue("@cid", conversationId);

                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            Guid msgId = rd.GetGuid(0);
                            Guid senderId = rd.GetGuid(2);
                            string body = rd.IsDBNull(3) ? "" : rd.GetString(3);
                            DateTime created = rd.GetDateTime(4);
                            bool mine = senderId == _currentUserId;

                            var bubble = new MessageBubble(msgId, body, created, mine, _palette);
                            bubble.Dock = DockStyle.Top;
                            AttachBubbleHandlers(bubble);
                            panelMessages.Controls.Add(bubble);
                            panelMessages.Controls.SetChildIndex(bubble, 0);
                        }
                    }
                }

                panelMessages.VerticalScroll.Value = panelMessages.VerticalScroll.Maximum;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải tin nhắn: " + ex.Message);
            }
            finally
            {
                panelMessages.ResumeLayout();
            }
        }

        private void AttachBubbleHandlers(MessageBubble bubble)
        {
            bubble.DeleteRequested += async (s, e) =>
            {
                await DeleteMessageAsync(bubble.MessageId, bubble, bubble.IsMine);
            };
        }

        private async Task DeleteMessageAsync(Guid messageId, MessageBubble bubble, bool isMine)
        {
            if (_currentConversationId == Guid.Empty) return;

            if (!isMine)
            {
                MessageBox.Show("Bạn chỉ được xóa tin nhắn của mình.");
                return;
            }

            if (MessageBox.Show("Xóa tin nhắn này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;

                    // xóa file đính kèm
                    cmd.CommandText = "DELETE FROM MessageAttachments WHERE MessageId = @mid";
                    cmd.Parameters.AddWithValue("@mid", messageId);
                    await cmd.ExecuteNonQueryAsync();

                    // xóa message (hard delete)
                    cmd.Parameters.Clear();
                    cmd.CommandText = "DELETE FROM Messages WHERE Id = @mid AND SenderId = @uid";
                    cmd.Parameters.AddWithValue("@mid", messageId);
                    cmd.Parameters.AddWithValue("@uid", _currentUserId);
                    int n = await cmd.ExecuteNonQueryAsync();

                    tran.Commit();

                    if (n == 0)
                    {
                        MessageBox.Show("Không xóa được tin (có thể không phải tin của bạn).");
                        return;
                    }
                }

                panelMessages.Controls.Remove(bubble);
                bubble.Dispose();

                await LoadSharedFilesAsync(_currentConversationId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa tin nhắn thất bại: " + ex.Message);
            }
        }

        // ───────────────────────────────────────────
        private async Task SendCurrentMessageAsync()
        {
            Guid mid = Guid.NewGuid();
            string text = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;
            if (_currentConversationId == Guid.Empty || _currentUserId == Guid.Empty) return;

            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var tran = conn.BeginTransaction())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = @"
INSERT INTO Messages(Id, ConversationId, SenderId, Body)
VALUES (@id, @cid, @sid, @body);

UPDATE Conversations SET LastMessageAt = SYSUTCDATETIME()
WHERE Id = @cid;";
                    cmd.Parameters.AddWithValue("@id", mid);
                    cmd.Parameters.AddWithValue("@cid", _currentConversationId);
                    cmd.Parameters.AddWithValue("@sid", _currentUserId);
                    cmd.Parameters.AddWithValue("@body", (object)text ?? DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                    tran.Commit();
                }

                var bubble = new MessageBubble(mid, text, DateTime.UtcNow, true, _palette)
                {
                    Dock = DockStyle.Top
                };
                AttachBubbleHandlers(bubble);

                panelMessages.Controls.Add(bubble);
                panelMessages.Controls.SetChildIndex(bubble, 0);
                panelMessages.VerticalScroll.Value = panelMessages.VerticalScroll.Maximum;
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi tin nhắn thất bại: " + ex.Message);
            }
        }

        // ───────────────────────────────────────────
        // SEND FILE – đã thêm cột Url để khớp DB của bạn
        private async Task SendFileAsync()
        {
            if (_currentConversationId == Guid.Empty || _currentUserId == Guid.Empty)
            {
                MessageBox.Show("Chưa chọn hội thoại.");
                return;
            }

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn file để gửi";
                ofd.Filter = "Tất cả file (.)|*.*";
                ofd.Multiselect = false;

                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                string filePath = ofd.FileName;
                string fileName = Path.GetFileName(filePath);
                long fileSize = new FileInfo(filePath).Length;

                try
                {
                    using (var conn = await Task.Run(() => Db.OpenConn()))
                    using (var tran = conn.BeginTransaction())
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;

                        Guid msgId = Guid.NewGuid();

                        // 1. insert message
                        cmd.CommandText = @"
INSERT INTO Messages(Id, ConversationId, SenderId, Body)
VALUES (@mid, @cid, @sid, @body);

UPDATE Conversations SET LastMessageAt = SYSUTCDATETIME()
WHERE Id = @cid;";
                        cmd.Parameters.AddWithValue("@mid", msgId);
                        cmd.Parameters.AddWithValue("@cid", _currentConversationId);
                        cmd.Parameters.AddWithValue("@sid", _currentUserId);
                        cmd.Parameters.AddWithValue("@body", (object)("Đã gửi tệp: " + fileName) ?? DBNull.Value);
                        await cmd.ExecuteNonQueryAsync();

                        // 2. insert attachment – DB bạn yêu cầu Url NOT NULL → mình để luôn = filePath
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
INSERT INTO MessageAttachments(MessageId, FileName, FileSize, Url, FilePath)
VALUES (@mid, @name, @size, @url, @path);";
                        cmd.Parameters.AddWithValue("@mid", msgId);
                        cmd.Parameters.AddWithValue("@name", fileName);
                        cmd.Parameters.AddWithValue("@size", fileSize);
                        cmd.Parameters.AddWithValue("@url", filePath);   // ← quan trọng
                        cmd.Parameters.AddWithValue("@path", filePath);

                        await cmd.ExecuteNonQueryAsync();

                        tran.Commit();

                        // 3. render
                        var bubble = new MessageBubble(
                            msgId,
                            "📎 " + fileName + $" ({fileSize / 1024.0:0.#} KB)",
                            DateTime.UtcNow,
                            true,
                            _palette
                        )
                        {
                            Dock = DockStyle.Top
                        };
                        AttachBubbleHandlers(bubble);

                        panelMessages.Controls.Add(bubble);
                        panelMessages.Controls.SetChildIndex(bubble, 0);
                        panelMessages.VerticalScroll.Value = panelMessages.VerticalScroll.Maximum;

                        await LoadSharedFilesAsync(_currentConversationId);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi SQL khi gửi file: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gửi file thất bại: " + ex.Message);
                }
            }
        }

        // ───────────────────────────────────────────
        private async Task LoadMembersAsync(Guid conversationId)
        {
            lstMembers.Items.Clear();

            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT u.Id,
       COALESCE(NULLIF(u.DisplayName,''), u.UserName) AS Name,
       CASE WHEN u.Id = @uid THEN 1 ELSE 0 END AS IsMe
FROM ConversationMembers m
JOIN Users u ON u.Id = m.UserId
WHERE m.ConversationId = @cid
ORDER BY IsMe DESC, Name ASC;";
                    cmd.Parameters.AddWithValue("@cid", conversationId);
                    cmd.Parameters.AddWithValue("@uid", _currentUserId);

                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            Guid uid = rd.GetGuid(0);
                            string name = rd.IsDBNull(1) ? "(Không tên)" : rd.GetString(1);
                            bool isMe = rd.GetInt32(2) == 1;

                            lstMembers.Items.Add(new MemberListItem
                            {
                                UserId = uid,
                                Text = isMe ? name + " (Bạn)" : name,
                                IsMe = isMe
                            });
                        }
                    }
                }

                lblParticipants.Text = "Thành viên (" + lstMembers.Items.Count + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thành viên: " + ex.Message);
            }
        }

        private async Task LoadSharedFilesAsync(Guid conversationId)
        {
            lvSharedFiles.Items.Clear();
            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT TOP 50 a.FileName, a.FileSize
FROM MessageAttachments a
JOIN Messages m ON m.Id = a.MessageId
WHERE m.ConversationId = @cid
ORDER BY a.Id DESC;";
                    cmd.Parameters.AddWithValue("@cid", conversationId);

                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            string name = rd.IsDBNull(0) ? "(tệp)" : rd.GetString(0);
                            long size = rd.GetInt64(1);
                            var item = new ListViewItem(name);
                            item.SubItems.Add(string.Format("{0:0.#} KB", size / 1024.0));
                            lvSharedFiles.Items.Add(item);
                        }
                    }
                }

                lblSharedFiles.Text = "Tệp đã chia sẻ (" + lvSharedFiles.Items.Count + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải tệp: " + ex.Message);
            }
        }

        // ───────────────────────────────────────────
        private void ToggleInfoPanel()
        {
            SetInfoVisibility(!_infoVisible);
        }

        private void SetInfoVisibility(bool show)
        {
            this.SuspendLayout();
            panelCenter.SuspendLayout();

            if (show)
            {
                panelRight.Width = _rightPanelWidth > 0 ? _rightPanelWidth : 260;
                panelRight.Visible = true;
                splitterRight.Visible = true;
                btnInfo.Text = ">";
            }
            else
            {
                if (panelRight.Visible && panelRight.Width > 0)
                    _rightPanelWidth = panelRight.Width;

                panelRight.Visible = false;
                splitterRight.Visible = false;
                btnInfo.Text = "<";
            }

            _infoVisible = show;
            PositionHeaderRightControls();

            panelCenter.ResumeLayout();
            this.ResumeLayout();

            panelMessages.PerformLayout();
            panelMessages.Invalidate();
            foreach (Control c in panelMessages.Controls)
            {
                if (c is MessageBubble mb)
                    mb.RefreshLayout();
            }
        }

        private void PositionHeaderRightControls()
        {
            int right = headerPanel.ClientSize.Width - 12;

            btnInfo.Left = right - btnInfo.Width;
            btnInfo.Top = 18;
            right = btnInfo.Left - 6;

            btnVideo.Left = right - btnVideo.Width;
            btnVideo.Top = 18;
            right = btnVideo.Left - 6;

            btnCall.Left = right - btnCall.Width;
            btnCall.Top = 18;
            right = btnCall.Left - 8;

            txtSearchInChat.Left = right - txtSearchInChat.Width;
            txtSearchInChat.Top = 22;
        }

        private void SetupPlaceholder(TextBox tb, string hint)
        {
            tb.Tag = hint;
            tb.Text = hint;
            tb.ForeColor = Color.Gray;

            tb.Enter += (s, e) =>
            {
                if (tb.Text == hint)
                {
                    tb.Text = "";
                    tb.ForeColor = Color.Black;
                }
            };
            tb.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = hint;
                    tb.ForeColor = Color.Gray;
                }
            };
        }

        private void ToggleTheme(AppTheme theme)
        {
            _theme = theme;
            _palette = Palette.For(theme);
            ApplyPalette(_palette);
        }

        private void ApplyPalette(Palette p)
        {
            this.BackColor = p.WindowBack;
            panelLeft.BackColor = p.SidebarBack;
            panelLeftTop.BackColor = p.HeaderBack;
            panelLeftBottom.BackColor = p.HeaderBack;
            lvConversations.BackColor = p.SidebarBack;

            panelCenter.BackColor = p.WindowBack;
            headerPanel.BackColor = p.HeaderBack;
            lblName.ForeColor = p.TextPrimary;
            lblStatus.ForeColor = p.TextSecondary;

            inputPanel.BackColor = p.InputBack;
            btnSend.BackColor = p.ButtonPrimary;
            btnSend.ForeColor = p.ButtonPrimaryText;

            panelRight.BackColor = p.HeaderBack;
            lblInfoTitle.ForeColor = p.TextPrimary;
            lblParticipants.ForeColor = p.TextPrimary;
            lblSharedFiles.ForeColor = p.TextPrimary;

            foreach (Control c in panelMessages.Controls)
            {
                if (c is MessageBubble b) b.ApplyTheme(p);
            }
        }

        // ───────────────────────────────────────────
        // bubble
        public class MessageBubble : UserControl
        {
            public Guid MessageId { get; }
            public bool IsMine => _isMine;

            private readonly Label _lblText = new Label();
            private readonly Label _lblTime = new Label();
            private bool _isMine;
            private Palette _palette;
            private bool _highlight = false;

            public event EventHandler DeleteRequested;

            public MessageBubble(Guid messageId, string text, DateTime time, bool mine, Palette palette)
            {
                MessageId = messageId;
                _isMine = mine;
                _palette = palette;
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.Transparent;
                this.Padding = new Padding(8);

                var container = new Panel();
                container.AutoSize = true;
                container.Padding = new Padding(12, 8, 12, 6);
                container.BackColor = Color.Transparent;

                _lblText.AutoSize = true;
                _lblText.MaximumSize = new Size(520, 0);
                _lblText.Text = text;
                _lblText.Font = new Font("Segoe UI", 10f);

                _lblTime.AutoSize = true;
                _lblTime.Text = time.ToString("HH:mm");
                _lblTime.Font = new Font("Segoe UI", 8f);

                container.Controls.Add(_lblText);
                container.Controls.Add(_lblTime);

                _lblTime.Top = _lblText.Bottom + 2;
                _lblTime.Left = _lblText.Left;

                this.Controls.Add(container);

                container.Resize += (s, e) => UpdateBubbleBounds(container);
                this.Resize += (s, e) => UpdateBubbleBounds(container);

                // context menu
                var cm = new ContextMenuStrip();
                var miDel = new ToolStripMenuItem("Xóa tin nhắn này");
                miDel.Click += (s, e) => DeleteRequested?.Invoke(this, EventArgs.Empty);
                cm.Items.Add(miDel);
                this.ContextMenuStrip = cm;
                if (!mine) miDel.Enabled = false;

                ApplyTheme(palette);
            }

            public void ApplyTheme(Palette p)
            {
                _palette = p;
                _lblText.ForeColor = Color.Black;
                _lblTime.ForeColor = Color.FromArgb(120, 120, 120);
                Invalidate();
            }

            public void RefreshLayout()
            {
                if (Controls.Count == 0) return;
                var container = Controls[0] as Panel;
                if (container == null) return;
                UpdateBubbleBounds(container);
            }

            private void UpdateBubbleBounds(Panel container)
            {
                int marginOuter = 16;
                container.MaximumSize = new Size(560, 0);
                container.AutoSize = true;

                int w = this.ClientSize.Width;
                if (w <= 0) w = this.Width;

                if (_isMine)
                    container.Left = Math.Max(marginOuter, w - container.Width - marginOuter);
                else
                    container.Left = marginOuter;

                container.Top = 0;
                this.Height = container.Height + 4;
            }

            public bool ContainsText(string keyword)
            {
                if (string.IsNullOrEmpty(keyword)) return false;
                return _lblText.Text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            public void SetHighlight(bool on)
            {
                _highlight = on;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                if (Controls.Count == 0) return;
                var container = Controls[0] as Panel;
                if (container == null) return;

                var rect = new Rectangle(container.Left - 8, container.Top - 4,
                                         container.Width + 16, container.Height + 8);
                int radius = 14;

                using (var path = Rounded(rect, radius))
                {
                    Color fill;
                    if (_highlight)
                        fill = Color.FromArgb(255, 249, 196);
                    else
                        fill = _isMine ? _palette.BubbleMine : _palette.BubbleOther;

                    using (var br = new SolidBrush(fill))
                    {
                        g.FillPath(br, path);
                    }
                }
            }

            private static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle b, int r)
            {
                int d = r * 2;
                var p = new System.Drawing.Drawing2D.GraphicsPath();
                p.AddArc(b.Left, b.Top, d, d, 180, 90);
                p.AddArc(b.Right - d, b.Top, d, d, 270, 90);
                p.AddArc(b.Right - d, b.Bottom - d, d, d, 0, 90);
                p.AddArc(b.Left, b.Bottom - d, d, d, 90, 90);
                p.CloseFigure();
                return p;
            }
        }

        // ───────────────────────────────────────────
        public class Palette
        {
            public Color WindowBack;
            public Color SidebarBack;
            public Color HeaderBack;
            public Color MessagesBack;
            public Color InputBack;
            public Color TextPrimary;
            public Color TextSecondary;
            public Color ButtonPrimary;
            public Color ButtonPrimaryText;
            public Color BubbleMine;
            public Color BubbleOther;

            public static Palette For(AppTheme t)
            {
                if (t == AppTheme.Dark)
                {
                    return new Palette
                    {
                        WindowBack = Color.FromArgb(24, 26, 27),
                        SidebarBack = Color.FromArgb(32, 34, 36),
                        HeaderBack = Color.FromArgb(32, 34, 36),
                        MessagesBack = Color.FromArgb(24, 26, 27),
                        InputBack = Color.FromArgb(32, 34, 36),
                        TextPrimary = Color.Gainsboro,
                        TextSecondary = Color.Silver,
                        ButtonPrimary = Color.FromArgb(33, 150, 243),
                        ButtonPrimaryText = Color.White,
                        BubbleMine = Color.FromArgb(0, 137, 123),
                        BubbleOther = Color.FromArgb(55, 71, 79)
                    };
                }

                return new Palette
                {
                    WindowBack = Color.White,
                    SidebarBack = Color.FromArgb(245, 246, 248),
                    HeaderBack = Color.White,
                    MessagesBack = Color.White,
                    InputBack = Color.FromArgb(250, 250, 250),
                    TextPrimary = Color.Black,
                    TextSecondary = Color.DimGray,
                    ButtonPrimary = Color.FromArgb(25, 118, 210),
                    ButtonPrimaryText = Color.White,
                    BubbleMine = Color.FromArgb(209, 233, 252),
                    BubbleOther = Color.FromArgb(237, 231, 246)
                };
            }

            public static Palette ForBackground(string file)
            {
                file = (file ?? "").ToLowerInvariant();

                if (file.Contains("love") || file.Contains("pink"))
                {
                    return new Palette
                    {
                        WindowBack = Color.White,
                        SidebarBack = Color.FromArgb(245, 246, 248),
                        HeaderBack = Color.White,
                        MessagesBack = Color.White,
                        InputBack = Color.FromArgb(255, 245, 248),
                        TextPrimary = Color.Black,
                        TextSecondary = Color.DimGray,
                        ButtonPrimary = Color.FromArgb(255, 105, 180),
                        ButtonPrimaryText = Color.White,
                        BubbleMine = Color.FromArgb(255, 238, 246),
                        BubbleOther = Color.FromArgb(255, 255, 255)
                    };
                }

                if (file.Contains("doodle") || file.Contains("trochuyen") || file.Contains("snoopy"))
                {
                    return new Palette
                    {
                        WindowBack = Color.White,
                        SidebarBack = Color.FromArgb(245, 246, 248),
                        HeaderBack = Color.White,
                        MessagesBack = Color.White,
                        InputBack = Color.FromArgb(250, 250, 250),
                        TextPrimary = Color.Black,
                        TextSecondary = Color.DimGray,
                        ButtonPrimary = Color.FromArgb(25, 118, 210),
                        ButtonPrimaryText = Color.White,
                        BubbleMine = Color.FromArgb(210, 232, 255),
                        BubbleOther = Color.FromArgb(240, 244, 248)
                    };
                }

                if (file.Contains("bear"))
                {
                    return new Palette
                    {
                        WindowBack = Color.White,
                        SidebarBack = Color.FromArgb(245, 246, 248),
                        HeaderBack = Color.White,
                        MessagesBack = Color.White,
                        InputBack = Color.FromArgb(250, 250, 245),
                        TextPrimary = Color.Black,
                        TextSecondary = Color.DimGray,
                        ButtonPrimary = Color.FromArgb(221, 167, 80),
                        ButtonPrimaryText = Color.White,
                        BubbleMine = Color.FromArgb(255, 248, 230),
                        BubbleOther = Color.FromArgb(255, 255, 255)
                    };
                }

                return For(AppTheme.Light);
            }
        }

        // ───────────────────────────────────────────
        private class MemberListItem
        {
            public Guid UserId;
            public string Text;
            public bool IsMe;
            public override string ToString() { return Text; }
        }

        // ───────────────────────────────────────────
        private void ApplyConversationFilter()
        {
            string hint = txtGlobalSearch.Tag as string ?? "";      // SetupPlaceholder đã gán Tag = hint
            string q = (txtGlobalSearch.Text ?? "").Trim();

            bool isEmpty = string.IsNullOrEmpty(q) || string.Equals(q, hint, StringComparison.Ordinal);

            lvConversations.BeginUpdate();
            lvConversations.Items.Clear();

            if (isEmpty)
            {
                // Hiện toàn bộ
                foreach (var c in _allConversations)
                    lvConversations.Items.Add(new ListViewItem(c.Title) { Tag = c.Id });
            }
            else
            {
                // Lọc theo từ khóa
                foreach (var c in _allConversations)
                    if (!string.IsNullOrEmpty(c.Title) &&
                        c.Title.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                        lvConversations.Items.Add(new ListViewItem(c.Title) { Tag = c.Id });
            }

            lvConversations.EndUpdate();
        }




        private void txtSearchInChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SearchInMessages(txtSearchInChat.Text);
            }
        }

        private void SearchInMessages(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return;

            keyword = keyword.Trim();

            foreach (Control c in panelMessages.Controls)
                if (c is MessageBubble mbOld)
                    mbOld.SetHighlight(false);

            MessageBubble found = null;

            for (int i = 0; i < panelMessages.Controls.Count; i++)
            {
                if (panelMessages.Controls[i] is MessageBubble mb)
                {
                    if (mb.ContainsText(keyword))
                    {
                        found = mb;
                        break;
                    }
                }
            }

            if (found == null)
            {
                MessageBox.Show("Không tìm thấy: " + keyword);
                return;
            }

            found.SetHighlight(true);
            panelMessages.ScrollControlIntoView(found);
        }

        // emoji
        private void btnEmoji_Click(object sender, EventArgs e)
        {
            var emojiForm = new EmojiPickerForm();
            var btn = sender as Button;
            var screenPoint = btn.PointToScreen(Point.Empty);

            emojiForm.Location = new Point(screenPoint.X, screenPoint.Y - emojiForm.Height);
            emojiForm.EmojiSelected += (emoji) =>
            {
                txtMessage.AppendText(emoji);
            };

            emojiForm.Show();
        }

        // kiểm tra nhóm
        private async Task<bool> IsGroupAsync(Guid conversationId)
        {
            using (var conn = await Task.Run(() => Db.OpenConn()))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT IsGroup FROM Conversations WHERE Id=@cid";
                cmd.Parameters.AddWithValue("@cid", conversationId);
                var v = await cmd.ExecuteScalarAsync();
                return v != null && Convert.ToBoolean(v);
            }
        }

        // popup input
        private string PromptInput(string title, string label, string placeholder)
        {
            using (var f = new Form())
            {
                f.Text = title;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ClientSize = new Size(360, 130);
                f.MinimizeBox = false;
                f.MaximizeBox = false;

                var lb = new Label { Text = label, AutoSize = true, Location = new Point(12, 12) };
                var txt = new TextBox { Location = new Point(12, 36), Width = 336, Text = placeholder ?? "" };
                var ok = new Button { Text = "OK", Location = new Point(186, 80), DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "Hủy", Location = new Point(270, 80), DialogResult = DialogResult.Cancel };

                f.Controls.AddRange(new Control[] { lb, txt, ok, cancel });
                f.AcceptButton = ok;
                f.CancelButton = cancel;

                if (f.ShowDialog(this) == DialogResult.OK)
                    return txt.Text.Trim();
                return null;
            }
        }

        private async Task PromptAddMemberAsync()
        {
            if (_currentConversationId == Guid.Empty) return;

            if (!await IsGroupAsync(_currentConversationId))
            {
                MessageBox.Show("Chỉ nhóm mới thêm được thành viên.");
                return;
            }

            var q = PromptInput("Thêm thành viên", "Nhập username hoặc email:", "");
            if (string.IsNullOrWhiteSpace(q)) return;

            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
DECLARE @uid UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Users WHERE UserName=@q OR Email=@q);
IF @uid IS NULL
BEGIN
    RAISERROR(N'Không tìm thấy người dùng.', 16, 1);
    RETURN;
END

IF EXISTS (SELECT 1 FROM ConversationMembers WHERE ConversationId=@cid AND UserId=@uid)
BEGIN
    RAISERROR(N'Người dùng đã là thành viên.', 16, 1);
    RETURN;
END

INSERT INTO ConversationMembers(ConversationId, UserId, Role) VALUES(@cid, @uid, 0);";
                    cmd.Parameters.AddWithValue("@cid", _currentConversationId);
                    cmd.Parameters.AddWithValue("@q", q);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LoadMembersAsync(_currentConversationId);
                await LoadConversationHeaderAsync(_currentConversationId);
                MessageBox.Show("Đã thêm thành viên.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi SQL");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thành viên thất bại: " + ex.Message);
            }
        }

        private async Task RemoveSelectedMemberAsync()
        {
            if (lstMembers.SelectedItem == null) return;
            var item = lstMembers.SelectedItem as MemberListItem;
            if (item == null) return;

            await RemoveSelectedMemberAsync(item.UserId, item.Text);
        }

        private async Task RemoveSelectedMemberAsync(Guid userId, string nameForConfirm)
        {
            if (_currentConversationId == Guid.Empty) return;

            if (!await IsGroupAsync(_currentConversationId))
            {
                MessageBox.Show("Chỉ nhóm mới xóa được thành viên.");
                return;
            }

            if (MessageBox.Show("Xóa '" + nameForConfirm + "' khỏi nhóm?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (var conn = await Task.Run(() => Db.OpenConn()))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
IF EXISTS (SELECT 1 FROM ConversationMembers WHERE ConversationId=@cid AND UserId=@uid AND Role=1)
   AND (SELECT COUNT(*) FROM ConversationMembers WHERE ConversationId=@cid AND Role=1) = 1
BEGIN
    RAISERROR(N'Không thể xóa admin cuối cùng của nhóm.', 16, 1);
    RETURN;
END

DELETE FROM ConversationMembers WHERE ConversationId=@cid AND UserId=@uid;";
                    cmd.Parameters.AddWithValue("@cid", _currentConversationId);
                    cmd.Parameters.AddWithValue("@uid", userId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LoadMembersAsync(_currentConversationId);
                await LoadConversationHeaderAsync(_currentConversationId);
                MessageBox.Show("Đã xóa thành viên.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi SQL");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa thành viên thất bại: " + ex.Message);
            }
        }

        // ───────────────────────────────────────────
        private static bool IsInDesignMode()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            try
            {
                return System.Diagnostics.Process.GetCurrentProcess().ProcessName
                    .Equals("devenv", StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        private void MessengerForm_Load(object sender, EventArgs e)
        {

        }
    }
}