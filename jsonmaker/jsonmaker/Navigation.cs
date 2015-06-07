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

                    //Puts JSON into JSON box tab
                    string json = JsonConvert.SerializeObject(mon, Formatting.Indented);
                    monsterJSONBox.Text = json;

                    //Formats JSON into readable text and puts in Text box tab
                    string text = "";
                    foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(mon))
                    {

                        if (property.PropertyType == typeof(List<string>))
                        {

                            //appends EX:    "Senses:"
                            //                  "passive Perception 13"
                            //                  "test"
                            string name = property.Name;
                            List<string> subproperty = (List<string>)property.GetValue(mon);

                            
                            if (subproperty != null)
                            {
                                text += name + ":";
                                text += Environment.NewLine;
                                foreach (string s in subproperty)
                                {
                                    text += "  ";
                                    text += s;
                                    text += Environment.NewLine;
                                }
                            }
                                                   
                            

                        }
                        else
                        {
                            //appends EX: "Type: Ape"
                            string name = property.Name;
                            object value = property.GetValue(mon);
                            text += string.Format("{0}: {1}", name, value);
                            text += Environment.NewLine;

                        }
                    }
                        
                    //Fill box with Text
                    monsterTextBox.Text = text;

                }
            }
        }

        private void CreateMonster()
        {
            
        }

        private void Navigation_Load(object sender, EventArgs e)
        {

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


