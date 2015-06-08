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
        public List<string> Senses;
        public List<string> Actions;
        public List<string> LegendaryActions;
        public Dictionary<string, int> Skills;


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
                else if (field.FieldType == typeof(List<string>))
                {
                    //Case: List of Strings
                    List<string> subproperty = (List<string>)field.GetValue(this);

                    if (subproperty != null)
                    {
                        //Print name
                        temp += field.Name + ":" + Environment.NewLine;
                        //Print listed items underneath
                        foreach (string s in subproperty)
                            temp += "  " + s + Environment.NewLine;
                    }
                }
                else if(field.FieldType == typeof(Dictionary<string,int>))
                {
                    //Case: Dictionary<string, int>
                    Dictionary<string, int> subproperty = (Dictionary<string, int>)field.GetValue(this);

                    if (subproperty != null)
                    {
                        //Print Name
                        temp += field.Name + ":" + Environment.NewLine;
                        //Print listed items underneath
                        foreach (KeyValuePair<string, int> s in Skills)
                            temp += String.Format("  {0} +{1}{2}", 
                                            s.Key, s.Value, Environment.NewLine);
                    }
                }
                
            }
         
            return temp;
        }

    }
}
