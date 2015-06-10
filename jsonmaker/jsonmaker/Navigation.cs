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
using System.Globalization;
using System.Threading;


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
            string text = "YOUNG RED SHADOW DRAGON\nLarge dragon, chaotic evil\nArmor Class 18 (natural armor)\nHit Points 178 (17d10 + 85)\nSpeed 40ft., climb 40ft., fly 80ft.\nSTR\n23 (+6)\nDEX\n10 (+0)\nCON\n21 (+5)\nINT\n14 (+2)\nWIS\n11 (+0)\nSaving Throws Dex +4, Con +9, Wis +4, Cha +8\nSkills Perception +8, Stealth +8\nDamage Resistances necrotic\nDamage Immunities fire\nCHA\n19 (+4)\nSenses blindsight 30ft., darkvision 120ft.,\n passive Perception 18\nLanguages Common, Draconic\nChallenge 13 (10,000 XP)\nLiving Shadow. While in dim light or darkness, the dragon has\nresistance to damage that isn't force, psychic, or radiant.\nShadow Stealth. While in dim light or darkn ess, the dragon can\ntake the Hide action as a bonus action.\nSunlight Sensitivity. While in sunlight, the dragon has\ndi sadvantage on attack rolls, as well as on Wisdom\n(Perception) checks that rely on sight.\nACTIONS\nMultiattack. The dragon makes three attacks: one with its bite\nand two with its claws.\nBite. Melee Weapon Attack: +10 to hit, reach 10ft., one\ntarget. Hit: 17 (2d10 + 6) pierci ng damage plus 3 (1d6)\nnecrotic damage.\nClaw. Melee Weapon Attack: +10 to hit, reach 5 ft., one target.\nHit: 13 (2d6 + 6) slashing damage.\nShadow Breath (Recharge 5-6}. The dragon exhales shadowy\nfire in a 30-foot cone. Each creature in that area must make\na DC 18 Dexterity saving throw, taking 56 (16d6) necrotic\ndamage on a failed save, or half as much damage on a\nsuccessful one. A humanoid reduced to 0 hit points by this\ndamage dies, and an undead shadow rises from its corpse and\nacts immediately after the dragon in the initiative count. The\nshadow is under the dragon 's control.";
            Monster mon = new Monster();

            List<string> stats = new List<string> { "STR", "DEX", "CON", "INT", "WIS", "CHA" };
            List<string> sizes = new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Gargantuan" };
            List<string> alignments = new List<string>{"Unaligned", "Lawful Good", "Lawful Neutral", "Lawful Evil", "Neutral Good", "True Neutral",
                "Neutral Evil", "Chaotic Good", "Chaotic Neutral", "Chaotic Evil"};

            //Text Blob
            text = text.Trim();
            string substr = text;
            int indexOfSize = 0;
            bool badData = true;
            
            //Used for ToTitleCase
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;


            //Grab beginning text to reduce chance of misread
            if (text.Length > 120)
                substr = text.Substring(0, 120);

            //Text is considered good if it contains a size
            foreach (string s in sizes)
            {
                if (substr.Contains(s))
                {
                    badData = false;
                    indexOfSize = text.IndexOf(s);
                }
            }
            
            //Don't make a monster
            if (badData)
            {
                System.Windows.Forms.MessageBox.Show("Bad text grabbed.\nGrab text starting at or before Size.");
                return;
            }


            //Get Species in monster if it exists
            if(text.Substring(0, indexOfSize).Length != 0)
            {
                substr = text.Substring(0, text.IndexOf("\n"));
                mon.Species = textInfo.ToTitleCase(substr.ToLower());
            }
            //Remove Species text and whitespace
            text = text.Remove(0, indexOfSize);
            text = text.Trim();

            
            //Get Size
            substr = text.Substring(0, text.IndexOf(' '));
            if (sizes.Any(s => s.Equals(substr, StringComparison.OrdinalIgnoreCase)))
            {
                mon.Size = substr;
            }
            //Removes Size from string
            text = text.Remove(0, substr.Length);
            text = text.Trim();

            //Get Type
            substr = text.Substring(0, text.IndexOf(','));
            mon.Type = textInfo.ToTitleCase(substr.ToLower());
            //Remove Type and ',' from string
            text = text.Remove(0, substr.Length + 1);
            text = text.Trim();

            //Get Alignment
            substr = text.Substring(0, text.IndexOf('\n'));
            if (alignments.Any(s => s.Equals(substr, StringComparison.OrdinalIgnoreCase)))
            {
                mon.Alignment = textInfo.ToTitleCase(substr.ToLower());
            }
            //Removes Alignment from string
            text = text.Remove(0, substr.Length);
            text = text.Trim();


            //Get Armor Class
            substr = text.Substring(0, text.IndexOf('\n'));
            mon.ArmorClass = textInfo.ToTitleCase(substr.ToLower());
            //Remove Armor Class from stiring
            text = text.Remove(0, substr.Length);
            text = text.Trim();

            //Remove average health from string
            text = text.Remove(0, text.IndexOf('('));
            //Get Number of Hit Die
            substr = text.Substring(1, text.IndexOf('d')-1);
            mon.HitDieNum = Convert.ToInt32(substr);
            //Remove Number of Hit Die  and '(' from string
            text = text.Remove(0, substr.Length + 1);

            //Get Hit Die
            substr = text.Substring(0, text.IndexOf(' '));
            mon.HitDie = substr;
            //Remove Hit Die from string
            text = text.Remove(0, text.IndexOf('\n'));
            text = text.Trim();

            //Get Speed
            substr = text.Substring(0, text.IndexOf('\n'));
            mon.Speed = substr;
            //Remove Speed from string
            text = text.Remove(0, text.IndexOf('\n'));
            text = text.Trim();

            //Gets all text before actions
            //so they dont mess up parse
            substr = text.Substring(0, text.IndexOf("XP)"));

            //For every stat
            foreach (string s in stats)
            {
                //Remove Everything before each stat
                substr = substr.Remove(0, substr.IndexOf(s) + 3);
                substr = substr.Trim();
                switch (s)
                {
                    case "STR":
                        mon.Strength = Convert.ToInt32(substr.Substring(0, substr.IndexOf('(') - 1));                            
                        break;
                    case "DEX":
                        mon.Dexterity = Convert.ToInt32(substr.Substring(0, substr.IndexOf('(') - 1));
                        break;
                    case "CON":
                        mon.Constitution = Convert.ToInt32(substr.Substring(0, substr.IndexOf('(') - 1));
                        break;
                    case "INT":
                        mon.Intelligence = Convert.ToInt32(substr.Substring(0, substr.IndexOf('(') - 1));
                        break;
                    case "WIS":
                        mon.Wisdom = Convert.ToInt32(substr.Substring(0, substr.IndexOf('(') - 1));
                        break;
                    case "CHA":
                        mon.Charisma = Convert.ToInt32(substr.Substring(0, substr.IndexOf('(') - 1));
                        break;         
                }
                text = text.Remove(text.IndexOf(s), text.IndexOf(')') - text.IndexOf(s) + 1);
            }
            text = text.Trim();



            //If Saving Throws Exists in text
            //Get Saving Throws
            if (text.Substring(0, 10).Contains("Saving"))
            {
                substr = text.Substring(0, text.IndexOf("XP)"));

                //Get Saving Throws
                foreach (string s in stats)
                {
                    if (substr.Contains(textInfo.ToTitleCase(s)))
                    {
                        switch (textInfo.ToTitleCase(s))
                        {
                            case "Str":
                                mon.StrengthSave = true;
                                break;
                            case "Dex":
                                mon.DexteritySave = true;
                                break;
                            case "Con":
                                mon.ConstitutionSave = true;
                                break;
                            case "Int":
                                mon.IntelligenceSave = true;
                                break;
                            case "Wis":
                                mon.WisdomSave = true;
                                break;
                            case "Cha":
                                mon.CharismaSave = true;
                                break;
                        }
                    }
                }

                //Remove Saving Throws from text
                text = text.Remove(0, text.IndexOf('\n'));
                text = text.Trim();
            }

            //If Skills Exist in text
            //Get Skills
            if (text.Substring(0, 10).Contains("Skills"))
            {

                //Remove "Skills " from text
                text = text.Replace("Skills ", "");
                substr = text.Substring(0, text.IndexOf("Senses"));

                string temp;
                int tempNum;

                //Loop until no more ',' in skills
                do
                {
                    temp = substr.Substring(0, substr.IndexOf('+') - 1);
                    temp.Trim();
                    substr = substr.Remove(0, substr.IndexOf('+') + 1);
                    tempNum = Convert.ToInt32(substr.Substring(0, substr.IndexOf(',')));
                    mon.Skills.Add(temp, tempNum);
                    substr = substr.Remove(0, substr.IndexOf(',') + 1);
                    substr = substr.Trim();

                } while (substr.Contains(','));

                //Get Last Skill
                temp = substr.Substring(0, substr.IndexOf('+') - 1);
                temp.Trim();
                substr = substr.Remove(0, substr.IndexOf('+') + 1);
                tempNum = Convert.ToInt32(substr.Substring(0,substr.IndexOf('\n')));
                mon.Skills.Add(temp, tempNum);

                //Remove Skills from text
                text = text.Remove(0, text.IndexOf('\n'));
                text = text.Trim();
            }

            //If Damage Vulnerabilities Exist in Text
            //Get Damage Vulnerabilities
            if (text.Substring(0, 14).Contains("Damage V"))
            {
                text = text.Replace("Damage Vulnerabilities ", "");

                //Damage Vulnerabilities may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select text.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');

                mon.DamageVulnerability = textInfo.ToTitleCase(substr.ToLower());
                //Remove Damage Vulnerabilities from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();
            }

            //If Damage Resistances Exist in Text
            //Get Damage Resistances
            if (text.Substring(0, 10).Contains("Damage R"))
            {
                
                text = text.Replace("Damage Resistances ", "");

                //Damage resistances may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select text.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');

                mon.DamageResistance = textInfo.ToTitleCase(substr.ToLower());
                //Remove Armor Class from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();
            }


            //If Damage Immunities Exist in Text
            //Get Damage Immunities
            if (text.Substring(0, 10).Contains("Damage I"))
            {                
                text = text.Replace("Damage Immunities ", "");

                //Damage Immunities may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select text.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');

                mon.DamageImmunity = textInfo.ToTitleCase(substr.ToLower());
                //Remove Damage Immunities from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();
            }

            //If Condition Immunities Exist in Text
            //Get Condition Immunities
            if (text.Substring(0, 14).Contains("Condition I"))
            {
                text = text.Replace("Condition Immunities ", "");

                //Condition Immunities may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select text.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');

                mon.ConditionImmunity = textInfo.ToTitleCase(substr.ToLower());
                //Remove Condition Immunities from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();
            }

            //If Senses Exist in Text
            //Get Senses
            if (text.Substring(0, 14).Contains("Senses"))
            {

                //Remove "Senses " from text
                text = text.Replace("Senses ", "");
                substr = text.Substring(0, text.IndexOf("Languages"));

                string temp;
                

                //Loop until no more ',' in senses
                do
                {
                    temp = substr.Substring(0, substr.IndexOf(',') - 1);
                    temp.Trim();
                    mon.Senses.Add(temp);
                    substr = substr.Remove(0, substr.IndexOf(',') + 1);
                    substr = substr.Trim();

                } while (substr.Contains(','));

                //Get Last Sense

                mon.Senses.Add(substr);

                //Remove Senses from text
                text = text.Remove(0, text.IndexOf("Languages"));
                text = text.Trim();
            }


            //Get Languages
            //Remove "Lanaguages " from text
            text = text.Replace("Languages ", "");
            substr = text.Substring(0, text.IndexOf("Challenge"));

            mon.Languages = textInfo.ToTitleCase(substr.ToLower());

            //Remove Languages substring from text
            text = text.Remove(0, substr.Length);
            text = text.Trim();


            //Get Challenge Level
            text = text.Replace("Challenge ", "");
            substr = text.Substring(0, text.IndexOf('('));
            mon.Challenge = substr;




            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}