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
        public string Senses;
        public string Actions;
        public string LegendaryActions;
        public string Skills;


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
