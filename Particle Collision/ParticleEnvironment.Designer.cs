
namespace Particle_Collision
{
    partial class ParticleEnvironment
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
            this.components = new System.ComponentModel.Container();
            this.DrawBox = new System.Windows.Forms.PictureBox();
            this.TickTimer = new System.Windows.Forms.Timer(this.components);
            this.TimerToggleButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DrawBox
            // 
            this.DrawBox.BackColor = System.Drawing.Color.White;
            this.DrawBox.Location = new System.Drawing.Point(0, 0);
            this.DrawBox.Name = "DrawBox";
            this.DrawBox.Size = new System.Drawing.Size(800, 450);
            this.DrawBox.TabIndex = 0;
            this.DrawBox.TabStop = false;
            // 
            // TickTimer
            // 
            this.TickTimer.Enabled = true;
            this.TickTimer.Interval = 1;
            this.TickTimer.Tick += new System.EventHandler(this.TickTimer_Tick);
            // 
            // TimerToggleButton
            // 
            this.TimerToggleButton.Location = new System.Drawing.Point(806, 12);
            this.TimerToggleButton.Name = "TimerToggleButton";
            this.TimerToggleButton.Size = new System.Drawing.Size(75, 35);
            this.TimerToggleButton.TabIndex = 1;
            this.TimerToggleButton.Text = "Toggle Timer\r\n";
            this.TimerToggleButton.UseVisualStyleBackColor = true;
            this.TimerToggleButton.Click += new System.EventHandler(this.TimerToggleButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(806, 53);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 35);
            this.ResetButton.TabIndex = 2;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ParticleEnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(891, 450);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.TimerToggleButton);
            this.Controls.Add(this.DrawBox);
            this.Name = "ParticleEnvironment";
            this.ShowIcon = false;
            this.Text = "Environment";
            this.Load += new System.EventHandler(this.ParticleEnvironment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox DrawBox;
        public System.Windows.Forms.Timer TickTimer;
        private System.Windows.Forms.Button TimerToggleButton;
        private System.Windows.Forms.Button ResetButton;
    }
}

