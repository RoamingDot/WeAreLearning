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
        bool editMode;
        bool monsterUnique = true;
        Monster selectMonster;
        OpenFileDialog openFileDialog1;

        private void Navigation_Load(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            monsterList = new RootObject();

            RefreshMonsterBox();
            ClearForms();
        }

        private void monsterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Monster mon;

            try
            {
                if (monsterBox.SelectedIndex < 0)
                    MessageBox.Show("Unable to load monster from list to Text box.");
                else
                { 
                    mon = monsterList.Monster[monsterBox.SelectedIndex];

                    //Puts JSON into JSON box tab
                    string json = JsonConvert.SerializeObject(mon, Formatting.Indented);
                    monsterJSONBox.Text = json;

                    //Fill box with Monsters
                    monsterTextBox.Text = mon.ToString();

                    populateButton.Enabled = true;
                    deleteButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught during monster selection: " + ex);
            }

        }


        /***
         * EVENT: TEXT CHANGED ON SPECIES BOX
         * Warning: May cause performance issues with larger lists and long names
         ***/
        private void speciesBox_TextChanged(object sender, EventArgs e)
        {
            monsterUnique = true;
            try
            {   
                //Determine if the monster is unique
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
                else //Null name assumes form of non-unique
                {
                    monsterUnique = false;
                }


                if (editMode)
                {
                    if (!monsterUnique && selectMonster.Species != speciesBox.Text)
                        modifyButton.Enabled = false;
                    else
                        modifyButton.Enabled = true;
                }

                if (!monsterUnique)
                    addMonButton.Enabled = false;
                else
                    addMonButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured in speciesBox_TextChanged: " + ex);
            }
        }

    /**********
    * 
    * BUTTON CONTROLS
    * 
    **********/

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
                ClearForms();
            }
        }

        private void populateButton_Click(object sender, EventArgs e)
        {
            //Create a reference to the loaded monster inside the list
                //Use selectMonster to commit edits later on
            try
            {
                if (monsterBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a monster from the list.");
                }
                else
                {
                    //Use selectMonster as a reference to the monster (Edits, checking, etc.)
                    selectMonster = monsterList.Monster[monsterBox.SelectedIndex];

                    FillForm(selectMonster);

                    //Monster loaded, turn on edit mode
                    EditMode(true);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered when attempting to reference monster: " + ex);
            }

            RefreshMonsterBox(); //Refresh the monster box just in case something funky is happening

        }


        private void modifyButton_Click(object sender, EventArgs e)
        {
            if (selectMonster.Species != speciesBox.Text)
            {
                DialogResult confirm = MessageBox.Show(
                    String.Format("Are you sure you want to replace {0} with {1}?",
                    selectMonster.Species, speciesBox.Text),
                    "Replacing Monster",
                    MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    FillMonster(selectMonster);
                }

            }
            else
            {
                FillMonster(selectMonster);
                MessageBox.Show(String.Format("Monster {0} successfully modified.",selectMonster.Species));
                selectMonster = null;
            }

            RefreshMonsterBox();
            EditMode(false);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (monsterBox.SelectedIndex >= 0)
                    selectMonster = monsterList.Monster[monsterBox.SelectedIndex];

                monsterList.Monster.RemoveAt(monsterBox.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred during monster delete attempt: " + ex);
            }

            RefreshMonsterBox();

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearForms();
        }

        
        private void buttonPaste_Click(object sender, EventArgs e)
        {
            string pastedText;
            using (GetPastedTextForm form = new GetPastedTextForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    
                }
                pastedText = form.getText;
            }

            if (pastedText != "")
            {
                Monster tempMon = new Monster(pastedText);
                FillForm(tempMon);
                tempMon = null;
            }
        }

    /**********
    * 
    * MENU STRIP TOOLS
    * 
    **********/

        //
        //  OpenToolStripMenuItem
        /// <summary>
        /// Loads monsters into the monsterList from a JSON file
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool confirmed = true; //Confirmation to proceed (in case of existing list)

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

                //If the result is yes
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        /// saveToolStripMenuItem
        /// <summary>
        /// Exports the monsterList into a JSON file at user-specified location
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Json Files (*.json)|*.json";
            saveFileDialog1.Title = "Export to JSON";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                try
                {
                    File.WriteAllText(Path.GetFullPath(saveFileDialog1.FileName),
                                JsonConvert.SerializeObject(monsterList, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
                }

                ClearForms();
            }
        }



    /**********
    * 
    * CUSTOM HELPER FUNCTIONS
    * 
    **********/

        private void ClearForms()
        {
            foreach (Control control in groupScores.Controls)
                Utilities.ResetControls(control, 8);

            foreach (Control control in groupCharacter.Controls)
                Utilities.ResetControls(control, 0);

            foreach (Control control in groupMisc.Controls)
                Utilities.ResetControls(control, 0);

            foreach (Control control in groupStats.Controls)
                Utilities.ResetControls(control, 1);

            foreach (Control control in groupTraits.Controls)
                Utilities.ResetControls(control, 0);

            skillsBox.Text = "";
            senseBox1.Text = "";
            langBox.Text = "";
            actioBox1.Text = "";
            abiliBox1.Text = "";
            legenBox1.Text = "";
            EditMode(false);

        }

        private void EditMode(bool on)
        {
            if (on)
            {
                editMode = true;
                addMonButton.Enabled = false;
                modifyButton.Enabled = true;
                monsterBox.Enabled = false;

            }
            else
            {
                editMode = false;
                modifyButton.Enabled = false;
                monsterBox.Enabled = true;
                selectMonster = null;

                if (!string.IsNullOrWhiteSpace(speciesBox.Text))
                    addMonButton.Enabled = true;
            }

        }

        private void RefreshMonsterBox()
        {
            //Get rid stale data
            monsterBox.Items.Clear();

            if (monsterList.Monster.Count > 0)
            {
                //Load in fresh data from the monster list
                foreach (Monster mon in monsterList.Monster)
                {
                    monsterBox.Items.Add(mon.Species);
                }

                //Allow deletion and population
                deleteButton.Enabled = true;
                populateButton.Enabled = true;
            }
            else
            {
                //Population and deletion are disabled
                deleteButton.Enabled = false;
                populateButton.Enabled = false;
            }

            monsterTextBox.Text = "";
            monsterJSONBox.Text = "";
        }

        
        private void FillMonster(Monster workingMonster)
        {

            workingMonster.Species = speciesBox.Text;
            workingMonster.Challenge = challengeInput.Text;
            workingMonster.Size = sizeInput.Text;
            workingMonster.Type = typeBox.Text;
            workingMonster.Alignment = alignInput.Text;
            workingMonster.ArmorClass = armorBox.Text;
            workingMonster.Speed = speedBox.Text;
            workingMonster.DamageVulnerability = damVulBox1.Text;
            workingMonster.DamageResistance = damResBox1.Text;
            workingMonster.ConditionImmunity = conImmBox1.Text;
            workingMonster.DamageImmunity = damImmBox1.Text;
            workingMonster.Languages = langBox.Text;
            workingMonster.PageNumber = (int)pageInput.Value;
            workingMonster.HitDie = hitDInput.Text;
            workingMonster.HitDieNum = (int)hitDNumInput.Value;
            workingMonster.Strength = (int)strInput.Value;
            workingMonster.Dexterity = (int)dexInput.Value;
            workingMonster.Constitution = (int)conInput.Value;
            workingMonster.Intelligence = (int)intInput.Value;
            workingMonster.Wisdom = (int)wisInput.Value;
            workingMonster.Charisma = (int)chaInput.Value;
            workingMonster.StrengthSave = strCheck.Checked;
            workingMonster.DexteritySave = dexCheck.Checked;
            workingMonster.ConstitutionSave = conCheck.Checked;
            workingMonster.WisdomSave = wisCheck.Checked;
            workingMonster.CharismaSave = chaCheck.Checked;
            workingMonster.IntelligenceSave = intCheck.Checked;
            getSenses(senseBox1.Text, ref workingMonster);
            getSkills(skillsBox.Text, ref workingMonster);
            workingMonster.Abilities = abiliBox1.Text;
            workingMonster.Actions = actioBox1.Text;
            workingMonster.LegendaryActions = legenBox1.Text;

        }

        private void FillForm(Monster inputMonster)
        {

           speciesBox.Text = inputMonster.Species;
           challengeInput.Text = inputMonster.Challenge;
           sizeInput.Text = inputMonster.Size;
           typeBox.Text = inputMonster.Type;
           alignInput.Text = inputMonster.Alignment;
           armorBox.Text = inputMonster.ArmorClass;
           speedBox.Text = inputMonster.Speed;
           damVulBox1.Text = inputMonster.DamageVulnerability;
           damResBox1.Text = inputMonster.DamageResistance;
           conImmBox1.Text = inputMonster.ConditionImmunity;
           damImmBox1.Text  = inputMonster.DamageImmunity;
           langBox.Text = inputMonster.Languages;
           pageInput.Value = inputMonster.PageNumber;
           hitDInput.Text = inputMonster.HitDie;
           hitDNumInput.Value = inputMonster.HitDieNum;
           strInput.Value = inputMonster.Strength;
           dexInput.Value = inputMonster.Dexterity;
           conInput.Value = inputMonster.Constitution;
           intInput.Value = inputMonster.Intelligence;
           wisInput.Value = inputMonster.Wisdom;
           chaInput.Value = inputMonster.Charisma;
           strCheck.Checked = inputMonster.StrengthSave;
           dexCheck.Checked = inputMonster.DexteritySave;
           conCheck.Checked = inputMonster.ConstitutionSave;
           wisCheck.Checked = inputMonster.WisdomSave;
           chaCheck.Checked = inputMonster.CharismaSave;
           intCheck.Checked = inputMonster.IntelligenceSave;
           senseBox1.Text = string.Join(", ",inputMonster.Senses.ToArray());
           skillsBox.Text = skillsToText(inputMonster.Skills);
           abiliBox1.Text = inputMonster.Abilities;
           actioBox1.Text = inputMonster.Actions;
           legenBox1.Text = inputMonster.LegendaryActions;

        }


    /**********
    * 
    * NUMERICUPDOWN ENTRY HELPERS
    * 
    **********/

        private void strInput_Enter(object sender, EventArgs e)
        {
            strInput.Select(0, strInput.Text.Length);
        }

        private void dexInput_Enter(object sender, EventArgs e)
        {
            dexInput.Select(0, dexInput.Text.Length);
        }

        private void conInput_Enter(object sender, EventArgs e)
        {
            conInput.Select(0, conInput.Text.Length);
        }

        private void intInput_Enter(object sender, EventArgs e)
        {
            intInput.Select(0, intInput.Text.Length);
        }

        private void wisInput_Enter(object sender, EventArgs e)
        {
            wisInput.Select(0, wisInput.Text.Length);
        }

        private void chaInput_Enter(object sender, EventArgs e)
        {
            chaInput.Select(0, chaInput.Text.Length);
        }

        private void hitDNumInput_Enter(object sender, EventArgs e)
        {
            hitDNumInput.Select(0, hitDNumInput.Text.Length);
        }

        private void xpInput_Enter(object sender, EventArgs e)
        {
            xpInput.Select(0, xpInput.Text.Length);
        }

        private void pageInput_ValueChanged(object sender, EventArgs e)
        {
            pageInput.Select(0, pageInput.Text.Length);
        }

        private void getSkills(string text, ref Monster mon)
        {
            if (text == "")
                return;

            if(mon.Skills.Count != 0)
                mon.Skills.Clear();

            text += "\n";

            List<string> stats = new List<string> { "STR", "DEX", "CON", "INT", "WIS", "CHA" };

            //Used for ToTitleCase
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            string temp;
            int tempNum;

            //Loop until no more ',' in skills
            while (text.Contains(','))
            {
                try
                {
                    temp = text.Substring(0, text.IndexOf('+') - 1);
                    temp.TrimStart();
                    text = text.Remove(0, text.IndexOf('+') + 1);
                    tempNum = Convert.ToInt32(text.Substring(0, text.IndexOf(',')));
                    mon.Skills.Add(temp, tempNum);
                    text = text.Remove(0, text.IndexOf(',') + 1);
                    text = text.TrimStart();
                }
                #pragma warning disable 0168
                catch (ArgumentOutOfRangeException e)
                #pragma warning restore 0168
                {
                    text = text.Remove(0, text.IndexOf(',') + 1);
                }

            }

            //Get Last Skill
            try
            {
                temp = text.Substring(0, text.IndexOf('+') - 1);
                text = text.Remove(0, text.IndexOf('+') + 1);
                tempNum = Convert.ToInt32(text.Substring(0, text.IndexOf('\n')));
                mon.Skills.Add(temp, tempNum);
            }
            #pragma warning disable 0168
            catch (ArgumentOutOfRangeException e)
            #pragma warning restore 0168
            {
                temp = "BadInput"; ;
                tempNum = 0;
                mon.Skills.Add(temp, tempNum);
            }


            return;
        }

        private void getSenses(string text, ref Monster mon)
        {
            if (text == "")
                return;

            if(mon.Senses.Count != 0)
                mon.Senses.Clear();

            text += "";

            string substr = text;

            //Used for ToTitleCase
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            string temp;

            //Loop until no more ',' in senses
            while (substr.Contains(','))
            {
                temp = substr.Substring(0, substr.IndexOf(',') - 1);
                temp.Trim();
                mon.Senses.Add(temp);
                substr = substr.Remove(0, substr.IndexOf(',') + 1);
                substr = substr.Trim();

            }

            //Add Last Sense
            mon.Senses.Add(substr);
        }

        //SKILLS TO TEXT HELPER
        private string skillsToText(Dictionary<string, int> skills)
        {
            if (skills.Count == 0)
                return "";

            string text = "";
            foreach (KeyValuePair<string, int>s in skills)
            {
                text += s.Key + " +" + s.Value + ',';
            }
            text = text.Remove(text.Length - 1);

            return text;
        }

        //END OF TEXT TO MONSTER HELPER FUNCTIONS
        //***************************************




    }

    public class Utilities
    {
        //Neat trick for clearing forms
        public static void ResetControls(Control control, int defaultValue)
        {
            if (control is TextBox)
            {
                TextBox textBox = (TextBox)control;
                textBox.Text = null;
            }

            if (control is ComboBox)
            {
                ComboBox comboBox = (ComboBox)control;
                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }

            if (control is CheckBox)
            {
                CheckBox checkBox = (CheckBox)control;
                checkBox.Checked = false;
            }

            if (control is NumericUpDown)
            {
                NumericUpDown numWheel = (NumericUpDown)control;
                numWheel.Value = defaultValue;
            }
        }


    }
}