namespace P2PChatWinForms
{
    partial class ChatForm
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
            this.AddressTextBox = new System.Windows.Forms.TextBox();
            this.NickTextBox = new System.Windows.Forms.TextBox();
            this.ChatButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ChatTextBox = new System.Windows.Forms.RichTextBox();
            this.MessageTextBox = new System.Windows.Forms.RichTextBox();
            this.ForkButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddressTextBox
            // 
            this.AddressTextBox.Location = new System.Drawing.Point(47, 12);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.Size = new System.Drawing.Size(242, 20);
            this.AddressTextBox.TabIndex = 0;
            // 
            // NickTextBox
            // 
            this.NickTextBox.Location = new System.Drawing.Point(345, 12);
            this.NickTextBox.Name = "NickTextBox";
            this.NickTextBox.Size = new System.Drawing.Size(242, 20);
            this.NickTextBox.TabIndex = 2;
            // 
            // ChatButton
            // 
            this.ChatButton.Location = new System.Drawing.Point(607, 10);
            this.ChatButton.Name = "ChatButton";
            this.ChatButton.Size = new System.Drawing.Size(75, 23);
            this.ChatButton.TabIndex = 4;
            this.ChatButton.Text = "Host";
            this.ChatButton.UseVisualStyleBackColor = true;
            this.ChatButton.Click += new System.EventHandler(this.ChatButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(764, 279);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 71);
            this.SendButton.TabIndex = 5;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(307, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Nick:";
            // 
            // ChatTextBox
            // 
            this.ChatTextBox.Location = new System.Drawing.Point(12, 39);
            this.ChatTextBox.Name = "ChatTextBox";
            this.ChatTextBox.ReadOnly = true;
            this.ChatTextBox.Size = new System.Drawing.Size(827, 235);
            this.ChatTextBox.TabIndex = 8;
            this.ChatTextBox.Text = "";
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.Location = new System.Drawing.Point(12, 280);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(730, 71);
            this.MessageTextBox.TabIndex = 9;
            this.MessageTextBox.Text = "Create host to enter the chat...";
            // 
            // ForkButton
            // 
            this.ForkButton.Location = new System.Drawing.Point(764, 10);
            this.ForkButton.Name = "ForkButton";
            this.ForkButton.Size = new System.Drawing.Size(75, 23);
            this.ForkButton.TabIndex = 10;
            this.ForkButton.Text = "Fork!";
            this.ForkButton.UseVisualStyleBackColor = true;
            this.ForkButton.Click += new System.EventHandler(this.ForkButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 362);
            this.Controls.Add(this.ForkButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.ChatTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.ChatButton);
            this.Controls.Add(this.NickTextBox);
            this.Controls.Add(this.AddressTextBox);
            this.Name = "Form1";
            this.Text = "The Best Chat v 1.0";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.TextBox NickTextBox;
        private System.Windows.Forms.Button ChatButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox ChatTextBox;
        private System.Windows.Forms.RichTextBox MessageTextBox;
        private System.Windows.Forms.Button ForkButton;
    }
}

