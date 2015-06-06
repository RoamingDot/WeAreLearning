namespace jsonmaker
{
    partial class Navigation
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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataLabel = new System.Windows.Forms.Label();
            this.textLabel1 = new System.Windows.Forms.Label();
            this.FileLocation1 = new System.Windows.Forms.Label();
            this.dataButton = new System.Windows.Forms.Button();
            this.monsterBox = new System.Windows.Forms.ListBox();
            this.monsterListLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataLabel
            // 
            this.dataLabel.Location = new System.Drawing.Point(12, 143);
            this.dataLabel.MinimumSize = new System.Drawing.Size(270, 270);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(352, 270);
            this.dataLabel.TabIndex = 1;
            this.dataLabel.Text = "Preview";
            // 
            // textLabel1
            // 
            this.textLabel1.AutoSize = true;
            this.textLabel1.Location = new System.Drawing.Point(12, 23);
            this.textLabel1.Name = "textLabel1";
            this.textLabel1.Size = new System.Drawing.Size(136, 13);
            this.textLabel1.TabIndex = 2;
            this.textLabel1.Text = "Select Monster List to Load";
            // 
            // FileLocation1
            // 
            this.FileLocation1.AutoSize = true;
            this.FileLocation1.Location = new System.Drawing.Point(87, 44);
            this.FileLocation1.Name = "FileLocation1";
            this.FileLocation1.Size = new System.Drawing.Size(16, 13);
            this.FileLocation1.TabIndex = 3;
            this.FileLocation1.Text = "...";
            // 
            // dataButton
            // 
            this.dataButton.Location = new System.Drawing.Point(15, 117);
            this.dataButton.Name = "dataButton";
            this.dataButton.Size = new System.Drawing.Size(75, 23);
            this.dataButton.TabIndex = 4;
            this.dataButton.Text = "Display Data";
            this.dataButton.UseVisualStyleBackColor = true;
            this.dataButton.Click += new System.EventHandler(this.dataButton_Click);
            // 
            // monsterBox
            // 
            this.monsterBox.FormattingEnabled = true;
            this.monsterBox.Location = new System.Drawing.Point(556, 138);
            this.monsterBox.Name = "monsterBox";
            this.monsterBox.Size = new System.Drawing.Size(146, 95);
            this.monsterBox.TabIndex = 5;
            // 
            // monsterListLable
            // 
            this.monsterListLable.AutoSize = true;
            this.monsterListLable.Location = new System.Drawing.Point(553, 122);
            this.monsterListLable.Name = "monsterListLable";
            this.monsterListLable.Size = new System.Drawing.Size(103, 13);
            this.monsterListLable.TabIndex = 6;
            this.monsterListLable.Text = "List of each Monster";
            // 
            // Navigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 444);
            this.Controls.Add(this.monsterListLable);
            this.Controls.Add(this.monsterBox);
            this.Controls.Add(this.dataButton);
            this.Controls.Add(this.FileLocation1);
            this.Controls.Add(this.textLabel1);
            this.Controls.Add(this.dataLabel);
            this.Controls.Add(this.button1);
            this.Name = "Navigation";
            this.Text = "Navigation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.Label textLabel1;
        private System.Windows.Forms.Label FileLocation1;
        private System.Windows.Forms.Button dataButton;
        private System.Windows.Forms.ListBox monsterBox;
        private System.Windows.Forms.Label monsterListLable;
    }
}

