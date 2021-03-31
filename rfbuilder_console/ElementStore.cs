using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;


namespace rfbuilder_console
{
    public class ElementStore
    {
        

        public static List<Element> Switches { get; private set; } 
        public static List<Element> LNAs { get; private set;  }   
        public static List<Element> Mixers { get; private set;  }  
        public static List<Element> Filters { get; private set;  }

     

        static ElementStore()
        {
            Switches = ElementsFileReader.Read("Switches.txt");
            LNAs = ElementsFileReader.Read("LNA.txt");
            Mixers  = ElementsFileReader.Read("Mixers.txt");
            Filters   = ElementsFileReader.Read("Filters.txt");
        }

        //public static void RefreshStore(int ElementNumber)
        //{
        //    Switches = ElementsFileReader.Read("Switches_" + ElementNumber + ".txt");
        //    LNAs = ElementsFileReader.Read("LNA_" + ElementNumber + ".txt");
        //    Mixers = ElementsFileReader.Read("Mixers_" + ElementNumber + ".txt");
        //    Filters = ElementsFileReader.Read("Filters_" + ElementNumber + ".txt");


        //}
    }
}


    
