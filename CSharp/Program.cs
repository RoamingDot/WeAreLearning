using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DeeEnDee
{
    class Program
    {
        static int Main(string[] args)
        {
            XElement root = XElement.Load("Monster_List.xml");
            XNamespace msp = "MSP";

            IEnumerable<XElement> elem = from el in root.Elements(msp + "Monster")
                                         where (string)el.Attribute(msp + "Type") == "Ape"
                                         select el;
            
            foreach (XElement el in elem)
                Console.WriteLine(el);

            return 0;
        }
    }
}
