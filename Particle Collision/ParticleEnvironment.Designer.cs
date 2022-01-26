
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
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.WindowRadio = new System.Windows.Forms.RadioButton();
            this.NoneRadio = new System.Windows.Forms.RadioButton();
            this.BorderRadio = new System.Windows.Forms.RadioButton();
            this.RandomButton = new System.Windows.Forms.Button();
            this.TimerTicksDisplay = new System.Windows.Forms.Label();
            this.SpawnerRadio = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).BeginInit();
            this.ButtonPanel.SuspendLayout();
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
            this.DrawBox.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawBox_Paint);
            this.DrawBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseClick);
            this.DrawBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseDoubleClick);
            this.DrawBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseDown);
            this.DrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseMove);
            this.DrawBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseUp);
            // 
            // TickTimer
            // 
            this.TickTimer.Enabled = true;
            this.TickTimer.Interval = 1;
            this.TickTimer.Tick += new System.EventHandler(this.TickTimer_Tick);
            // 
            // TimerToggleButton
            // 
            this.TimerToggleButton.ForeColor = System.Drawing.Color.Black;
            this.TimerToggleButton.Location = new System.Drawing.Point(3, 3);
            this.TimerToggleButton.Name = "TimerToggleButton";
            this.TimerToggleButton.Size = new System.Drawing.Size(75, 35);
            this.TimerToggleButton.TabIndex = 1;
            this.TimerToggleButton.Text = "Toggle Timer\r\n";
            this.TimerToggleButton.UseVisualStyleBackColor = true;
            this.TimerToggleButton.Click += new System.EventHandler(this.TimerToggleButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.ForeColor = System.Drawing.Color.Black;
            this.ResetButton.Location = new System.Drawing.Point(3, 44);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 35);
            this.ResetButton.TabIndex = 2;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.SpawnerRadio);
            this.ButtonPanel.Controls.Add(this.WindowRadio);
            this.ButtonPanel.Controls.Add(this.NoneRadio);
            this.ButtonPanel.Controls.Add(this.BorderRadio);
            this.ButtonPanel.Controls.Add(this.RandomButton);
            this.ButtonPanel.Controls.Add(this.TimerTicksDisplay);
            this.ButtonPanel.Controls.Add(this.TimerToggleButton);
            this.ButtonPanel.Controls.Add(this.ResetButton);
            this.ButtonPanel.ForeColor = System.Drawing.Color.Black;
            this.ButtonPanel.Location = new System.Drawing.Point(806, 0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(81, 450);
            this.ButtonPanel.TabIndex = 3;
            // 
            // WindowRadio
            // 
            this.WindowRadio.AutoSize = true;
            this.WindowRadio.ForeColor = System.Drawing.Color.White;
            this.WindowRadio.Location = new System.Drawing.Point(3, 335);
            this.WindowRadio.Name = "WindowRadio";
            this.WindowRadio.Size = new System.Drawing.Size(64, 17);
            this.WindowRadio.TabIndex = 7;
            this.WindowRadio.Tag = "Radio";
            this.WindowRadio.Text = "Window";
            this.WindowRadio.UseVisualStyleBackColor = true;
            this.WindowRadio.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // NoneRadio
            // 
            this.NoneRadio.AutoSize = true;
            this.NoneRadio.Checked = true;
            this.NoneRadio.ForeColor = System.Drawing.Color.White;
            this.NoneRadio.Location = new System.Drawing.Point(3, 289);
            this.NoneRadio.Name = "NoneRadio";
            this.NoneRadio.Size = new System.Drawing.Size(51, 17);
            this.NoneRadio.TabIndex = 6;
            this.NoneRadio.TabStop = true;
            this.NoneRadio.Tag = "Radio";
            this.NoneRadio.Text = "None";
            this.NoneRadio.UseVisualStyleBackColor = true;
            this.NoneRadio.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // BorderRadio
            // 
            this.BorderRadio.AutoSize = true;
            this.BorderRadio.ForeColor = System.Drawing.Color.White;
            this.BorderRadio.Location = new System.Drawing.Point(3, 312);
            this.BorderRadio.Name = "BorderRadio";
            this.BorderRadio.Size = new System.Drawing.Size(56, 17);
            this.BorderRadio.TabIndex = 5;
            this.BorderRadio.Tag = "Radio";
            this.BorderRadio.Text = "Border";
            this.BorderRadio.UseVisualStyleBackColor = true;
            this.BorderRadio.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // RandomButton
            // 
            this.RandomButton.ForeColor = System.Drawing.Color.Black;
            this.RandomButton.Location = new System.Drawing.Point(3, 85);
            this.RandomButton.Name = "RandomButton";
            this.RandomButton.Size = new System.Drawing.Size(75, 35);
            this.RandomButton.TabIndex = 4;
            this.RandomButton.Text = "Random";
            this.RandomButton.UseVisualStyleBackColor = true;
            this.RandomButton.Click += new System.EventHandler(this.RandomButton_Click);
            // 
            // TimerTicksDisplay
            // 
            this.TimerTicksDisplay.AutoSize = true;
            this.TimerTicksDisplay.ForeColor = System.Drawing.Color.White;
            this.TimerTicksDisplay.Location = new System.Drawing.Point(3, 252);
            this.TimerTicksDisplay.Name = "TimerTicksDisplay";
            this.TimerTicksDisplay.Size = new System.Drawing.Size(33, 13);
            this.TimerTicksDisplay.TabIndex = 3;
            this.TimerTicksDisplay.Text = "Ticks";
            // 
            // SpawnerRadio
            // 
            this.SpawnerRadio.AutoSize = true;
            this.SpawnerRadio.ForeColor = System.Drawing.Color.White;
            this.SpawnerRadio.Location = new System.Drawing.Point(3, 358);
            this.SpawnerRadio.Name = "SpawnerRadio";
            this.SpawnerRadio.Size = new System.Drawing.Size(67, 17);
            this.SpawnerRadio.TabIndex = 8;
            this.SpawnerRadio.Tag = "Radio";
            this.SpawnerRadio.Text = "Spawner";
            this.SpawnerRadio.UseVisualStyleBackColor = true;
            this.SpawnerRadio.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // ParticleEnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(887, 450);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.DrawBox);
            this.Name = "ParticleEnvironment";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Environment";
            this.Load += new System.EventHandler(this.ParticleEnvironment_Load);
            this.SizeChanged += new System.EventHandler(this.ParticleEnvironment_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).EndInit();
            this.ButtonPanel.ResumeLayout(false);
            this.ButtonPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox DrawBox;
        public System.Windows.Forms.Timer TickTimer;
        private System.Windows.Forms.Button TimerToggleButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Label TimerTicksDisplay;
        private System.Windows.Forms.Button RandomButton;
        private System.Windows.Forms.RadioButton BorderRadio;
        private System.Windows.Forms.RadioButton NoneRadio;
        private System.Windows.Forms.RadioButton WindowRadio;
        private System.Windows.Forms.RadioButton SpawnerRadio;
    }
}

