using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rfbuilder_console
{
    public class Tinder
    {
        //public static int Mother { get; set; }
        public static int Around { get; set; }        
        private static Random rndround = new Random((int)DateTime.Now.Ticks);

        static public int LookAround(int mother)
        {


            Around = GeneticAlgorithm.SortedPopulation.Count ;
            
            int lowerlimit = Math.Max(0, mother - Around);
            int upperlimit = Math.Min(GeneticAlgorithm.SortedPopulation.Count, mother + Around);

            return rndround.Next(lowerlimit, upperlimit);

        }
    } 
}
