namespace WFFilter
{
    partial class Filter
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
            this.loadbutton = new System.Windows.Forms.Button();
            this.filtbutton = new System.Windows.Forms.Button();
            this.picturesbox = new System.Windows.Forms.PictureBox();
            this.progressbar = new System.Windows.Forms.ProgressBar();
            this.choiceofimage = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picturesbox)).BeginInit();
            this.SuspendLayout();
            // 
            // loadbutton
            // 
            this.loadbutton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.loadbutton.Location = new System.Drawing.Point(102, 295);
            this.loadbutton.Name = "loadbutton";
            this.loadbutton.Size = new System.Drawing.Size(141, 23);
            this.loadbutton.TabIndex = 1;
            this.loadbutton.Text = "Загрузить изображение";
            this.loadbutton.UseVisualStyleBackColor = true;
            this.loadbutton.Click += new System.EventHandler(this.loadbutton_Click);
            // 
            // filtbutton
            // 
            this.filtbutton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.filtbutton.Location = new System.Drawing.Point(249, 295);
            this.filtbutton.Name = "filtbutton";
            this.filtbutton.Size = new System.Drawing.Size(137, 23);
            this.filtbutton.TabIndex = 2;
            this.filtbutton.Text = "Применить  фильтр";
            this.filtbutton.UseVisualStyleBackColor = true;
            this.filtbutton.Click += new System.EventHandler(this.filtbutton_Click);
            // 
            // picturesbox
            // 
            this.picturesbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.picturesbox.Location = new System.Drawing.Point(102, 12);
            this.picturesbox.Name = "picturesbox";
            this.picturesbox.Size = new System.Drawing.Size(603, 277);
            this.picturesbox.TabIndex = 5;
            this.picturesbox.TabStop = false;
            // 
            // progressbar
            // 
            this.progressbar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressbar.Location = new System.Drawing.Point(392, 295);
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(313, 23);
            this.progressbar.TabIndex = 6;
            // 
            // choiceofimage
            // 
            this.choiceofimage.FileName = "openFileDialog1";
            // 
            // Filter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 368);
            this.Controls.Add(this.progressbar);
            this.Controls.Add(this.picturesbox);
            this.Controls.Add(this.filtbutton);
            this.Controls.Add(this.loadbutton);
            this.Name = "Filter";
            this.Text = "Filter";
            ((System.ComponentModel.ISupportInitialize)(this.picturesbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadbutton;
        private System.Windows.Forms.Button filtbutton;
        private System.Windows.Forms.PictureBox picturesbox;
        private System.Windows.Forms.ProgressBar progressbar;
        private System.Windows.Forms.OpenFileDialog choiceofimage;
    }
}

