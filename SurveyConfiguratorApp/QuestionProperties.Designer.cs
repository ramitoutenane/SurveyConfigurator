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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuestionProperties));
            this.titleLabel = new System.Windows.Forms.Label();
            this.questionLabel = new System.Windows.Forms.Label();
            this.questionTextBox = new System.Windows.Forms.TextBox();
            this.orderNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.orderLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.sliderGroupBox = new System.Windows.Forms.GroupBox();
            this.endValueGroupLabel = new System.Windows.Forms.Label();
            this.startValueLabel = new System.Windows.Forms.Label();
            this.startValueGroupLabel = new System.Windows.Forms.Label();
            this.startValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endValueLabel = new System.Windows.Forms.Label();
            this.endCaptionTextBox = new System.Windows.Forms.TextBox();
            this.endValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.startCaptionTextBox = new System.Windows.Forms.TextBox();
            this.startCaptionLabel = new System.Windows.Forms.Label();
            this.endCaptionLabel = new System.Windows.Forms.Label();
            this.smileyGroupBox = new System.Windows.Forms.GroupBox();
            this.smileyFacesLabel = new System.Windows.Forms.Label();
            this.smileyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.starsGroupBox = new System.Windows.Forms.GroupBox();
            this.starsLabel = new System.Windows.Forms.Label();
            this.starsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.maxCharLabel = new System.Windows.Forms.Label();
            this.currentCharCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.orderNumericUpDown)).BeginInit();
            this.sliderGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startValueNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endValueNumericUpDown)).BeginInit();
            this.smileyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smileyNumericUpDown)).BeginInit();
            this.starsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.starsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            resources.ApplyResources(this.titleLabel, "titleLabel");
            this.titleLabel.Name = "titleLabel";
            // 
            // questionLabel
            // 
            resources.ApplyResources(this.questionLabel, "questionLabel");
            this.questionLabel.Name = "questionLabel";
            // 
            // questionTextBox
            // 
            resources.ApplyResources(this.questionTextBox, "questionTextBox");
            this.questionTextBox.Name = "questionTextBox";
            this.questionTextBox.TextChanged += new System.EventHandler(this.questionTextBox_TextChanged);
            // 
            // orderNumericUpDown
            // 
            resources.ApplyResources(this.orderNumericUpDown, "orderNumericUpDown");
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
            this.orderNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.orderNumericUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.orderNumericUpDown_Validating);
            // 
            // orderLabel
            // 
            resources.ApplyResources(this.orderLabel, "orderLabel");
            this.orderLabel.Name = "orderLabel";
            // 
            // typeLabel
            // 
            resources.ApplyResources(this.typeLabel, "typeLabel");
            this.typeLabel.Name = "typeLabel";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.typeComboBox, "typeComboBox");
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // sliderGroupBox
            // 
            this.sliderGroupBox.Controls.Add(this.endValueGroupLabel);
            this.sliderGroupBox.Controls.Add(this.startValueLabel);
            this.sliderGroupBox.Controls.Add(this.startValueGroupLabel);
            this.sliderGroupBox.Controls.Add(this.startValueNumericUpDown);
            this.sliderGroupBox.Controls.Add(this.endValueLabel);
            this.sliderGroupBox.Controls.Add(this.endCaptionTextBox);
            this.sliderGroupBox.Controls.Add(this.endValueNumericUpDown);
            this.sliderGroupBox.Controls.Add(this.startCaptionTextBox);
            this.sliderGroupBox.Controls.Add(this.startCaptionLabel);
            this.sliderGroupBox.Controls.Add(this.endCaptionLabel);
            resources.ApplyResources(this.sliderGroupBox, "sliderGroupBox");
            this.sliderGroupBox.Name = "sliderGroupBox";
            this.sliderGroupBox.TabStop = false;
            // 
            // endValueGroupLabel
            // 
            resources.ApplyResources(this.endValueGroupLabel, "endValueGroupLabel");
            this.endValueGroupLabel.Name = "endValueGroupLabel";
            // 
            // startValueLabel
            // 
            resources.ApplyResources(this.startValueLabel, "startValueLabel");
            this.startValueLabel.Name = "startValueLabel";
            // 
            // startValueGroupLabel
            // 
            resources.ApplyResources(this.startValueGroupLabel, "startValueGroupLabel");
            this.startValueGroupLabel.Name = "startValueGroupLabel";
            // 
            // startValueNumericUpDown
            // 
            resources.ApplyResources(this.startValueNumericUpDown, "startValueNumericUpDown");
            this.startValueNumericUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.startValueNumericUpDown.Name = "startValueNumericUpDown";
            this.startValueNumericUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.startValueNumericUpDown_Validating);
            // 
            // endValueLabel
            // 
            resources.ApplyResources(this.endValueLabel, "endValueLabel");
            this.endValueLabel.Name = "endValueLabel";
            // 
            // endCaptionTextBox
            // 
            resources.ApplyResources(this.endCaptionTextBox, "endCaptionTextBox");
            this.endCaptionTextBox.Name = "endCaptionTextBox";
            // 
            // endValueNumericUpDown
            // 
            resources.ApplyResources(this.endValueNumericUpDown, "endValueNumericUpDown");
            this.endValueNumericUpDown.Name = "endValueNumericUpDown";
            this.endValueNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.endValueNumericUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.endValueNumericUpDown_Validating);
            // 
            // startCaptionTextBox
            // 
            resources.ApplyResources(this.startCaptionTextBox, "startCaptionTextBox");
            this.startCaptionTextBox.Name = "startCaptionTextBox";
            // 
            // startCaptionLabel
            // 
            resources.ApplyResources(this.startCaptionLabel, "startCaptionLabel");
            this.startCaptionLabel.Name = "startCaptionLabel";
            // 
            // endCaptionLabel
            // 
            resources.ApplyResources(this.endCaptionLabel, "endCaptionLabel");
            this.endCaptionLabel.Name = "endCaptionLabel";
            // 
            // smileyGroupBox
            // 
            this.smileyGroupBox.Controls.Add(this.smileyFacesLabel);
            this.smileyGroupBox.Controls.Add(this.smileyNumericUpDown);
            resources.ApplyResources(this.smileyGroupBox, "smileyGroupBox");
            this.smileyGroupBox.Name = "smileyGroupBox";
            this.smileyGroupBox.TabStop = false;
            // 
            // smileyFacesLabel
            // 
            resources.ApplyResources(this.smileyFacesLabel, "smileyFacesLabel");
            this.smileyFacesLabel.Name = "smileyFacesLabel";
            // 
            // smileyNumericUpDown
            // 
            resources.ApplyResources(this.smileyNumericUpDown, "smileyNumericUpDown");
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
            this.smileyNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.smileyNumericUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.smileyNumericUpDown_Validating);
            // 
            // starsGroupBox
            // 
            this.starsGroupBox.Controls.Add(this.starsLabel);
            this.starsGroupBox.Controls.Add(this.starsNumericUpDown);
            resources.ApplyResources(this.starsGroupBox, "starsGroupBox");
            this.starsGroupBox.Name = "starsGroupBox";
            this.starsGroupBox.TabStop = false;
            // 
            // starsLabel
            // 
            resources.ApplyResources(this.starsLabel, "starsLabel");
            this.starsLabel.Name = "starsLabel";
            // 
            // starsNumericUpDown
            // 
            resources.ApplyResources(this.starsNumericUpDown, "starsNumericUpDown");
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
            this.starsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.starsNumericUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.starsNumericUpDown_Validating);
            // 
            // cancelButton
            // 
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // maxCharLabel
            // 
            resources.ApplyResources(this.maxCharLabel, "maxCharLabel");
            this.maxCharLabel.Name = "maxCharLabel";
            // 
            // currentCharCount
            // 
            resources.ApplyResources(this.currentCharCount, "currentCharCount");
            this.currentCharCount.Name = "currentCharCount";
            // 
            // QuestionProperties
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuestionProperties";
            this.Load += new System.EventHandler(this.QuestionProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.orderNumericUpDown)).EndInit();
            this.sliderGroupBox.ResumeLayout(false);
            this.sliderGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startValueNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endValueNumericUpDown)).EndInit();
            this.smileyGroupBox.ResumeLayout(false);
            this.smileyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smileyNumericUpDown)).EndInit();
            this.starsGroupBox.ResumeLayout(false);
            this.starsGroupBox.PerformLayout();
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
        private System.Windows.Forms.TextBox startCaptionTextBox;
        private System.Windows.Forms.Label startCaptionLabel;
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
        private System.Windows.Forms.Label endValueGroupLabel;
        private System.Windows.Forms.Label startValueGroupLabel;
    }
}