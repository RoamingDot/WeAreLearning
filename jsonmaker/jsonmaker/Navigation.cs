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

        RootObject monsterList; //Grabs the root object of the JSON file
        OpenFileDialog openFileDialog1;
        
        private void button1_Click(object sender, EventArgs e)
        {
            bool confirmed = true; //Confirmation to load variable

            openFileDialog1.InitialDirectory = ".";
            openFileDialog1.Filter = "Json Files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (monsterList.Monster.Count() > 0)
            {
                DialogResult result;

                result = MessageBox.Show(
                        "There are currently unsaved changes. Discard and load from file?",
                        "Current list cleared.",
                        MessageBoxButtons.YesNo);
                confirmed = result == DialogResult.Yes;
            }

            if (confirmed && openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    monsterList.Monster.Clear();

                    monsterList = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText
                        (Path.GetFullPath(openFileDialog1.FileName)));

                    //Inform via label success
                    FileLocation1.Text = Path.GetFullPath(openFileDialog1.FileName) + " was loaded.";

                    //Allow population button and refresh the monster box
                    RefreshMonsterBox();

                    //Show Add To button
                    addMonButton.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void dataButton_Click(object sender, EventArgs e)
        {
            RefreshMonsterBox();
        }

        private void monsterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Monster mon in monsterList.Monster)
            {
                if (String.Equals(monsterBox.GetItemText(monsterBox.SelectedItem), mon.Species))
                {
                 
                    //Puts JSON into JSON box tab
                    string json = JsonConvert.SerializeObject(mon, Formatting.Indented);
                    monsterJSONBox.Text = json;
                    
                    //Formats JSON into readable text and puts in Text box tab
                    //Fill box with Text
                    monsterTextBox.Text = mon.ToString();

                }
            }
        }

        private void FillMonster(Monster workingMonster)
        {

            workingMonster.Species = speciesBox.Text;
            workingMonster.Challenge = challengeInput.Text;
            workingMonster.Size = sizeInput.Text;
            workingMonster.Alignment = alignInput.Text;
            workingMonster.ArmorClass = armorBox.Text;
            workingMonster.Speed = speedBox.Text;
            workingMonster.DamageImmunity = damImmBox1.Text;
            workingMonster.DamageResistance = damResBox1.Text;
            workingMonster.DamageVulnerability = damVulBox1.Text;
            workingMonster.ConditionImmunity = conImmBox1.Text;
            workingMonster.Languages = langBox.Text;
            workingMonster.PageNumber = (int)pageInput.Value;
            workingMonster.HitDie = hitDInput.Text;
            workingMonster.Strength = (int)strInput.Value;
            workingMonster.Dexterity = (int)dexInput.Value;
            workingMonster.Constitution = (int)conInput.Value;
            workingMonster.Wisdom = (int)wisInput.Value;
            workingMonster.Charisma = (int)chaInput.Value;
            workingMonster.StrengthSave = strCheck.Checked;
            workingMonster.DexteritySave = dexCheck.Checked;
            workingMonster.ConstitutionSave = conCheck.Checked;
            workingMonster.WisdomSave = wisCheck.Checked;
            workingMonster.CharismaSave = chaCheck.Checked;

            if (!string.IsNullOrWhiteSpace(senseBox1.Text))
                workingMonster.Senses.Add(senseBox1.Text);
            if (!string.IsNullOrWhiteSpace(senseBox2.Text))
                workingMonster.Senses.Add(senseBox2.Text);
            if (!string.IsNullOrWhiteSpace(senseBox3.Text))
                workingMonster.Senses.Add(senseBox3.Text);
            if (!string.IsNullOrWhiteSpace(senseBox4.Text))
                workingMonster.Senses.Add(senseBox4.Text);
            if (!string.IsNullOrWhiteSpace(senseBox5.Text))
                workingMonster.Senses.Add(senseBox5.Text);
            if (!string.IsNullOrWhiteSpace(senseBox6.Text))
                workingMonster.Senses.Add(senseBox6.Text);
            if (!string.IsNullOrWhiteSpace(senseBox7.Text))
                workingMonster.Senses.Add(senseBox7.Text);


        }

        /***
         * EVENT: TEXT CHANGED ON SPECIES BOX
         * Warning: May cause performance issues with larger lists and long names
         ***/
        private void speciesBox_TextChanged(object sender, EventArgs e)
        {
            bool monsterUnique = true;
            try
            {   
                if (!string.IsNullOrWhiteSpace(speciesBox.Text))
                {
                    foreach (Monster m in monsterList.Monster)
                    {
                        if (m.Species == speciesBox.Text)
                        {
                            monsterUnique = false;
                            break;
                        }
                    }
                }
                else
                {
                    monsterUnique = false;
                }

                if (!monsterUnique)
                    addMonButton.Enabled = false;
                else
                    addMonButton.Enabled = true;
            }
            catch
            {
                // If there is an error, display the text using the system colors (?)
                speciesBox.ForeColor = SystemColors.ControlText;
            }
        }


        private void Navigation_Load(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            monsterList = new RootObject();

            RefreshMonsterBox();
        }

        private void addMonButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(speciesBox.Text))
            {
                MessageBox.Show("Entity must have a species name (e.g. Bandit)");
            }
            else
            {
                    Monster newMonster = new Monster();
                    FillMonster(newMonster);

                    monsterList.Monster.Add(newMonster);
                    RefreshMonsterBox();
            }
        }

        private void RefreshMonsterBox()
        {
            //Get rid stale data
            monsterBox.Items.Clear();

            //Load in fresh data from the monster list
            foreach (Monster mon in monsterList.Monster)
            {
                monsterBox.Items.Add(mon.Species);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = "YOUNG RED SHADOW DRAGON\nLarge dragon, chaotic evil\nArmor Class 18 (natural armor)\nHit Points 178 (17d10 + 85)\nSpeed 40ft., climb 40ft., fly 80ft.\nSTR\n23 (+6)\nDEX\n10 (+0)\nCON\n21 (+5)\nINT\n14 (+2)\nWIS\n11 (+0)\nSaving Throws Dex +4, Con +9, Wis +4, Cha +8\nSkills Perception +8, Stealth +8\nDamage Resistances necrotic\nDamage Immunities fire\nCHA\n19 (+4)\nSenses blindsight 30ft., darkvision 120ft., passive Perception 18\nLanguages Common, Draconic\nChallenge 13 (10,000 XP)\nLiving Shadow. While in dim light or darkness, the dragon has\nresistance to damage that isn't force, psychic, or radiant.\nShadow Stealth. While in dim light or darkn ess, the dragon can\ntake the Hide action as a bonus action.\nSunlight Sensitivity. While in sunlight, the dragon has\ndi sadvantage on attack rolls, as well as on Wisdom\n(Perception) checks that rely on sight.\nACTIONS\nMultiattack. The dragon makes three attacks: one with its bite\nand two with its claws.\nBite. Melee Weapon Attack: +10 to hit, reach 10ft., one\ntarget. Hit: 17 (2d10 + 6) pierci ng damage plus 3 (1d6)\nnecrotic damage.\nClaw. Melee Weapon Attack: +10 to hit, reach 5 ft., one target.\nHit: 13 (2d6 + 6) slashing damage.\nShadow Breath (Recharge 5-6}. The dragon exhales shadowy\nfire in a 30-foot cone. Each creature in that area must make\na DC 18 Dexterity saving throw, taking 56 (16d6) necrotic\ndamage on a failed save, or half as much damage on a\nsuccessful one. A humanoid reduced to 0 hit points by this\ndamage dies, and an undead shadow rises from its corpse and\nacts immediately after the dragon in the initiative count. The\nshadow is under the dragon 's control.";
            Monster test = new Monster(text);
        }
    }
}