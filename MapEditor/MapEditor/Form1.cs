using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MapEditor
{
    public partial class Form1 : Form
    {
        public int lvlNum;

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            numBox.Text = null;
            mapBox.Text = null;
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
           


           if (numBox.Text != " " || mapBox.Text != " ")
            {
                string filename = "level" + numBox.Text + ".txt";
                StreamWriter writer = new StreamWriter(filename, false);
                Directory.Move(@"F:\Debug", @"F:\UGWCodeProj - Copy\bin\WindowsGL\Debug");
                writer.WriteLine(mapBox.Text);
                
                writer.Close();

                MessageBox.Show("Level has been created.");
            }

           else if (numBox.Text == " " || mapBox.Text == " ")
            {
                MessageBox.Show("There are empty boxes. Please fill them out.");
                return;
            }
        }
    }
}
