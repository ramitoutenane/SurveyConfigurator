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
            this.endValueGroupBox = new System.Windows.Forms.GroupBox();
            this.endValueLabel = new System.Windows.Forms.Label();
            this.endValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endCaptionTextBox = new System.Windows.Forms.TextBox();
            this.endCaptionLabel = new System.Windows.Forms.Label();
            this.startValueGroupBox = new System.Windows.Forms.GroupBox();
            this.startValueLabel = new System.Windows.Forms.Label();
            this.startValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.startCaptionTextBox = new System.Windows.Forms.TextBox();
            this.startCaptionLabel = new System.Windows.Forms.Label();
            this.smileyGroupBox = new System.Windows.Forms.GroupBox();
            this.starsGroupBox = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.maxCharLabel = new System.Windows.Forms.Label();
            this.currentCharCount = new System.Windows.Forms.Label();
            this.smileyFacesLabel = new System.Windows.Forms.Label();
            this.smileyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.starsLabel = new System.Windows.Forms.Label();
            this.starsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.orderNumericUpDown)).BeginInit();
            this.sliderGroupBox.SuspendLayout();
            this.endValueGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endValueNumericUpDown)).BeginInit();
            this.startValueGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startValueNumericUpDown)).BeginInit();
            this.smileyGroupBox.SuspendLayout();
            this.starsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smileyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.starsNumericUpDown)).BeginInit();
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
            this.questionTextBox.TextChanged += new System.EventHandler(this.questionTextBox_TextChanged);
            // 
            // orderNumericUpDown
            // 
            this.orderNumericUpDown.Location = new System.Drawing.Point(60, 179);
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
            this.orderLabel.Location = new System.Drawing.Point(5, 182);
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
            this.typeLabel.Location = new System.Drawing.Point(240, 182);
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
            this.typeComboBox.Location = new System.Drawing.Point(308, 179);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(114, 25);
            this.typeComboBox.TabIndex = 7;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // sliderGroupBox
            // 
            this.sliderGroupBox.Controls.Add(this.endValueGroupBox);
            this.sliderGroupBox.Controls.Add(this.startValueGroupBox);
            this.sliderGroupBox.Location = new System.Drawing.Point(8, 230);
            this.sliderGroupBox.Name = "sliderGroupBox";
            this.sliderGroupBox.Size = new System.Drawing.Size(415, 168);
            this.sliderGroupBox.TabIndex = 8;
            this.sliderGroupBox.TabStop = false;
            this.sliderGroupBox.Text = "Slider Qusetion";
            this.sliderGroupBox.Visible = false;
            // 
            // endValueGroupBox
            // 
            this.endValueGroupBox.Controls.Add(this.endValueLabel);
            this.endValueGroupBox.Controls.Add(this.endValueNumericUpDown);
            this.endValueGroupBox.Controls.Add(this.endCaptionTextBox);
            this.endValueGroupBox.Controls.Add(this.endCaptionLabel);
            this.endValueGroupBox.Location = new System.Drawing.Point(6, 92);
            this.endValueGroupBox.Name = "endValueGroupBox";
            this.endValueGroupBox.Size = new System.Drawing.Size(402, 62);
            this.endValueGroupBox.TabIndex = 12;
            this.endValueGroupBox.TabStop = false;
            this.endValueGroupBox.Text = "End Value";
            // 
            // endValueLabel
            // 
            this.endValueLabel.AutoSize = true;
            this.endValueLabel.Location = new System.Drawing.Point(274, 28);
            this.endValueLabel.Name = "endValueLabel";
            this.endValueLabel.Size = new System.Drawing.Size(50, 17);
            this.endValueLabel.TabIndex = 3;
            this.endValueLabel.Text = "value : ";
            // 
            // endValueNumericUpDown
            // 
            this.endValueNumericUpDown.Location = new System.Drawing.Point(330, 24);
            this.endValueNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.endValueNumericUpDown.Name = "endValueNumericUpDown";
            this.endValueNumericUpDown.Size = new System.Drawing.Size(66, 25);
            this.endValueNumericUpDown.TabIndex = 2;
            this.endValueNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.endValueNumericUpDown.ValueChanged += new System.EventHandler(this.endValueNumericUpDown_ValueChanged);
            // 
            // endCaptionTextBox
            // 
            this.endCaptionTextBox.Location = new System.Drawing.Point(69, 25);
            this.endCaptionTextBox.MaxLength = 25;
            this.endCaptionTextBox.Name = "endCaptionTextBox";
            this.endCaptionTextBox.Size = new System.Drawing.Size(187, 25);
            this.endCaptionTextBox.TabIndex = 1;
            // 
            // endCaptionLabel
            // 
            this.endCaptionLabel.AutoSize = true;
            this.endCaptionLabel.Location = new System.Drawing.Point(6, 28);
            this.endCaptionLabel.Name = "endCaptionLabel";
            this.endCaptionLabel.Size = new System.Drawing.Size(64, 17);
            this.endCaptionLabel.TabIndex = 0;
            this.endCaptionLabel.Text = "Caption : ";
            // 
            // startValueGroupBox
            // 
            this.startValueGroupBox.Controls.Add(this.startValueLabel);
            this.startValueGroupBox.Controls.Add(this.startValueNumericUpDown);
            this.startValueGroupBox.Controls.Add(this.startCaptionTextBox);
            this.startValueGroupBox.Controls.Add(this.startCaptionLabel);
            this.startValueGroupBox.Location = new System.Drawing.Point(6, 24);
            this.startValueGroupBox.Name = "startValueGroupBox";
            this.startValueGroupBox.Size = new System.Drawing.Size(402, 62);
            this.startValueGroupBox.TabIndex = 11;
            this.startValueGroupBox.TabStop = false;
            this.startValueGroupBox.Text = "Start Value";
            // 
            // startValueLabel
            // 
            this.startValueLabel.AutoSize = true;
            this.startValueLabel.Location = new System.Drawing.Point(274, 28);
            this.startValueLabel.Name = "startValueLabel";
            this.startValueLabel.Size = new System.Drawing.Size(50, 17);
            this.startValueLabel.TabIndex = 3;
            this.startValueLabel.Text = "value : ";
            // 
            // startValueNumericUpDown
            // 
            this.startValueNumericUpDown.Location = new System.Drawing.Point(330, 24);
            this.startValueNumericUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.startValueNumericUpDown.Name = "startValueNumericUpDown";
            this.startValueNumericUpDown.Size = new System.Drawing.Size(66, 25);
            this.startValueNumericUpDown.TabIndex = 2;
            this.startValueNumericUpDown.ValueChanged += new System.EventHandler(this.startValueNumericUpDown_ValueChanged);
            // 
            // startCaptionTextBox
            // 
            this.startCaptionTextBox.Location = new System.Drawing.Point(69, 25);
            this.startCaptionTextBox.MaxLength = 25;
            this.startCaptionTextBox.Name = "startCaptionTextBox";
            this.startCaptionTextBox.Size = new System.Drawing.Size(187, 25);
            this.startCaptionTextBox.TabIndex = 1;
            this.startCaptionTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // startCaptionLabel
            // 
            this.startCaptionLabel.AutoSize = true;
            this.startCaptionLabel.Location = new System.Drawing.Point(6, 28);
            this.startCaptionLabel.Name = "startCaptionLabel";
            this.startCaptionLabel.Size = new System.Drawing.Size(64, 17);
            this.startCaptionLabel.TabIndex = 0;
            this.startCaptionLabel.Text = "Caption : ";
            // 
            // smileyGroupBox
            // 
            this.smileyGroupBox.Controls.Add(this.smileyFacesLabel);
            this.smileyGroupBox.Controls.Add(this.smileyNumericUpDown);
            this.smileyGroupBox.Location = new System.Drawing.Point(439, 38);
            this.smileyGroupBox.Name = "smileyGroupBox";
            this.smileyGroupBox.Size = new System.Drawing.Size(415, 125);
            this.smileyGroupBox.TabIndex = 9;
            this.smileyGroupBox.TabStop = false;
            this.smileyGroupBox.Text = "Smiley Faces Qusetion";
            this.smileyGroupBox.Visible = false;
            // 
            // starsGroupBox
            // 
            this.starsGroupBox.Controls.Add(this.starsLabel);
            this.starsGroupBox.Controls.Add(this.starsNumericUpDown);
            this.starsGroupBox.Location = new System.Drawing.Point(439, 230);
            this.starsGroupBox.Name = "starsGroupBox";
            this.starsGroupBox.Size = new System.Drawing.Size(415, 125);
            this.starsGroupBox.TabIndex = 10;
            this.starsGroupBox.TabStop = false;
            this.starsGroupBox.Text = "Stars Qusetion";
            this.starsGroupBox.Visible = false;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(347, 458);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(263, 458);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 12;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // maxCharLabel
            // 
            this.maxCharLabel.AutoSize = true;
            this.maxCharLabel.Location = new System.Drawing.Point(380, 129);
            this.maxCharLabel.Name = "maxCharLabel";
            this.maxCharLabel.Size = new System.Drawing.Size(33, 17);
            this.maxCharLabel.TabIndex = 13;
            this.maxCharLabel.Text = "/100";
            // 
            // currentCharCount
            // 
            this.currentCharCount.AutoSize = true;
            this.currentCharCount.Location = new System.Drawing.Point(354, 129);
            this.currentCharCount.Name = "currentCharCount";
            this.currentCharCount.Size = new System.Drawing.Size(15, 17);
            this.currentCharCount.TabIndex = 14;
            this.currentCharCount.Text = "0";
            // 
            // smileyFacesLabel
            // 
            this.smileyFacesLabel.AutoSize = true;
            this.smileyFacesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.smileyFacesLabel.Location = new System.Drawing.Point(22, 59);
            this.smileyFacesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.smileyFacesLabel.Name = "smileyFacesLabel";
            this.smileyFacesLabel.Size = new System.Drawing.Size(161, 16);
            this.smileyFacesLabel.TabIndex = 16;
            this.smileyFacesLabel.Text = "Number of Smiley Faces :";
            // 
            // smileyNumericUpDown
            // 
            this.smileyNumericUpDown.Location = new System.Drawing.Point(201, 57);
            this.smileyNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.smileyNumericUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.smileyNumericUpDown.Name = "smileyNumericUpDown";
            this.smileyNumericUpDown.Size = new System.Drawing.Size(79, 25);
            this.smileyNumericUpDown.TabIndex = 15;
            this.smileyNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // starsLabel
            // 
            this.starsLabel.AutoSize = true;
            this.starsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starsLabel.Location = new System.Drawing.Point(22, 57);
            this.starsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.starsLabel.Name = "starsLabel";
            this.starsLabel.Size = new System.Drawing.Size(161, 16);
            this.starsLabel.TabIndex = 18;
            this.starsLabel.Text = "Number of Smiley Faces :";
            // 
            // starsNumericUpDown
            // 
            this.starsNumericUpDown.Location = new System.Drawing.Point(201, 55);
            this.starsNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.starsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.starsNumericUpDown.Name = "starsNumericUpDown";
            this.starsNumericUpDown.Size = new System.Drawing.Size(79, 25);
            this.starsNumericUpDown.TabIndex = 17;
            this.starsNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // QuestionProperties
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(434, 486);
            this.Controls.Add(this.currentCharCount);
            this.Controls.Add(this.maxCharLabel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
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
            this.MaximumSize = new System.Drawing.Size(1000, 550);
            this.MinimumSize = new System.Drawing.Size(450, 450);
            this.Name = "QuestionProperties";
            this.Text = "QuestionProperties";
            this.Load += new System.EventHandler(this.QuestionProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.orderNumericUpDown)).EndInit();
            this.sliderGroupBox.ResumeLayout(false);
            this.endValueGroupBox.ResumeLayout(false);
            this.endValueGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endValueNumericUpDown)).EndInit();
            this.startValueGroupBox.ResumeLayout(false);
            this.startValueGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startValueNumericUpDown)).EndInit();
            this.smileyGroupBox.ResumeLayout(false);
            this.smileyGroupBox.PerformLayout();
            this.starsGroupBox.ResumeLayout(false);
            this.starsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smileyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.starsNumericUpDown)).EndInit();
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
        private System.Windows.Forms.GroupBox startValueGroupBox;
        private System.Windows.Forms.TextBox startCaptionTextBox;
        private System.Windows.Forms.Label startCaptionLabel;
        private System.Windows.Forms.GroupBox endValueGroupBox;
        private System.Windows.Forms.Label endValueLabel;
        private System.Windows.Forms.NumericUpDown endValueNumericUpDown;
        private System.Windows.Forms.TextBox endCaptionTextBox;
        private System.Windows.Forms.Label endCaptionLabel;
        private System.Windows.Forms.Label startValueLabel;
        private System.Windows.Forms.NumericUpDown startValueNumericUpDown;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label maxCharLabel;
        private System.Windows.Forms.Label currentCharCount;
        private System.Windows.Forms.Label smileyFacesLabel;
        private System.Windows.Forms.NumericUpDown smileyNumericUpDown;
        private System.Windows.Forms.Label starsLabel;
        private System.Windows.Forms.NumericUpDown starsNumericUpDown;
    }
}