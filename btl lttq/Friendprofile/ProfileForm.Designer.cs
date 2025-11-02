namespace btl_lttq.Friendprofile
{
    partial class ProfileForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblHometown;
        private System.Windows.Forms.Label lblEducation;
        private System.Windows.Forms.Label lblWork;
        private System.Windows.Forms.Label lblRelationship;
        private btl_lttq.Friendprofile.RoundedTextBox txtFullName;
        private btl_lttq.Friendprofile.RoundedTextBox txtEmail;
        private btl_lttq.Friendprofile.RoundedTextBox txtPhone;
        private btl_lttq.Friendprofile.RoundedTextBox txtHometown;
        private btl_lttq.Friendprofile.RoundedTextBox txtEducation;
        private btl_lttq.Friendprofile.RoundedTextBox txtWork;
        private btl_lttq.Friendprofile.RoundedTextBox txtRelationship;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnUpdate;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblHometown = new System.Windows.Forms.Label();
            this.lblEducation = new System.Windows.Forms.Label();
            this.lblWork = new System.Windows.Forms.Label();
            this.lblRelationship = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtRelationship = new btl_lttq.Friendprofile.RoundedTextBox();
            this.txtWork = new btl_lttq.Friendprofile.RoundedTextBox();
            this.txtEducation = new btl_lttq.Friendprofile.RoundedTextBox();
            this.txtHometown = new btl_lttq.Friendprofile.RoundedTextBox();
            this.txtPhone = new btl_lttq.Friendprofile.RoundedTextBox();
            this.txtEmail = new btl_lttq.Friendprofile.RoundedTextBox();
            this.txtFullName = new btl_lttq.Friendprofile.RoundedTextBox();
            this.cboGender = new btl_lttq.Friendprofile.RoundedComboBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(153, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(221, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thông tin cá nhân";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblName.Location = new System.Drawing.Point(304, 56);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(76, 16);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Họ và Tên";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblEmail.Location = new System.Drawing.Point(304, 105);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(44, 16);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblPhone.Location = new System.Drawing.Point(304, 154);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(99, 16);
            this.lblPhone.TabIndex = 3;
            this.lblPhone.Text = "Số điện thoại";
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblGender.Location = new System.Drawing.Point(304, 205);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(67, 16);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "Giới tính";
            // 
            // lblHometown
            // 
            this.lblHometown.AutoSize = true;
            this.lblHometown.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHometown.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblHometown.Location = new System.Drawing.Point(37, 56);
            this.lblHometown.Name = "lblHometown";
            this.lblHometown.Size = new System.Drawing.Size(74, 16);
            this.lblHometown.TabIndex = 5;
            this.lblHometown.Text = "Quê quán";
            // 
            // lblEducation
            // 
            this.lblEducation.AutoSize = true;
            this.lblEducation.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEducation.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblEducation.Location = new System.Drawing.Point(37, 105);
            this.lblEducation.Name = "lblEducation";
            this.lblEducation.Size = new System.Drawing.Size(64, 16);
            this.lblEducation.TabIndex = 6;
            this.lblEducation.Text = "Học vấn";
            // 
            // lblWork
            // 
            this.lblWork.AutoSize = true;
            this.lblWork.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWork.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblWork.Location = new System.Drawing.Point(37, 154);
            this.lblWork.Name = "lblWork";
            this.lblWork.Size = new System.Drawing.Size(93, 16);
            this.lblWork.TabIndex = 7;
            this.lblWork.Text = "Nơi làm việc";
            // 
            // lblRelationship
            // 
            this.lblRelationship.AutoSize = true;
            this.lblRelationship.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRelationship.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblRelationship.Location = new System.Drawing.Point(37, 204);
            this.lblRelationship.Name = "lblRelationship";
            this.lblRelationship.Size = new System.Drawing.Size(139, 16);
            this.lblRelationship.TabIndex = 8;
            this.lblRelationship.Text = "Tình trạng quan hệ";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(83, 290);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(103, 30);
            this.btnEdit.TabIndex = 17;
            this.btnEdit.Text = "✏️ Sửa thông tin";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(300, 290);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(103, 30);
            this.btnUpdate.TabIndex = 18;
            this.btnUpdate.Text = "💾 Cập nhật";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtRelationship
            // 
            this.txtRelationship.BackColor = System.Drawing.Color.White;
            this.txtRelationship.EditingMode = false;
            this.txtRelationship.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRelationship.ForeColor = System.Drawing.Color.Black;
            this.txtRelationship.Location = new System.Drawing.Point(34, 225);
            this.txtRelationship.Name = "txtRelationship";
            this.txtRelationship.Padding = new System.Windows.Forms.Padding(4);
            this.txtRelationship.ReadOnly = false;
            this.txtRelationship.Size = new System.Drawing.Size(172, 26);
            this.txtRelationship.TabIndex = 19;
            this.txtRelationship.TextValue = "";
            // 
            // txtWork
            // 
            this.txtWork.BackColor = System.Drawing.Color.White;
            this.txtWork.EditingMode = false;
            this.txtWork.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtWork.ForeColor = System.Drawing.Color.Black;
            this.txtWork.Location = new System.Drawing.Point(34, 172);
            this.txtWork.Name = "txtWork";
            this.txtWork.Padding = new System.Windows.Forms.Padding(4);
            this.txtWork.ReadOnly = false;
            this.txtWork.Size = new System.Drawing.Size(172, 26);
            this.txtWork.TabIndex = 20;
            this.txtWork.TextValue = "";
            // 
            // txtEducation
            // 
            this.txtEducation.BackColor = System.Drawing.Color.White;
            this.txtEducation.EditingMode = false;
            this.txtEducation.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEducation.ForeColor = System.Drawing.Color.Black;
            this.txtEducation.Location = new System.Drawing.Point(34, 121);
            this.txtEducation.Name = "txtEducation";
            this.txtEducation.Padding = new System.Windows.Forms.Padding(4);
            this.txtEducation.ReadOnly = false;
            this.txtEducation.Size = new System.Drawing.Size(172, 26);
            this.txtEducation.TabIndex = 21;
            this.txtEducation.TextValue = "";
            // 
            // txtHometown
            // 
            this.txtHometown.BackColor = System.Drawing.Color.White;
            this.txtHometown.EditingMode = false;
            this.txtHometown.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtHometown.ForeColor = System.Drawing.Color.Black;
            this.txtHometown.Location = new System.Drawing.Point(34, 72);
            this.txtHometown.Name = "txtHometown";
            this.txtHometown.Padding = new System.Windows.Forms.Padding(4);
            this.txtHometown.ReadOnly = false;
            this.txtHometown.Size = new System.Drawing.Size(172, 26);
            this.txtHometown.TabIndex = 22;
            this.txtHometown.TextValue = "";
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.White;
            this.txtPhone.EditingMode = false;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPhone.ForeColor = System.Drawing.Color.Black;
            this.txtPhone.Location = new System.Drawing.Point(301, 172);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Padding = new System.Windows.Forms.Padding(4);
            this.txtPhone.ReadOnly = false;
            this.txtPhone.Size = new System.Drawing.Size(172, 26);
            this.txtPhone.TabIndex = 24;
            this.txtPhone.TextValue = "";
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.EditingMode = false;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail.ForeColor = System.Drawing.Color.Black;
            this.txtEmail.Location = new System.Drawing.Point(301, 121);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Padding = new System.Windows.Forms.Padding(4);
            this.txtEmail.ReadOnly = false;
            this.txtEmail.Size = new System.Drawing.Size(172, 26);
            this.txtEmail.TabIndex = 25;
            this.txtEmail.TextValue = "";
            // 
            // txtFullName
            // 
            this.txtFullName.BackColor = System.Drawing.Color.White;
            this.txtFullName.EditingMode = false;
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFullName.ForeColor = System.Drawing.Color.Black;
            this.txtFullName.Location = new System.Drawing.Point(301, 72);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Padding = new System.Windows.Forms.Padding(4);
            this.txtFullName.ReadOnly = false;
            this.txtFullName.Size = new System.Drawing.Size(172, 26);
            this.txtFullName.TabIndex = 26;
            this.txtFullName.TextValue = "";
            // 
            // cboGender
            // 
            this.cboGender.BackColor = System.Drawing.Color.White;
            this.cboGender.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboGender.DropDownHeight = 168;
            this.cboGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboGender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cboGender.FormattingEnabled = true;
            this.cboGender.IntegralHeight = false;
            this.cboGender.ItemHeight = 28;
            this.cboGender.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Khác",
            "Nam",
            "Nữ",
            "Khác",
            "Nam",
            "Nữ",
            "Khác",
            "Nam",
            "Nữ",
            "Khác"});
            this.cboGender.Location = new System.Drawing.Point(301, 225);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new System.Drawing.Size(172, 34);
            this.cboGender.TabIndex = 27;
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 349);
            this.Controls.Add(this.cboGender);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.txtRelationship);
            this.Controls.Add(this.txtWork);
            this.Controls.Add(this.txtEducation);
            this.Controls.Add(this.txtHometown);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblRelationship);
            this.Controls.Add(this.lblWork);
            this.Controls.Add(this.lblEducation);
            this.Controls.Add(this.lblHometown);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblTitle);
            this.Name = "ProfileForm";
            this.Text = "Hồ sơ cá nhân";
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private RoundedComboBox cboGender;
    }
}
