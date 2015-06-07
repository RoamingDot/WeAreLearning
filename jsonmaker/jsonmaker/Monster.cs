using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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
        public List<Monster> Monster { get; set; }

    }

/*********************
* Monster Struct
* 
* Holds a monster's possible stats
* 
* *******************/
    public struct Monster
    {
        
        public string Species;
        public string Challenge;
        public string Size;
        public string Type;
        public string Race;
        public string Alignment;
        public string ArmorClass;
        public string Speed;
        public string DamageVulnerability;
        public string DamageResistance;
        public string ConditionImmunity;
        public string DamageImmunity;
        public string Languages;
        public int PageNumber;
        public int HitDie;
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
        public List<string> Senses;
        public List<string> Actions;
        public List<string> LegendaryActions;
        public Dictionary<string, int> Skills;

    /* *********************************************
    * string Monster.ToString()
    * 
    * Returns a string with all struct member names and values on multiple lines.
    * 
    * TODO: Figure out why it doesn't automatically convert to string on assignment.
    * 
    ***********************************************/
        public override string ToString()
        {
            string temp = "";

            //Grab all the fields in _this.
                //Loop through them and print out the non-collection types
            foreach (var field in this.GetType().GetFields())
            {
                if (!field.FieldType.IsGenericType)
                    temp += field.Name + ": " + field.GetValue(this) + Environment.NewLine;
            }


            //Lists and Dictionaries may be individually uninitialized
                //Check and print SENSES
            if (Senses != null)
            {
                temp += "Senses: " + Senses.Count() + Environment.NewLine;
                foreach (string s in Senses)
                    temp += "  " + s + Environment.NewLine;
            }

                //Check and print ACTIONS
            if (Actions != null)
            {
                temp += "Actions: " + Actions.Count() + Environment.NewLine;
                foreach (string s in Actions)
                    temp += "  " + s + Environment.NewLine;
            }

                //Check and print LEGENDARYACTIONS
            if (LegendaryActions != null)
            { 
                temp += "Legendary Actions: " + LegendaryActions.Count() + Environment.NewLine;
                foreach (string s in LegendaryActions)
                    temp += "  " + s + Environment.NewLine;
            }
            //Check and print SKILLS
            if (Skills != null)
            { 
                temp += "Skills: " + Skills.Count() + Environment.NewLine;
                foreach (KeyValuePair<string, int> s in Skills)
                    temp += String.Format("  {0} +{1}{2}", s.Key, s.Value, Environment.NewLine);
            }

            return temp;
        }

    }
}
