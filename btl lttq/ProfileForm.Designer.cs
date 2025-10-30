namespace btl_lttq
{
    partial class ProfileForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblHometown;
        private System.Windows.Forms.Label lblEducation;
        private System.Windows.Forms.Label lblWork;
        private System.Windows.Forms.Label lblRelationship;
        private btl_lttq.RoundedTextBox txtFullName;
        private btl_lttq.RoundedTextBox txtEmail;
        private btl_lttq.RoundedTextBox txtPhone;
        private btl_lttq.RoundedComboBox cboGender;
        private btl_lttq.RoundedTextBox txtHometown;
        private btl_lttq.RoundedTextBox txtEducation;
        private btl_lttq.RoundedTextBox txtWork;
        private btl_lttq.RoundedTextBox txtRelationship;
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
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblHometown = new System.Windows.Forms.Label();
            this.lblEducation = new System.Windows.Forms.Label();
            this.lblWork = new System.Windows.Forms.Label();
            this.lblRelationship = new System.Windows.Forms.Label();
            this.txtFullName = new btl_lttq.RoundedTextBox();
            this.txtEmail = new btl_lttq.RoundedTextBox();
            this.txtPhone = new btl_lttq.RoundedTextBox();
            this.cboGender = new btl_lttq.RoundedComboBox();
            this.txtHometown = new btl_lttq.RoundedTextBox();
            this.txtEducation = new btl_lttq.RoundedTextBox();
            this.txtWork = new btl_lttq.RoundedTextBox();
            this.txtRelationship = new btl_lttq.RoundedTextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // picAvatar
            // 
            this.picAvatar.Location = new System.Drawing.Point(12, 13);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(103, 104);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 19;
            this.picAvatar.TabStop = false;
            this.picAvatar.Click += new System.EventHandler(this.picAvatar_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(177, 72);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thông tin cá nhân";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(307, 125);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(26, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Tên";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(301, 173);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(301, 221);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(70, 13);
            this.lblPhone.TabIndex = 3;
            this.lblPhone.Text = "Số điện thoại";
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(301, 269);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(47, 13);
            this.lblGender.TabIndex = 4;
            this.lblGender.Text = "Giới tính";
            // 
            // lblHometown
            // 
            this.lblHometown.AutoSize = true;
            this.lblHometown.Location = new System.Drawing.Point(34, 119);
            this.lblHometown.Name = "lblHometown";
            this.lblHometown.Size = new System.Drawing.Size(54, 13);
            this.lblHometown.TabIndex = 5;
            this.lblHometown.Text = "Quê quán";
            // 
            // lblEducation
            // 
            this.lblEducation.AutoSize = true;
            this.lblEducation.Location = new System.Drawing.Point(34, 168);
            this.lblEducation.Name = "lblEducation";
            this.lblEducation.Size = new System.Drawing.Size(48, 13);
            this.lblEducation.TabIndex = 6;
            this.lblEducation.Text = "Học vấn";
            // 
            // lblWork
            // 
            this.lblWork.AutoSize = true;
            this.lblWork.Location = new System.Drawing.Point(28, 219);
            this.lblWork.Name = "lblWork";
            this.lblWork.Size = new System.Drawing.Size(65, 13);
            this.lblWork.TabIndex = 7;
            this.lblWork.Text = "Nơi làm việc";
            // 
            // lblRelationship
            // 
            this.lblRelationship.AutoSize = true;
            this.lblRelationship.Location = new System.Drawing.Point(34, 267);
            this.lblRelationship.Name = "lblRelationship";
            this.lblRelationship.Size = new System.Drawing.Size(97, 13);
            this.lblRelationship.TabIndex = 8;
            this.lblRelationship.Text = "Tình trạng quan hệ";
            // 
            // txtFullName
            // 
            this.txtFullName.BackColor = System.Drawing.Color.White;
            this.txtFullName.Location = new System.Drawing.Point(298, 140);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Padding = new System.Windows.Forms.Padding(4);
            this.txtFullName.ReadOnly = false;
            this.txtFullName.Size = new System.Drawing.Size(172, 30);
            this.txtFullName.TabIndex = 26;
            this.txtFullName.TextValue = "";
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.Location = new System.Drawing.Point(298, 186);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Padding = new System.Windows.Forms.Padding(4);
            this.txtEmail.ReadOnly = false;
            this.txtEmail.Size = new System.Drawing.Size(172, 30);
            this.txtEmail.TabIndex = 25;
            this.txtEmail.TextValue = "";
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.White;
            this.txtPhone.Location = new System.Drawing.Point(298, 237);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Padding = new System.Windows.Forms.Padding(4);
            this.txtPhone.ReadOnly = false;
            this.txtPhone.Size = new System.Drawing.Size(172, 30);
            this.txtPhone.TabIndex = 24;
            this.txtPhone.TextValue = "";
            // 
            // cboGender
            // 
            this.cboGender.BackColor = System.Drawing.Color.White;
            this.cboGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboGender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboGender.ForeColor = System.Drawing.Color.DimGray;
            this.cboGender.Location = new System.Drawing.Point(298, 285);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new System.Drawing.Size(172, 24);
            this.cboGender.TabIndex = 23;
            // 
            // txtHometown
            // 
            this.txtHometown.BackColor = System.Drawing.Color.White;
            this.txtHometown.Location = new System.Drawing.Point(31, 135);
            this.txtHometown.Name = "txtHometown";
            this.txtHometown.Padding = new System.Windows.Forms.Padding(4);
            this.txtHometown.ReadOnly = false;
            this.txtHometown.Size = new System.Drawing.Size(172, 30);
            this.txtHometown.TabIndex = 22;
            this.txtHometown.TextValue = "";
            // 
            // txtEducation
            // 
            this.txtEducation.BackColor = System.Drawing.Color.White;
            this.txtEducation.Location = new System.Drawing.Point(31, 184);
            this.txtEducation.Name = "txtEducation";
            this.txtEducation.Padding = new System.Windows.Forms.Padding(4);
            this.txtEducation.ReadOnly = false;
            this.txtEducation.Size = new System.Drawing.Size(172, 30);
            this.txtEducation.TabIndex = 21;
            this.txtEducation.TextValue = "";
            // 
            // txtWork
            // 
            this.txtWork.BackColor = System.Drawing.Color.White;
            this.txtWork.Location = new System.Drawing.Point(31, 235);
            this.txtWork.Name = "txtWork";
            this.txtWork.Padding = new System.Windows.Forms.Padding(4);
            this.txtWork.ReadOnly = false;
            this.txtWork.Size = new System.Drawing.Size(172, 30);
            this.txtWork.TabIndex = 20;
            this.txtWork.TextValue = "";
            // 
            // txtRelationship
            // 
            this.txtRelationship.BackColor = System.Drawing.Color.White;
            this.txtRelationship.Location = new System.Drawing.Point(31, 283);
            this.txtRelationship.Name = "txtRelationship";
            this.txtRelationship.Padding = new System.Windows.Forms.Padding(4);
            this.txtRelationship.ReadOnly = false;
            this.txtRelationship.Size = new System.Drawing.Size(172, 30);
            this.txtRelationship.TabIndex = 19;
            this.txtRelationship.TextValue = "";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(100, 338);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(103, 30);
            this.btnEdit.TabIndex = 17;
            this.btnEdit.Text = "✏️ Sửa thông tin";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(277, 338);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(103, 30);
            this.btnUpdate.TabIndex = 18;
            this.btnUpdate.Text = "💾 Cập nhật";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 377);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.txtRelationship);
            this.Controls.Add(this.txtWork);
            this.Controls.Add(this.txtEducation);
            this.Controls.Add(this.txtHometown);
            this.Controls.Add(this.cboGender);
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
            this.Controls.Add(this.picAvatar);
            this.Name = "ProfileForm";
            this.Text = "Hồ sơ cá nhân";
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
