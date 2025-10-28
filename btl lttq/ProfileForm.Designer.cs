namespace btl_lttq
{
    partial class ProfileForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblHometown = new System.Windows.Forms.Label();
            this.lblEducation = new System.Windows.Forms.Label();
            this.lblWork = new System.Windows.Forms.Label();
            this.lblRelationship = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.cboGender = new System.Windows.Forms.TextBox();
            this.txtHometown = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.txtWork = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRelationship = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtRelationship);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtWork);
            this.panel2.Controls.Add(this.textBox7);
            this.panel2.Controls.Add(this.txtHometown);
            this.panel2.Controls.Add(this.cboGender);
            this.panel2.Controls.Add(this.txtPhone);
            this.panel2.Controls.Add(this.txtEmail);
            this.panel2.Controls.Add(this.txtFullName);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblRelationship);
            this.panel2.Controls.Add(this.lblWork);
            this.panel2.Controls.Add(this.lblEducation);
            this.panel2.Controls.Add(this.lblHometown);
            this.panel2.Controls.Add(this.lblPhone);
            this.panel2.Controls.Add(this.lblEmail);
            this.panel2.Controls.Add(this.lblName);
            this.panel2.Controls.Add(this.picAvatar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 472);
            this.panel2.TabIndex = 1;
            // 
            // picAvatar
            // 
            this.picAvatar.Location = new System.Drawing.Point(115, 12);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(134, 114);
            this.picAvatar.TabIndex = 0;
            this.picAvatar.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(112, 184);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(26, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Tên";
            this.lblName.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(107, 210);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(31, 13);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "email";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(107, 236);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(70, 13);
            this.lblPhone.TabIndex = 3;
            this.lblPhone.Text = "Số điện thoại";
            this.lblPhone.Click += new System.EventHandler(this.label3_Click);
            // 
            // lblHometown
            // 
            this.lblHometown.AutoSize = true;
            this.lblHometown.Location = new System.Drawing.Point(107, 291);
            this.lblHometown.Name = "lblHometown";
            this.lblHometown.Size = new System.Drawing.Size(54, 13);
            this.lblHometown.TabIndex = 5;
            this.lblHometown.Text = "Quê quán";
            // 
            // lblEducation
            // 
            this.lblEducation.AutoSize = true;
            this.lblEducation.Location = new System.Drawing.Point(107, 317);
            this.lblEducation.Name = "lblEducation";
            this.lblEducation.Size = new System.Drawing.Size(48, 13);
            this.lblEducation.TabIndex = 6;
            this.lblEducation.Text = "Học vấn";
            // 
            // lblWork
            // 
            this.lblWork.AutoSize = true;
            this.lblWork.Location = new System.Drawing.Point(107, 342);
            this.lblWork.Name = "lblWork";
            this.lblWork.Size = new System.Drawing.Size(65, 13);
            this.lblWork.TabIndex = 7;
            this.lblWork.Text = "Nơi làm việc";
            // 
            // lblRelationship
            // 
            this.lblRelationship.AutoSize = true;
            this.lblRelationship.Location = new System.Drawing.Point(106, 370);
            this.lblRelationship.Name = "lblRelationship";
            this.lblRelationship.Size = new System.Drawing.Size(97, 13);
            this.lblRelationship.TabIndex = 8;
            this.lblRelationship.Text = "Tình trạng quan hệ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Thông tin cá nhân";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(234, 177);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(100, 20);
            this.txtFullName.TabIndex = 10;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(234, 203);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(100, 20);
            this.txtEmail.TabIndex = 11;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(234, 233);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(100, 20);
            this.txtPhone.TabIndex = 12;
            // 
            // cboGender
            // 
            this.cboGender.Location = new System.Drawing.Point(234, 256);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new System.Drawing.Size(100, 20);
            this.cboGender.TabIndex = 13;
            // 
            // txtHometown
            // 
            this.txtHometown.Location = new System.Drawing.Point(234, 288);
            this.txtHometown.Name = "txtHometown";
            this.txtHometown.Size = new System.Drawing.Size(100, 20);
            this.txtHometown.TabIndex = 15;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(234, 320);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 20);
            this.textBox7.TabIndex = 16;
            // 
            // txtWork
            // 
            this.txtWork.Location = new System.Drawing.Point(234, 346);
            this.txtWork.Name = "txtWork";
            this.txtWork.Size = new System.Drawing.Size(100, 20);
            this.txtWork.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 263);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Giới tính";
            // 
            // txtRelationship
            // 
            this.txtRelationship.Location = new System.Drawing.Point(234, 372);
            this.txtRelationship.Name = "txtRelationship";
            this.txtRelationship.Size = new System.Drawing.Size(100, 20);
            this.txtRelationship.TabIndex = 19;
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 510);
            this.Controls.Add(this.panel2);
            this.Name = "ProfileForm";
            this.Text = "ProfileForm";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblWork;
        private System.Windows.Forms.Label lblEducation;
        private System.Windows.Forms.Label lblHometown;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblRelationship;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWork;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox txtHometown;
        private System.Windows.Forms.TextBox cboGender;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRelationship;
    }
}