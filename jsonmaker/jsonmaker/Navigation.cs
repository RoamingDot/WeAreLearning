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
            foreach (Monster mon in monsterList.Monster)
            {
                if (String.Equals(monsterBox.GetItemText(monsterBox.SelectedItem), mon.Species))
                {

                    //Puts JSON into JSON box tab
                    string json = JsonConvert.SerializeObject(mon, Formatting.Indented);
                    monsterJSONBox.Text = json;

                    //Fill box with Monsters
                    monsterTextBox.Text = mon.ToString();

                }
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
            catch
            {
                // If there is an error, display the text using the system colors (?)
                speciesBox.ForeColor = SystemColors.ControlText;
            }
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
                ClearForms();
            }
        }

        private void populateButton_Click(object sender, EventArgs e)
        {
            //Create a reference to the loaded monster inside the list
                //Use selectMonster to commit edits later on
            foreach (Monster mon in monsterList.Monster)
            {
                if (String.Equals(monsterBox.GetItemText(monsterBox.SelectedItem), mon.Species))
                {
                    //Don't bother assigning if the name is somehow null
                    if (!string.IsNullOrWhiteSpace(mon.Species))
                    {
                        selectMonster = mon;

                        FillForm(selectMonster);

                        //Monster loaded, turn on edit mode
                        EditMode(true);
                    }

                    //Grab only one (first) monster; don't keep going and redo the work for duplicates
                    break;
                }
            }

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


        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearForms();
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

            }
            else
            {
                editMode = false;
                addMonButton.Enabled = true;
                modifyButton.Enabled = false;
                selectMonster = null;
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
            workingMonster.Senses = senseBox1.Text;
            workingMonster.Skills = skillsBox.Text;
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
           senseBox1.Text = inputMonster.Senses;
           skillsBox.Text = inputMonster.Skills;
           abiliBox1.Text = inputMonster.Abilities;
           actioBox1.Text = inputMonster.Actions;
           legenBox1.Text = inputMonster.LegendaryActions;

        }

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