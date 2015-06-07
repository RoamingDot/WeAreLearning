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
using Newtonsoft.Json;


namespace jsonmaker
{
    
    public partial class Navigation : Form
    {
        public Navigation()
        {
            InitializeComponent();
        }

        RootObject monsterList;

        public static Dictionary<string, int> choices = new Dictionary<string, int>() 
        {
           { "d4", 4},
           { "d6", 6},
           { "d8", 8},
           { "d10", 10}, 
           { "d12", 12},
           { "d20", 20},
           { "d100", 100}
        };

        private void button1_Click(object sender, EventArgs e)
        {
         
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = ".";
            openFileDialog1.Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    monsterList = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText
                        (Path.GetFullPath(openFileDialog1.FileName)));
                    FileLocation1.Text = Path.GetFullPath(openFileDialog1.FileName) + " was loaded.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }


        }

        private void dataButton_Click(object sender, EventArgs e)
        {


            foreach (Monster mon in monsterList.Monster)
            {
                monsterBox.Items.Add(mon.Type);
            }
       

        }

        private void monsterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Monster mon in monsterList.Monster)
            {
                if (String.Equals(monsterBox.GetItemText(monsterBox.SelectedItem), mon.Type))
                {
                    string json = JsonConvert.SerializeObject(mon, Formatting.Indented);
                    textBox1.Text = json;
                }
            }
        }

        private void CreateMonster()
        {
            
        }

        private void Navigation_Load(object sender, EventArgs e)
        {

        }

    }


    public class RootObject
    {
        public List<Monster> Monster { get; set; }
    }

}


