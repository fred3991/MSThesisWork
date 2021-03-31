using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


public class ElementsFileReader
{


    public static List<Element> Read( string filename)
    {       
        string[] File_Infos = File.ReadAllLines(filename);
        List<Element> ReadElements = new List<Element>();
        bool first_line = true;
        int index = 0;
        foreach (var line in File_Infos)
        {    
            if (first_line==true)
            {
                first_line = false;
                continue;
            }
            string[] Element_info = line.Split('\t');
            Element Naming = new Element()
            {
                name = Element_info[0],
                gain = Convert.ToDouble(Element_info[1], CultureInfo.InvariantCulture),
                noise = Convert.ToDouble(Element_info[2], CultureInfo.InvariantCulture),
                cost = Convert.ToDouble(Element_info[3], CultureInfo.InvariantCulture),
                index = index++
            } ;           
                ReadElements.Add(Naming);
        }        
        return ReadElements;
    }         
}
    


