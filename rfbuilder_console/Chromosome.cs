using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rfbuilder_console
{
    public class Chromosome
    {
        public int SwitchIndex { get; set; }
        public int LNAIndex { get; set; }
        public int MixerIndex { get; set; }
        public int FilterIndex { get; set; }

        public  Element SystemSwitch { get; set; }
        public  Element SystemLNA { get; set; }
        public  Element SystemMixer { get; set; }
        public  Element SystemFilter { get; set; }



        public  double ChromosomeTotalGain { get; set; }
        public  double ChromosomeTotalNoise { get; set; }
        public  double ChromosomeTotalCost { get; set; }
        public  string ChromosomeDescription { get; set; }
        public  string ChromosomeIndexes { get; set; }


        public double GainFitness { get; set; }
        public double NoiseFitness { get; set; }
        public double CostFitness { get; set; }
        public double PerfomanceFitness { get; set; }
     
         // Все методы ниже взяты по примеру из RfSystem
             

        // метод для пересчета
        public void GetRecall()
        {


            CostFitness = ChromosomeTotalCost;
            //Расчет фитнес функции
            if (MainBody.UserGain > ChromosomeTotalGain)
            {
                GainFitness = MainBody.UserGain - ChromosomeTotalGain;
            }
            else
            {
                GainFitness = 0;
            }
            if (MainBody.UserNoise < ChromosomeTotalNoise)
            {
                NoiseFitness = ChromosomeTotalNoise - MainBody.UserNoise;
            }
            else
            {
                NoiseFitness = 0; // 
            }
            PerfomanceFitness = GainFitness + NoiseFitness;
        }

        //Конструктор хромосомы с задаными индексами для детей
        public Chromosome(int SwitchIndex, int LNAIndex, int MixerIndex, int FilterIndex) 
        {
            this.SwitchIndex = SwitchIndex;
            this.LNAIndex = LNAIndex;
            this.MixerIndex = MixerIndex;
            this.FilterIndex = FilterIndex;

            RfSystem SystemChromosome = new RfSystem(ElementStore.Switches[SwitchIndex], ElementStore.LNAs[LNAIndex],
                                                    ElementStore.Mixers[MixerIndex], ElementStore.Filters[FilterIndex]);

         
            ChromosomeTotalGain = SystemChromosome.TotalGain();
            ChromosomeTotalNoise = SystemChromosome.TotalNoise();
            ChromosomeTotalCost = SystemChromosome.TotalCost(); // Есть и цена, и цена фитнесс - надо определиться и что то одно оставить
            ChromosomeDescription = SystemChromosome.SystemDescription(); //Запрос имен
            ChromosomeIndexes = SwitchIndex + " " + LNAIndex + " " + MixerIndex + " " + FilterIndex; //Запрос индексов

            SystemSwitch = SystemChromosome.SystemSwitch();
            SystemLNA = SystemChromosome.SystemLNA();
            SystemMixer = SystemChromosome.SystemMixer();
            SystemFilter = SystemChromosome.SystemFilter();

            GetRecall();     //Пересчет всегда, при создании хромосомы       
        }
           
    }
}
