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

        string jsonPath;
        string jsonFileName;
        RootObject monsterList;

        private void button1_Click(object sender, EventArgs e)
        {
         
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = ".";
            openFileDialog1.Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                jsonPath = Path.GetDirectoryName(openFileDialog1.FileName);
                jsonFileName = openFileDialog1.FileName;
                try
                {
                    monsterList = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText
                        (Path.GetFullPath(openFileDialog1.FileName)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }


            foreach(Monster mon in monsterList.Monster)
                    MessageBox.Show(mon.Type);
            label1.Text = monsterList.Monster[0].MaxHP + " ___ " + monsterList.Monster[1].Alignment;
        }
    }


    public class Monster
    {
        public string Type { get; set; }
        public float Challenge { get; set; }
        public int PageNumber { get; set; }
        public string Size { get; set; }
        public string Appearance { get; set; }
        public string Race { get; set; }
        public string Alignment { get; set; }
        public int ArmorClass { get; set; }
        public int MinHP { get; set; }
        public int MaxHP { get; set; }
        public int Speed { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public string DamageVulnerability { get; set; }
        public string DamageResistance { get; set; }
        public string Description { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Senses { get; set; }
        public List<string> Actions { get; set; }
        public List<string> LegendaryActions { get; set; }
        public List<string> Skills { get; set; }
    }

    public class RootObject
    {
        public List<Monster> Monster { get; set; }
    }

}


