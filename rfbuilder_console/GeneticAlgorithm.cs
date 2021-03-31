using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using rfbuilder_console;

namespace rfbuilder_console
{
    public class GeneticAlgorithm
    {
        public static Element SystemSwitch { get; set; }
        public static Element SystemLNA { get; set; }
        public static Element SystemMixer { get; set; }
        public static Element SystemFilter { get; set; }




        public static int PopulationSize { get; set; }

        public static int IterationNumber { get; set; }      

        public static List<Chromosome> SortedPopulation { get; set; }
      
       
        public static List<Chromosome> Initialization(int PopulationSize) // Метод создания первичной популяции
        {        
            List<Chromosome> Population = new List<Chromosome>(); //Лист первичной популяции        
            int seed = (int)DateTime.Now.Ticks;
            Random rnd = new Random(seed);
            for (int g = 0; g < PopulationSize; g++)
            {              
                int SwitchIndex = rnd.Next(ElementStore.Switches.Count); //Назначение случайных коэффициентов
                int LNAIndex = rnd.Next(ElementStore.LNAs.Count);
                int MixerIndex = rnd.Next(ElementStore.Mixers.Count);
                int FilterIndex = rnd.Next(ElementStore.Filters.Count);
                Population.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
            }
         return Population;
        }
        //Проверка на близняшество
        public static bool CheckTwins(Chromosome GenOne, Chromosome GenTwo) 
        {
            bool duplicat = false;
            if (GenOne.ChromosomeIndexes == GenTwo.ChromosomeIndexes)
            {
                duplicat = true;
            }          
            return duplicat;
        }
        //Удаление дубликатов
        public static List<Chromosome> DestroyDouble(List<Chromosome> SortedPopulation) 
        {
            SortedPopulation = SortedPopulation.OrderBy(x => x.ChromosomeIndexes).ToList();
            for (int c = 0; c < SortedPopulation.Count-1; c++)
            {               
                for (int h = c + 1; h < SortedPopulation.Count; h++)
                {
                    bool duplicat = CheckTwins(SortedPopulation[c], SortedPopulation[h]);
                    if (duplicat == true)
                    {
                        SortedPopulation.RemoveAt(h);
                        h = h - 1;
                    }                
                }              
            }          
            return SortedPopulation;
        }//Оставление гудфитнеса      
        public static List<Chromosome> GoodFitness(List<Chromosome> SortedPopulation) 
        {
           
            SortedPopulation = SortedPopulation.Where(x => x.PerfomanceFitness == 0).ToList();
            return SortedPopulation;
        }
        public static void ScreenData(List<Chromosome> ScrenData)
        {
            if (ScrenData.Count >= 1)
            {
                Console.WriteLine("Число решений = "+ScrenData.Count);
                Console.WriteLine(ScrenData[0].ChromosomeDescription);
            }
            else
            {
                Console.WriteLine("No solutions for this time...");
            }          
        }    
        public static List<Chromosome> Kindergarden(List<Chromosome> SortedPopulation)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);

            List<Chromosome> Children = new List<Chromosome>(PopulationSize);
            do
            {
                int i = rnd.Next(0, SortedPopulation.Count); //Скрещивание между лучшими особями  
                int j = rnd.Next(0, SortedPopulation.Count);

                Chromosome Mother = SortedPopulation[i];
                Chromosome Father = SortedPopulation[j];           
                int SwitchIndex;
                int LNAIndex;
                int MixerIndex;
                int FilterIndex;
                int caseSwitch = rnd.Next(1,9);
                switch (caseSwitch)
                {
                    case 1:
                        SwitchIndex = Mother.SwitchIndex;
                        LNAIndex = Mother.LNAIndex;
                        MixerIndex = Mother.MixerIndex;
                        FilterIndex = Father.FilterIndex;
                        Children.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
                        break;
                    case 2:
                        SwitchIndex = Mother.SwitchIndex;
                        LNAIndex = Father.LNAIndex;
                        MixerIndex = Mother.MixerIndex;
                        FilterIndex = Mother.FilterIndex;
                        Children.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
                        break;
                    case 3:
                        SwitchIndex = Mother.SwitchIndex;
                        LNAIndex = Father.LNAIndex;
                        MixerIndex = Mother.MixerIndex;
                        FilterIndex = Father.FilterIndex;
                        Children.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
                        break;
                    case 4:
                        SwitchIndex = Mother.SwitchIndex;
                        LNAIndex = Mother.LNAIndex;
                        MixerIndex = Father.MixerIndex;
                        FilterIndex = Father.FilterIndex;
                        Children.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
                        break;
                    case 6:
                        SwitchIndex = Mother.SwitchIndex;
                        LNAIndex = Father.LNAIndex;
                        MixerIndex = Father.MixerIndex;
                        FilterIndex = Father.FilterIndex;
                        Children.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
                        break;
                    case 7:
                        SwitchIndex = Father.SwitchIndex;
                        LNAIndex = Father.LNAIndex;
                        MixerIndex = Mother.MixerIndex;
                        FilterIndex = Mother.FilterIndex;
                        Children.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
                        break;
                    case 8:
                        SwitchIndex = Father.SwitchIndex;
                        LNAIndex = Father.LNAIndex;
                        MixerIndex = Mother.MixerIndex;
                        FilterIndex = Father.FilterIndex;
                        Children.Add(new Chromosome(SwitchIndex, LNAIndex, MixerIndex, FilterIndex));
                        break;                  
                }
                

            } while (Children.Count < PopulationSize);  
            
            foreach (Chromosome Child in Children)
            {
                int mutaion_random = rnd.Next(1, 101);                                                     
                if (mutaion_random <= MainBody.Mutation) 
                {                   
                    int caseMutationSwitch = rnd.Next(1, 5);
                    switch (caseMutationSwitch)
                    {
                        case 1:
                            Child.SwitchIndex = rnd.Next(0, ElementStore.Switches.Count);                         
                            break;
                        case 2:
                            Child.LNAIndex = rnd.Next(0, ElementStore.LNAs.Count);                           
                            break;
                        case 3:
                            Child.MixerIndex = rnd.Next(0, ElementStore.Mixers.Count);                          
                            break;
                        case 4:
                            Child.FilterIndex = rnd.Next(0, ElementStore.Filters.Count);
                            break;                         
                    }
                      Child.GetRecall(); 
                }
            }
            return Children;
        }
        public static void GeneticSearch() 
        {         

             SortedPopulation = Initialization(PopulationSize);

             SortedPopulation = SortedPopulation.OrderBy(x => x.PerfomanceFitness).ToList();


            for (int generation = 1; generation < IterationNumber+1; generation++)         
            {                                            
                 var Children = Kindergarden(SortedPopulation);

                 SortedPopulation.InsertRange(SortedPopulation.Count, Children);     
                
                 Children.Clear();

                 SortedPopulation = DestroyDouble(SortedPopulation);

                 SortedPopulation = SortedPopulation.OrderBy(x => x.PerfomanceFitness).ToList();


                SortedPopulation.RemoveRange(PopulationSize, SortedPopulation.Count-PopulationSize);

               
               
            }
            SortedPopulation = GoodFitness(SortedPopulation);
            SortedPopulation = SortedPopulation.OrderBy(x => x.ChromosomeTotalCost).ToList();
            //ScreenData(SortedPopulation);

            SystemSwitch = SortedPopulation[0].SystemSwitch;
            SystemLNA = SortedPopulation[0].SystemLNA;
            SystemMixer = SortedPopulation[0].SystemMixer;
            SystemFilter = SortedPopulation[0].SystemFilter;

            

        }
    }
}