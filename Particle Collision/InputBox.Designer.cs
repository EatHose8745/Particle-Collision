namespace Particle_Collision
{
    partial class InputBox
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
            this.Value1Label = new System.Windows.Forms.Label();
            this.Value1Textbox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.Value2TextBox = new System.Windows.Forms.TextBox();
            this.Value2Label = new System.Windows.Forms.Label();
            this.InfectiousCheck = new System.Windows.Forms.CheckBox();
            this.Value3Label = new System.Windows.Forms.Label();
            this.Value3Textbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Value1Label
            // 
            this.Value1Label.AutoSize = true;
            this.Value1Label.Location = new System.Drawing.Point(12, 9);
            this.Value1Label.Name = "Value1Label";
            this.Value1Label.Size = new System.Drawing.Size(0, 13);
            this.Value1Label.TabIndex = 0;
            // 
            // Value1Textbox
            // 
            this.Value1Textbox.Location = new System.Drawing.Point(12, 25);
            this.Value1Textbox.Name = "Value1Textbox";
            this.Value1Textbox.Size = new System.Drawing.Size(100, 20);
            this.Value1Textbox.TabIndex = 1;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(197, 112);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // Value2TextBox
            // 
            this.Value2TextBox.Location = new System.Drawing.Point(12, 64);
            this.Value2TextBox.Name = "Value2TextBox";
            this.Value2TextBox.Size = new System.Drawing.Size(100, 20);
            this.Value2TextBox.TabIndex = 3;
            // 
            // Value2Label
            // 
            this.Value2Label.AutoSize = true;
            this.Value2Label.Location = new System.Drawing.Point(12, 48);
            this.Value2Label.Name = "Value2Label";
            this.Value2Label.Size = new System.Drawing.Size(0, 13);
            this.Value2Label.TabIndex = 4;
            // 
            // InfectiousCheck
            // 
            this.InfectiousCheck.AutoSize = true;
            this.InfectiousCheck.Location = new System.Drawing.Point(118, 27);
            this.InfectiousCheck.Name = "InfectiousCheck";
            this.InfectiousCheck.Size = new System.Drawing.Size(78, 17);
            this.InfectiousCheck.TabIndex = 5;
            this.InfectiousCheck.Text = "Infectious?";
            this.InfectiousCheck.UseVisualStyleBackColor = true;
            // 
            // Value3Label
            // 
            this.Value3Label.AutoSize = true;
            this.Value3Label.Location = new System.Drawing.Point(12, 87);
            this.Value3Label.Name = "Value3Label";
            this.Value3Label.Size = new System.Drawing.Size(0, 13);
            this.Value3Label.TabIndex = 7;
            // 
            // Value3Textbox
            // 
            this.Value3Textbox.Location = new System.Drawing.Point(12, 103);
            this.Value3Textbox.Name = "Value3Textbox";
            this.Value3Textbox.Size = new System.Drawing.Size(100, 20);
            this.Value3Textbox.TabIndex = 6;
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 147);
            this.Controls.Add(this.Value3Label);
            this.Controls.Add(this.Value3Textbox);
            this.Controls.Add(this.InfectiousCheck);
            this.Controls.Add(this.Value2Label);
            this.Controls.Add(this.Value2TextBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.Value1Textbox);
            this.Controls.Add(this.Value1Label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "InputBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InputBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Value1Label;
        private System.Windows.Forms.TextBox Value1Textbox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox Value2TextBox;
        private System.Windows.Forms.Label Value2Label;
        private System.Windows.Forms.CheckBox InfectiousCheck;
        private System.Windows.Forms.Label Value3Label;
        private System.Windows.Forms.TextBox Value3Textbox;
    }
}