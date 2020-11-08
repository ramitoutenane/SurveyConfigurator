namespace SurveyConfiguratorApp
{
    partial class QuestionProperties
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.questionLabel = new System.Windows.Forms.Label();
            this.questionTextBox = new System.Windows.Forms.TextBox();
            this.orderNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.orderLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.sliderGroupBox = new System.Windows.Forms.GroupBox();
            this.smileyGroupBox = new System.Windows.Forms.GroupBox();
            this.starsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.orderNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(434, 54);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Question Properties";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // questionLabel
            // 
            this.questionLabel.AutoSize = true;
            this.questionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionLabel.Location = new System.Drawing.Point(5, 59);
            this.questionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(67, 16);
            this.questionLabel.TabIndex = 2;
            this.questionLabel.Text = "Question :";
            // 
            // questionTextBox
            // 
            this.questionTextBox.Location = new System.Drawing.Point(8, 78);
            this.questionTextBox.MaxLength = 100;
            this.questionTextBox.Multiline = true;
            this.questionTextBox.Name = "questionTextBox";
            this.questionTextBox.Size = new System.Drawing.Size(414, 48);
            this.questionTextBox.TabIndex = 3;
            // 
            // orderNumericUpDown
            // 
            this.orderNumericUpDown.Location = new System.Drawing.Point(57, 146);
            this.orderNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.orderNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.orderNumericUpDown.Name = "orderNumericUpDown";
            this.orderNumericUpDown.Size = new System.Drawing.Size(79, 25);
            this.orderNumericUpDown.TabIndex = 4;
            this.orderNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // orderLabel
            // 
            this.orderLabel.AutoSize = true;
            this.orderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLabel.Location = new System.Drawing.Point(5, 148);
            this.orderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.orderLabel.Name = "orderLabel";
            this.orderLabel.Size = new System.Drawing.Size(48, 16);
            this.orderLabel.TabIndex = 5;
            this.orderLabel.Text = "Order :";
            this.orderLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeLabel.Location = new System.Drawing.Point(238, 148);
            this.typeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(46, 16);
            this.typeLabel.TabIndex = 6;
            this.typeLabel.Text = "Type :";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Location = new System.Drawing.Point(308, 145);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(114, 25);
            this.typeComboBox.TabIndex = 7;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // sliderGroupBox
            // 
            this.sliderGroupBox.Location = new System.Drawing.Point(8, 225);
            this.sliderGroupBox.Name = "sliderGroupBox";
            this.sliderGroupBox.Size = new System.Drawing.Size(414, 79);
            this.sliderGroupBox.TabIndex = 8;
            this.sliderGroupBox.TabStop = false;
            this.sliderGroupBox.Text = "Slider Qusetion";
            this.sliderGroupBox.Visible = false;
            // 
            // smileyGroupBox
            // 
            this.smileyGroupBox.Location = new System.Drawing.Point(428, 436);
            this.smileyGroupBox.Name = "smileyGroupBox";
            this.smileyGroupBox.Size = new System.Drawing.Size(414, 186);
            this.smileyGroupBox.TabIndex = 9;
            this.smileyGroupBox.TabStop = false;
            this.smileyGroupBox.Text = "Smiley Qusetion";
            this.smileyGroupBox.Visible = false;
            // 
            // starsGroupBox
            // 
            this.starsGroupBox.Location = new System.Drawing.Point(428, 235);
            this.starsGroupBox.Name = "starsGroupBox";
            this.starsGroupBox.Size = new System.Drawing.Size(414, 186);
            this.starsGroupBox.TabIndex = 10;
            this.starsGroupBox.TabStop = false;
            this.starsGroupBox.Text = "Stars Qusetion";
            this.starsGroupBox.Visible = false;
            // 
            // QuestionProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 511);
            this.Controls.Add(this.starsGroupBox);
            this.Controls.Add(this.smileyGroupBox);
            this.Controls.Add(this.sliderGroupBox);
            this.Controls.Add(this.typeComboBox);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.orderLabel);
            this.Controls.Add(this.orderNumericUpDown);
            this.Controls.Add(this.questionTextBox);
            this.Controls.Add(this.questionLabel);
            this.Controls.Add(this.titleLabel);
            this.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(450, 550);
            this.MinimumSize = new System.Drawing.Size(450, 550);
            this.Name = "QuestionProperties";
            this.Text = "QuestionProperties";
            this.Load += new System.EventHandler(this.QuestionProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.orderNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.TextBox questionTextBox;
        private System.Windows.Forms.NumericUpDown orderNumericUpDown;
        private System.Windows.Forms.Label orderLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.GroupBox sliderGroupBox;
        private System.Windows.Forms.GroupBox smileyGroupBox;
        private System.Windows.Forms.GroupBox starsGroupBox;
    }
}