using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jsonmaker
{
    public partial class GetPastedTextForm : Form
    {

        public string getText
        {
            get { return pasteBox.Text; }
        }

        public GetPastedTextForm()
        {
            InitializeComponent();
        }

        private void GetPastedTextForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool badData = true;
            string text = pasteBox.Text;
            List<string> sizes = new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Gargantuan" };

            //Grab beginning text to reduce chance of misread
            if (text.Length > 120)
                text = text.Substring(0, 120);

            //Text is considered good if it contains a size
            foreach (string s in sizes)
            {
                if (text.Contains(s))
                {
                    badData = false;
                }
            }

            //Don't make a monster
            if (badData)
            {
                System.Windows.Forms.MessageBox.Show("Bad text grabbed.\nGrab text starting at or before Size.");
            }
            else
                this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pasteBox.Text = "";
            this.Close();
        }
    }
}
