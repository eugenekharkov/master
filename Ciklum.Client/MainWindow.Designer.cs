namespace Ciklum.Client
{
    partial class MainWindow
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstEncryptedFiles = new System.Windows.Forms.ListBox();
            this.lblEnqryptedFiles = new System.Windows.Forms.Label();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtUrlAdress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lstEncryptedFiles
            // 
            this.lstEncryptedFiles.AllowDrop = true;
            this.lstEncryptedFiles.BackColor = System.Drawing.Color.White;
            this.lstEncryptedFiles.FormattingEnabled = true;
            this.lstEncryptedFiles.Location = new System.Drawing.Point(12, 28);
            this.lstEncryptedFiles.Name = "lstEncryptedFiles";
            this.lstEncryptedFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstEncryptedFiles.Size = new System.Drawing.Size(560, 290);
            this.lstEncryptedFiles.TabIndex = 0;
            // 
            // lblEnqryptedFiles
            // 
            this.lblEnqryptedFiles.AutoSize = true;
            this.lblEnqryptedFiles.Location = new System.Drawing.Point(13, 9);
            this.lblEnqryptedFiles.Name = "lblEnqryptedFiles";
            this.lblEnqryptedFiles.Size = new System.Drawing.Size(79, 13);
            this.lblEnqryptedFiles.TabIndex = 1;
            this.lblEnqryptedFiles.Text = "Encrypted Files";
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(497, 324);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(16, 329);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(32, 13);
            this.lblUrl.TabIndex = 3;
            this.lblUrl.Text = "URL:";
            // 
            // txtUrlAdress
            // 
            this.txtUrlAdress.Location = new System.Drawing.Point(51, 326);
            this.txtUrlAdress.Name = "txtUrlAdress";
            this.txtUrlAdress.Size = new System.Drawing.Size(366, 20);
            this.txtUrlAdress.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.txtUrlAdress);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.lblEnqryptedFiles);
            this.Controls.Add(this.lstEncryptedFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Ciklum.Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstEncryptedFiles;
        private System.Windows.Forms.Label lblEnqryptedFiles;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrlAdress;
    }
}

