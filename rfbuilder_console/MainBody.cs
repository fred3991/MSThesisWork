using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using System.IO;


namespace rfbuilder_console
{
    public class MainBody
    {
        public static double UserGain { get; set; }
        public static double UserNoise { get; set; }
        public static int Mutation { get; set; }
        public static int PopulationSize { get; set; }
        public static int Iteration { get; set; }
        static public void Main(string[] args)
        {
            {           
                BruteForceSearch.BruteForce(); //Инициализация полного перебора систем
                GeneticAlgorithm.PopulationSize = PopulationSize;
                GeneticAlgorithm.IterationNumber = Iteration;
                GeneticAlgorithm.GeneticSearch();
            }

        }      
    }    
}

