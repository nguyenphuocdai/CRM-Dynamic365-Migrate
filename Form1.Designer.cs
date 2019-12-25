namespace CRM
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSyncAccount = new System.Windows.Forms.Button();
            this.btnClick_SyncAllEntities = new System.Windows.Forms.Button();
            this.btnLoginAD = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(171, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "Sync Contacts";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSyncContacts_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 78);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(171, 34);
            this.button2.TabIndex = 0;
            this.button2.Text = "Sync Sale Contacts";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnSyncSaleContacts_Click);
            // 
            // btnSyncAccount
            // 
            this.btnSyncAccount.Location = new System.Drawing.Point(20, 139);
            this.btnSyncAccount.Name = "btnSyncAccount";
            this.btnSyncAccount.Size = new System.Drawing.Size(171, 34);
            this.btnSyncAccount.TabIndex = 0;
            this.btnSyncAccount.Text = "Sync Accounts";
            this.btnSyncAccount.UseVisualStyleBackColor = true;
            this.btnSyncAccount.Click += new System.EventHandler(this.btnSyncAccount_Click);
            // 
            // btnClick_SyncAllEntities
            // 
            this.btnClick_SyncAllEntities.Location = new System.Drawing.Point(20, 200);
            this.btnClick_SyncAllEntities.Name = "btnClick_SyncAllEntities";
            this.btnClick_SyncAllEntities.Size = new System.Drawing.Size(171, 37);
            this.btnClick_SyncAllEntities.TabIndex = 1;
            this.btnClick_SyncAllEntities.Text = "Sync All Entities";
            this.btnClick_SyncAllEntities.UseVisualStyleBackColor = true;
            this.btnClick_SyncAllEntities.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnLoginAD
            // 
            this.btnLoginAD.Location = new System.Drawing.Point(343, 139);
            this.btnLoginAD.Name = "btnLoginAD";
            this.btnLoginAD.Size = new System.Drawing.Size(138, 34);
            this.btnLoginAD.TabIndex = 2;
            this.btnLoginAD.Text = "Login AD";
            this.btnLoginAD.UseVisualStyleBackColor = true;
            this.btnLoginAD.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(355, 48);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 23);
            this.txtUsername.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(355, 85);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 23);
            this.txtPassword.TabIndex = 4;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(284, 56);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(284, 93);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(530, 285);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnLoginAD);
            this.Controls.Add(this.btnClick_SyncAllEntities);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSyncAccount);
            this.Controls.Add(this.lblPassword);
            this.Name = "Form1";

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSyncAccount;
        private System.Windows.Forms.Button btnClick_SyncAllEntities;
        private System.Windows.Forms.Button btnLoginAD;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
    }
}

