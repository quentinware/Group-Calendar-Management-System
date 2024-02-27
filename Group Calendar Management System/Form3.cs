using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar_Management_System
{
    /*
     *  THIS IS THE FORM THAT HANDLES THE LOGIN 
     * 
     */

    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if(Employee.validateIdentification(textBox1.Text, textBox2.Text))
            {
                this.Hide();
                var form1 = new Form1(textBox1.Text);
                form1.Closed += (s, args) => this.Close();
                form1.Show();
            }
            else
            {
                DialogResult res = MessageBox.Show("There is an error", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if(res == DialogResult.OK)
                {
                    MessageBox.Show("You have clicked the Ok button");
                }
                if(res == DialogResult.Cancel)
                {
                    MessageBox.Show("You have clicked the cancel button");
                }
            }

            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
