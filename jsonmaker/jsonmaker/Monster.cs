using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace jsonmaker
{
/*********************
 * RootObject Class
 * 
 * Used to hold the root object of "Monster" (?)
 * 
 * *******************/
    public class RootObject
    {
        public RootObject()
        {
            Monster = new List<Monster>();
        }
        public List<Monster> Monster { get; set; }

    }

/*********************
* Monster Struct
* 
* Holds a monster's possible stats
* 
* *******************/
    public class Monster
    {        
        public string Species;
        public string Challenge;
        public string Size;
        public string Type;
        public string Alignment;
        public string ArmorClass;
        public string Speed;
        public string DamageVulnerability;
        public string DamageResistance;
        public string ConditionImmunity;
        public string DamageImmunity;
        public string Languages;
        public int PageNumber;
        public string HitDie;
        public int HitDieNum;
        public int Strength;
        public int Dexterity;
        public int Constitution;
        public int Intelligence;
        public int Wisdom;
        public int Charisma;
        public bool StrengthSave;
        public bool DexteritySave;
        public bool ConstitutionSave;
        public bool IntelligenceSave;
        public bool WisdomSave;
        public bool CharismaSave;
        public string Abilities;
        public List<string> Senses;
        public string Actions;
        public string LegendaryActions;
        public Dictionary<string, int> Skills;

        public Monster()
        {
            Senses = new List<string>();
            Skills = new Dictionary<string , int>();
        }

        public Monster(string text)
        {
            Senses = new List<string>();
            Skills = new Dictionary<string, int>();
            getMonster(ref text);
        }

        public Monster getMonster(ref string text)
        {
            this.Species = getSpecies(ref text);
            this.Size = getSize(ref text);
            this.Type = getType(ref text);
            this.Alignment = getAlignemnt(ref text);
            this.ArmorClass = getArmorClass(ref text);
            this.HitDieNum = getNumHitDie(ref text);
            this.HitDie = getHitDie(ref text);
            this.Speed = getSpeed(ref text);
            getAllStats(ref text, this);
            getAllSaves(ref text, this);
            getAllSkills(ref text, this);
            this.DamageVulnerability = getDamageVulnerabilities(ref text);
            this.DamageResistance = getDamageResistances(ref text);
            this.DamageImmunity = getDamageImmunities(ref text);
            this.ConditionImmunity = getConditionImmunities(ref text);
            getAllSenses(ref text, this);
            this.Languages = getLanguages(ref text);
            this.Challenge = getChallenge(ref text);

            return this;
        }

        /****************************************
             * 
             * TEXT TO MONSTER HELPER FUNCTIONS
             * 
             ****************************************/


        private string getSpecies(ref string text)
        {
            List<string> sizes = new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Gargantuan" };


            //Text Blob
            text = text.Trim();
            string substr = text;
            int indexOfSize = 0;
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
                    indexOfSize = text.IndexOf(s);
                }
            }


            //Get Species in monster if it exists
            if (text.Substring(0, indexOfSize).Length != 0)
            {
                substr = text.Substring(0, text.IndexOf("\n"));

                //Remove Species text and whitespace
                text = text.Remove(0, indexOfSize);
                text = text.Trim();

                return textInfo.ToTitleCase(substr.ToLower());

            }
            else
                return "";
        }




        private string getSize(ref string text)
        {
            string substr = text;
            List<string> sizes = new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Gargantuan" };

            //Get Size
            substr = text.Substring(0, text.IndexOf(' '));

            //Removes Size from text
            text = text.Remove(0, substr.Length);
            text = text.Trim();

            if (sizes.Any(s => s.Equals(substr, StringComparison.OrdinalIgnoreCase)))
            {
                return substr;
            }
            else
                return "";

        }

        private string getType(ref string text)
        {
            string substr = text;

            //Used for ToTitleCase
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            //Get Type
            substr = text.Substring(0, text.IndexOf(','));

            //Remove Type and ',' from text
            text = text.Remove(0, substr.Length + 1);
            text = text.Trim();

            return textInfo.ToTitleCase(substr.ToLower());
        }

        private string getAlignemnt(ref string text)
        {
            string substr = text;

            List<string> alignments = new List<string>{"Unaligned", "Lawful Good", "Lawful Neutral", "Lawful Evil", "Neutral Good", "True Neutral",
                "Neutral Evil", "Chaotic Good", "Chaotic Neutral", "Chaotic Evil"};

            //Used for ToTitleCase
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            //Get Alignment
            substr = text.Substring(0, text.IndexOf('\n'));
            substr = substr.Trim();

            //Removes Alignment from text
            text = text.Remove(0, substr.Length);
            text = text.Trim();
            

            if (alignments.Any(s => s.Equals(substr, StringComparison.OrdinalIgnoreCase)))
            {
                return textInfo.ToTitleCase(substr.ToLower());
            }
            else
                return "";
        }

        private string getArmorClass(ref string text)
        {
            string substr = text;

            //Used for ToTitleCase
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            //Get Armor Class
            substr = text.Substring(0, text.IndexOf('\n'));

            //Remove Armor Class from text
            text = text.Remove(0, substr.Length);
            text = text.Trim();


            return textInfo.ToTitleCase(substr.ToLower());

        }

        private int getNumHitDie(ref string text)
        {
            string substr = text;

            //Remove average health from text
            text = text.Remove(0, text.IndexOf('('));

            //Get Number of Hit Die
            substr = text.Substring(1, text.IndexOf('d') - 1);

            //Remove Number of Hit Die  and '(' from text
            text = text.Remove(0, substr.Length + 1);

            return Convert.ToInt32(substr);
        }


        private string getHitDie(ref string text)
        {
            string substr = text;

            //Get Hit Die
            substr = text.Substring(0, text.IndexOf(' '));

            //Remove Hit Die from string
            text = text.Remove(0, text.IndexOf('\n'));
            text = text.Trim();

            return substr;

        }

        private string getSpeed(ref string text)
        {
            string substr = text;

            //Get Speed
            substr = text.Substring(0, text.IndexOf('\n'));

            //Remove Speed from string
            substr = substr.Replace("Speed ", "");
            substr = substr.Trim();

            //Remove Speed from text
            text = text.Remove(0, text.IndexOf('\n'));
            text = text.Trim();

            return substr;
        }

        private void getAllStats(ref string text, Monster mon)
        {
            string substr;
            List<string> stats = new List<string> { "STR", "DEX", "CON", "INT", "WIS", "CHA" };

            //Gets all text before actions
            //because stats arent in order
            substr = text.Substring(0, text.IndexOf("CHA") + 11);

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

                try
                {
                    text = text.Remove(text.IndexOf(s), text.IndexOf(')') - text.IndexOf(s) + 1);
                }
                #pragma warning disable 0168
                catch(ArgumentOutOfRangeException e)
                #pragma warning restore 0168
                {
                    int index = 0;

                    for (int i = 0; i <= 10; i++)
                    {
                        try
                        {
                            if(index == 0 )
                                index = text.IndexOf('-' + Convert.ToString(i) + ")") + 1;
                            if(index == 0)
                            index = text.IndexOf('+' + Convert.ToString(i) + ")") + 1;
                        }
                        #pragma warning disable 0168
                        catch (ArgumentOutOfRangeException e2) { }
                        #pragma warning restore 0168
                    }
                    if(index == 0)
                        text.IndexOf(')');
                    text = text.Remove(text.IndexOf(s), index - text.IndexOf(s) + 2);
                }
            }
            text = text.Trim();

            return;
        }

        private void getAllSaves(ref string text, Monster mon)
        {
            string substr;
            List<string> stats = new List<string> { "Str", "Dex", "Con", "Int", "lnt", "Wis", "Cha" };


            //If Saving Throws Exists in text
            //Get Saving Throws
            if (text.Substring(0, 10).Contains("Saving"))
            {
                substr = text.Substring(0, text.IndexOf('\n'));

                //Get Saving Throws
                foreach (string s in stats)
                {
                    if (substr.Contains(s))
                    {
                        switch (s)
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
                            case "lnt":
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

                return;
            }
        }



        private void getAllSkills(ref string text, Monster mon)
        {
            string substr;
            List<string> stats = new List<string> { "STR", "DEX", "CON", "INT", "WIS", "CHA" };

            if(mon.Skills.Count != 0)
                mon.Skills.Clear();

            //Used for ToTitleCase
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            //If Skills Exist in text
            //Get Skills
            if (text.Substring(0, 10).Contains("Skills"))
            {

                //Remove "Skills " from text
                text = text.Replace("Skills ", "");

                //Get All Skills
                substr = text.Substring(0, text.IndexOf("Senses"));
                substr = substr.Substring(0, substr.LastIndexOf('+') + 2) + '\n';

                string temp;
                int tempNum;

                //Loop until no more ',' in skills
                while (substr.Contains(','))
                {
                    try
                    {
                        temp = substr.Substring(0, text.IndexOf('+') - 1);
                        temp.Trim();
                        substr = substr.Remove(0, substr.IndexOf('+') + 1);
                        tempNum = Convert.ToInt32(substr.Substring(0, substr.IndexOf(',')));
                        mon.Skills.Add(temp, tempNum);

                        substr = substr.Remove(0, substr.IndexOf(',') + 1);
                        substr = substr.TrimStart();
                        text = text.Remove(0, text.IndexOf(',') + 1);
                        text = text.Trim();
                        
                    }
                    #pragma warning disable 0168
                    catch (ArgumentOutOfRangeException e)
                    #pragma warning restore 0168
                    {
                        text = text.Remove(0, text.IndexOf(',') + 1);
                    }

                } while (substr.Contains(',')) ;


                //Get Last Skill
                try
                {
                    temp = substr.Substring(0, substr.IndexOf('+') - 1);
                    substr = substr.Remove(0, substr.IndexOf('+') + 1);
                    tempNum = Convert.ToInt32(substr.Substring(0, substr.IndexOf('\n')));
                    mon.Skills.Add(temp, tempNum);
                }
#pragma warning disable 0168
                catch (ArgumentOutOfRangeException e)
#pragma warning restore 0168
                {
                    temp = "BadInput";
                    tempNum = 0;
                    mon.Skills.Add(temp, tempNum);
                }

                //Remove Skills from text
                text = text.Remove(0, text.IndexOf('\n'));
                text = text.Trim();
            }
            return;
        }

        private string getDamageVulnerabilities(ref string text)
        {
            string substr = text;

            //If Damage Vulnerabilities Exist in Text
            //Get Damage Vulnerabilities
            if (text.Substring(0, 14).Contains("Damage V"))
            {
                text = text.Replace("Damage Vulnerabilities ", "");
                substr = text;

                //Damage Vulnerabilities may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select substr.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');
                substr = substr.Replace('\r', ' ');

                //Remove Damage Vulnerabilities from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();

                return substr;
            }
            return "";
        }

        private string getDamageResistances(ref string text)
        {
            string substr = text;

            //If Damage Resistances Exist in Text
            //Get Damage Resistances
            if (text.Substring(0, 10).Contains("Damage R"))
            {

                text = text.Replace("Damage Resistances ", "");
                substr = text;

                //Damage resistances may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select substr.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');
                substr = substr.Replace('\r', ' ');

                //Remove Armor Class from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();

                return substr;

            }
            return "";
        }

        private string getDamageImmunities(ref string text)
        {
            string substr = text;

            //If Damage Immunities Exist in Text
            //Get Damage Immunities
            if (text.Substring(0, 10).Contains("Damage I"))
            {
                text = text.Replace("Damage Immunities ", "");
                substr = text;
                //Damage Immunities may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select substr.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');
                substr = substr.Replace('\r', ' ');

                //Remove Damage Immunities from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();

                return substr;

            }
            return "";
        }

        private string getConditionImmunities(ref string text)
        {
            string substr = text;

            //If Condition Immunities Exist in Text
            //Get Condition Immunities
            if (text.Substring(0, 14).Contains("Condition I"))
            {
                text = text.Replace("Condition Immunities ", "");
                substr = text;

                //Condition Immunities may have new line char so
                //must make substr go until first upper case letter
                var index = from ch in text
                            where Char.IsUpper(ch)
                            select substr.IndexOf(ch);

                substr = text.Substring(0, Convert.ToInt32(index.First()));

                //Replace New lines with space
                substr = substr.Replace('\n', ' ');
                substr = substr.Replace('\r', ' ');

                //Remove Condition Immunities from string
                text = text.Remove(0, substr.Length);
                text = text.Trim();

                return substr;

            }
            return "";
        }
  

        private void getAllSenses(ref string text, Monster mon)
        {
            string substr = text;

            if(mon.Senses.Count != 0)
                mon.Senses.Clear();

            //If Senses Exist in Text
            //Get Senses
            if (text.Substring(0, 14).Contains("Senses"))
            {

                //Remove "Senses " from text
                text = text.Replace("Senses ", "");
                substr = text.Substring(0, text.IndexOf("Languages"));

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

                //Remove Senses from text
                text = text.Remove(0, text.IndexOf("Languages"));
                text = text.Trim();
            }
        }

        private string getLanguages(ref string text)
        {
            string substr = text;

            //Remove "Lanaguages " from text
            text = text.Replace("Languages ", "");

            //Get Languages
            substr = text.Substring(0, text.IndexOf("Challenge"));
            substr = substr.Trim();

            //Remove Languages substring from text
            text = text.Remove(0, substr.Length);
            text = text.Trim();

            return substr;
        }



        private string getChallenge(ref string text)
        {
            string substr = text;

            //Get Challenge Level
            text = text.Replace("Challenge ", "");
            substr = text.Substring(0, text.IndexOf('('));
            substr = substr.Trim();
            return substr;
        }


    /* *********************************************
    * string Monster.ToString()
    * 
    * Returns a string with all struct member names and values on multiple lines.
    * 
    * TODO: Figure out why it doesn't automatically convert to string upon assignment.
    * 
    ***********************************************/
        public override string ToString()
        {
            string temp = ""; //Return Value

            //Print member names and values of _this
            foreach (var field in this.GetType().GetFields())
            {
                if (!field.FieldType.IsGenericType) //Regular variables
                {
                    temp += field.Name + ": " + field.GetValue(this) + Environment.NewLine;
                }
                
            }
         
            return temp;
        }

    }


}
