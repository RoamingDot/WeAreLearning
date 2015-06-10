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
            List<string> stats = new List<string> { "STR", "DEX", "CON", "INT", "WIS", "CHA" };


            //Text is considered good if it contains a size
            foreach (string s in sizes)
            {
                if (text.Contains(s))
                {
                    badData = false;
                    break;
                }
            }
            foreach (string s in stats)
            {
                if (!text.Contains(s))
                    badData = true;
            }

            //Don't make a monster
            if (badData)
            {
                System.Windows.Forms.MessageBox.Show("Bad text grabbed. Some text may not have copied properly.\nGrab text starting at or before Size.");
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
