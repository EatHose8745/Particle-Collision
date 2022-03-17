
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
            this.LoadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SpawnerRadio = new System.Windows.Forms.RadioButton();
            this.WindowRadio = new System.Windows.Forms.RadioButton();
            this.NoneRadio = new System.Windows.Forms.RadioButton();
            this.BorderRadio = new System.Windows.Forms.RadioButton();
            this.RandomButton = new System.Windows.Forms.Button();
            this.TimerTicksDisplay = new System.Windows.Forms.Label();
            this.DescPanel = new System.Windows.Forms.Panel();
            this.ItemDesc = new System.Windows.Forms.Label();
            this.HoverDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TickPerSecondSlider = new System.Windows.Forms.TrackBar();
            this.TicksPerSecondLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).BeginInit();
            this.ButtonPanel.SuspendLayout();
            this.DescPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TickPerSecondSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // DrawBox
            // 
            this.DrawBox.BackColor = System.Drawing.Color.White;
            this.DrawBox.Location = new System.Drawing.Point(0, 0);
            this.DrawBox.Name = "DrawBox";
            this.DrawBox.Size = new System.Drawing.Size(800, 417);
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
            this.TimerToggleButton.MouseEnter += new System.EventHandler(this.TimerToggleButton_MouseEnter);
            this.TimerToggleButton.MouseLeave += new System.EventHandler(this.TimerToggleButton_MouseLeave);
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
            this.ResetButton.MouseEnter += new System.EventHandler(this.ResetButton_MouseEnter);
            this.ResetButton.MouseLeave += new System.EventHandler(this.ResetButton_MouseLeave);
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.TicksPerSecondLabel);
            this.ButtonPanel.Controls.Add(this.TickPerSecondSlider);
            this.ButtonPanel.Controls.Add(this.LoadButton);
            this.ButtonPanel.Controls.Add(this.SaveButton);
            this.ButtonPanel.Controls.Add(this.SpawnerRadio);
            this.ButtonPanel.Controls.Add(this.WindowRadio);
            this.ButtonPanel.Controls.Add(this.NoneRadio);
            this.ButtonPanel.Controls.Add(this.BorderRadio);
            this.ButtonPanel.Controls.Add(this.RandomButton);
            this.ButtonPanel.Controls.Add(this.TimerTicksDisplay);
            this.ButtonPanel.Controls.Add(this.TimerToggleButton);
            this.ButtonPanel.Controls.Add(this.ResetButton);
            this.ButtonPanel.ForeColor = System.Drawing.Color.Black;
            this.ButtonPanel.Location = new System.Drawing.Point(804, 0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(81, 450);
            this.ButtonPanel.TabIndex = 3;
            // 
            // LoadButton
            // 
            this.LoadButton.ForeColor = System.Drawing.Color.Black;
            this.LoadButton.Location = new System.Drawing.Point(3, 184);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 35);
            this.LoadButton.TabIndex = 10;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            this.LoadButton.MouseEnter += new System.EventHandler(this.LoadButton_MouseEnter);
            this.LoadButton.MouseLeave += new System.EventHandler(this.LoadButton_MouseLeave);
            // 
            // SaveButton
            // 
            this.SaveButton.ForeColor = System.Drawing.Color.Black;
            this.SaveButton.Location = new System.Drawing.Point(3, 142);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 35);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            this.SaveButton.MouseEnter += new System.EventHandler(this.SaveButton_MouseEnter);
            this.SaveButton.MouseLeave += new System.EventHandler(this.SaveButton_MouseLeave);
            // 
            // SpawnerRadio
            // 
            this.SpawnerRadio.AutoSize = true;
            this.SpawnerRadio.ForeColor = System.Drawing.Color.White;
            this.SpawnerRadio.Location = new System.Drawing.Point(3, 414);
            this.SpawnerRadio.Name = "SpawnerRadio";
            this.SpawnerRadio.Size = new System.Drawing.Size(67, 17);
            this.SpawnerRadio.TabIndex = 8;
            this.SpawnerRadio.Tag = "Radio";
            this.SpawnerRadio.Text = "Spawner";
            this.SpawnerRadio.UseVisualStyleBackColor = true;
            this.SpawnerRadio.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // WindowRadio
            // 
            this.WindowRadio.AutoSize = true;
            this.WindowRadio.ForeColor = System.Drawing.Color.White;
            this.WindowRadio.Location = new System.Drawing.Point(3, 391);
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
            this.NoneRadio.Location = new System.Drawing.Point(3, 345);
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
            this.BorderRadio.Location = new System.Drawing.Point(3, 368);
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
            this.RandomButton.MouseEnter += new System.EventHandler(this.RandomButton_MouseEnter);
            this.RandomButton.MouseLeave += new System.EventHandler(this.RandomButton_MouseLeave);
            // 
            // TimerTicksDisplay
            // 
            this.TimerTicksDisplay.AutoSize = true;
            this.TimerTicksDisplay.ForeColor = System.Drawing.Color.White;
            this.TimerTicksDisplay.Location = new System.Drawing.Point(3, 237);
            this.TimerTicksDisplay.Name = "TimerTicksDisplay";
            this.TimerTicksDisplay.Size = new System.Drawing.Size(33, 13);
            this.TimerTicksDisplay.TabIndex = 3;
            this.TimerTicksDisplay.Text = "Ticks";
            // 
            // DescPanel
            // 
            this.DescPanel.Controls.Add(this.ItemDesc);
            this.DescPanel.Controls.Add(this.HoverDescription);
            this.DescPanel.Controls.Add(this.label1);
            this.DescPanel.Location = new System.Drawing.Point(0, 420);
            this.DescPanel.Name = "DescPanel";
            this.DescPanel.Size = new System.Drawing.Size(800, 27);
            this.DescPanel.TabIndex = 4;
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSize = true;
            this.ItemDesc.ForeColor = System.Drawing.SystemColors.Control;
            this.ItemDesc.Location = new System.Drawing.Point(8, 7);
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.Size = new System.Drawing.Size(0, 13);
            this.ItemDesc.TabIndex = 2;
            // 
            // HoverDescription
            // 
            this.HoverDescription.AutoSize = true;
            this.HoverDescription.ForeColor = System.Drawing.SystemColors.Control;
            this.HoverDescription.Location = new System.Drawing.Point(8, 7);
            this.HoverDescription.Name = "HoverDescription";
            this.HoverDescription.Size = new System.Drawing.Size(0, 13);
            this.HoverDescription.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // TickPerSecondSlider
            // 
            this.TickPerSecondSlider.Location = new System.Drawing.Point(3, 253);
            this.TickPerSecondSlider.Maximum = 1000;
            this.TickPerSecondSlider.Minimum = 10;
            this.TickPerSecondSlider.Name = "TickPerSecondSlider";
            this.TickPerSecondSlider.Size = new System.Drawing.Size(75, 45);
            this.TickPerSecondSlider.TabIndex = 11;
            this.TickPerSecondSlider.Value = 1000;
            this.TickPerSecondSlider.Scroll += new System.EventHandler(this.TickPerSecondSlider_Scroll);
            this.TickPerSecondSlider.ValueChanged += new System.EventHandler(this.TickPerSecondSlider_ValueChanged);
            // 
            // TicksPerSecondLabel
            // 
            this.TicksPerSecondLabel.AutoSize = true;
            this.TicksPerSecondLabel.ForeColor = System.Drawing.Color.White;
            this.TicksPerSecondLabel.Location = new System.Drawing.Point(3, 301);
            this.TicksPerSecondLabel.Name = "TicksPerSecondLabel";
            this.TicksPerSecondLabel.Size = new System.Drawing.Size(68, 13);
            this.TicksPerSecondLabel.TabIndex = 12;
            this.TicksPerSecondLabel.Text = "Tick/s: 1000";
            // 
            // ParticleEnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(887, 450);
            this.Controls.Add(this.DescPanel);
            this.Controls.Add(this.ButtonPanel);
            this.Controls.Add(this.DrawBox);
            this.Name = "ParticleEnvironment";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Particle Simulation Interface";
            this.Load += new System.EventHandler(this.ParticleEnvironment_Load);
            this.SizeChanged += new System.EventHandler(this.ParticleEnvironment_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).EndInit();
            this.ButtonPanel.ResumeLayout(false);
            this.ButtonPanel.PerformLayout();
            this.DescPanel.ResumeLayout(false);
            this.DescPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TickPerSecondSlider)).EndInit();
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
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Panel DescPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label HoverDescription;
        private System.Windows.Forms.Label ItemDesc;
        private System.Windows.Forms.TrackBar TickPerSecondSlider;
        private System.Windows.Forms.Label TicksPerSecondLabel;
    }
}

