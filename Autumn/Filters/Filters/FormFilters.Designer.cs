namespace Filters
{
    partial class FormFilters
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.Load_Button = new System.Windows.Forms.Button();
            this.Filter_Button = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.FiltersList = new System.Windows.Forms.ComboBox();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(22, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(560, 343);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // Load_Button
            // 
            this.Load_Button.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Load_Button.Location = new System.Drawing.Point(22, 417);
            this.Load_Button.Name = "Load_Button";
            this.Load_Button.Size = new System.Drawing.Size(204, 57);
            this.Load_Button.TabIndex = 1;
            this.Load_Button.Text = "Загрузить изображение";
            this.Load_Button.UseVisualStyleBackColor = true;
            this.Load_Button.Click += new System.EventHandler(this.Load_Button_Click);
            // 
            // Filter_Button
            // 
            this.Filter_Button.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Filter_Button.Location = new System.Drawing.Point(600, 412);
            this.Filter_Button.Name = "Filter_Button";
            this.Filter_Button.Size = new System.Drawing.Size(204, 62);
            this.Filter_Button.TabIndex = 1;
            this.Filter_Button.Text = "Применить фильтр";
            this.Filter_Button.UseVisualStyleBackColor = true;
            this.Filter_Button.Click += new System.EventHandler(this.Filter_Button_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(232, 428);
            this.progressBar1.Maximum = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(362, 31);
            this.progressBar1.TabIndex = 2;
            // 
            // FiltersList
            // 
            this.FiltersList.FormattingEnabled = true;
            this.FiltersList.Items.AddRange(new object[] {
            "Размытие 3x3",
            "Размытие 5x5",
            "Фильтр Собеля по X",
            "Фильтр Собеля по Y",
            "Оттенки серого",
            "Размытие по Гауссу"});
            this.FiltersList.Location = new System.Drawing.Point(632, 33);
            this.FiltersList.Name = "FiltersList";
            this.FiltersList.Size = new System.Drawing.Size(186, 21);
            this.FiltersList.TabIndex = 3;
            this.FiltersList.Text = "Выберите фильтр";
            // 
            // FileDialog
            // 
            this.FileDialog.FileName = "openFileDialog1";
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SaveButton.Location = new System.Drawing.Point(600, 344);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(204, 62);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Cохранить картинку";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // FormFilters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 512);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.FiltersList);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Filter_Button);
            this.Controls.Add(this.Load_Button);
            this.Controls.Add(this.pictureBox);
            this.Name = "FormFilters";
            this.Text = "Графические фильтры";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button Load_Button;
        private System.Windows.Forms.Button Filter_Button;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox FiltersList;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.Button SaveButton;
    }
}

