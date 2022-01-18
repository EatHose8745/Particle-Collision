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

        public InputBox(string question = "Input:")
        {
            InitializeComponent();
            this.QuestionLabel.Text = question;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.InputString = Input.Text;
            this.Close();
        }
    }
}
