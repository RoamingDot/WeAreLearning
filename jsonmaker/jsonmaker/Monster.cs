using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace jsonmaker
{

    public class RootObject
    {
        public List<Monster> Monster { get; set; }

    }


    public struct Monster
    {

        public string Species = "";
        public string Challenge = "";
        public string Size = "";
        public string Type = "";
        public string Race = "";
        public string Alignment = "";
        public string ArmorClass = "";
        public string Speed = "";
        public string DamageVulnerability = "";
        public string DamageResistance = "";
        public string ConditionImmunity = "";
        public string DamageImmunity = "";
        public string Languages = "";
        public int PageNumber = 0;
        public int HitDie = 0;
        public int HitDieNum = 0;
        public int Strength = 0;
        public int Dexterity = 0;
        public int Constitution = 0;
        public int Intelligence = 0;
        public int Wisdom = 0;
        public int Charisma = 0;
        public bool StrengthSave = false;
        public bool DexteritySave = false;
        public bool ConstitutionSave = false;
        public bool IntelligenceSave = false;
        public bool WisdomSave = false;
        public bool CharismaSave = false;
        public List<string> Senses;
        public List<string> Actions;
        public List<string> LegendaryActions;
        public Dictionary<string, int> Skills;
       

  /*      public override string ToString()
        {
            List<PropertyDescriptor> easyProperties = (PropertyDescriptor property in TypeDescriptor.GetProperties(this) )
            foreach ()
            {


            }
            return temp;
        }*/
    }
}
