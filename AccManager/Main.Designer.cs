
namespace AccManager
{
    partial class MainFrm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.clbAccounts = new System.Windows.Forms.CheckedListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clbAccounts
            // 
            this.clbAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbAccounts.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.clbAccounts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbAccounts.ForeColor = System.Drawing.Color.Lime;
            this.clbAccounts.FormattingEnabled = true;
            this.clbAccounts.Location = new System.Drawing.Point(0, 0);
            this.clbAccounts.Name = "clbAccounts";
            this.clbAccounts.Size = new System.Drawing.Size(256, 150);
            this.clbAccounts.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.ForeColor = System.Drawing.Color.Lime;
            this.btnDelete.Location = new System.Drawing.Point(0, 150);
            this.btnDelete.MinimumSize = new System.Drawing.Size(256, 32);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(256, 32);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(256, 181);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.clbAccounts);
            this.MinimumSize = new System.Drawing.Size(272, 220);
            this.Name = "MainFrm";
            this.Text = "[loadsasuke]";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbAccounts;
        private System.Windows.Forms.Button btnDelete;
    }
}

