namespace WinFormsFilters
{
    partial class MainForm
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
            this.OpenButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.SwitchFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.MainImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MainImage)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(690, 15);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(140, 30);
            this.OpenButton.TabIndex = 0;
            this.OpenButton.Text = "Открыть файл";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(690, 158);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(140, 30);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Обработать";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // SwitchFilter
            // 
            this.SwitchFilter.FormattingEnabled = true;
            this.SwitchFilter.Items.AddRange(new object[] {
            "Mean 3*3",
            "Mean 5*5",
            "Gauss",
            "Grayscale",
            "Sobel on X",
            "Sobel on Y"});
            this.SwitchFilter.Location = new System.Drawing.Point(690, 87);
            this.SwitchFilter.Name = "SwitchFilter";
            this.SwitchFilter.Size = new System.Drawing.Size(140, 24);
            this.SwitchFilter.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(687, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Фильтр";
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(12, 471);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(818, 30);
            this.Progress.TabIndex = 4;
            // 
            // MainImage
            // 
            this.MainImage.Location = new System.Drawing.Point(12, 15);
            this.MainImage.Name = "MainImage";
            this.MainImage.Size = new System.Drawing.Size(669, 450);
            this.MainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MainImage.TabIndex = 5;
            this.MainImage.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 513);
            this.Controls.Add(this.MainImage);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SwitchFilter);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.OpenButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Графические фильтры";
            ((System.ComponentModel.ISupportInitialize)(this.MainImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.ComboBox SwitchFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.PictureBox MainImage;
    }
}

