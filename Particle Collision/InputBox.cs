using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Particle_Collision
{
    public partial class InputBox : Form
    {
        public string InputString { get; private set; }
        public string Input2String { get; private set; }
        public string Input3String { get; private set; }
        public bool Infectious { get; private set; }

        public InputBox(string question = "Input:", string question2 = "", string question3 = "")
        {
            InitializeComponent();
            this.Value1Label.Text = question;
            this.Value2Label.Text = question2;
            this.Value3Label.Text = question3;
            if (question2 == "")
                Value2TextBox.Hide();
            if (question3 == "")
            {
                Value3Textbox.Hide();
                if (question3 != "Frequency:")
                    InfectiousCheck.Hide();
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.InputString = Value1Textbox.Text;
            this.Input2String = Value2TextBox.Text;
            this.Input3String = Value3Textbox.Text;
            this.Infectious = InfectiousCheck.Checked;
            this.Close();
        }
    }
}
