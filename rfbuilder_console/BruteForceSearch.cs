using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rfbuilder_console
{
    public class BruteForceSearch
    {
        public static string bruteresult { get; set; }

        public static Element SystemSwitch { get; set; }
        public static Element SystemLNA { get; set; }
        public static Element SystemMixer { get; set; }
        public static Element SystemFilter { get; set; }

        public static double systemgain { get; set; }

        public static double systemnoise { get; set; }

        public static double systemcost { get; set; }




        static public void BruteForce()
        {
            List<RfSystem> AllList = new List<RfSystem>();

            List<RfSystem> BruteForceCheck = new List<RfSystem>();

            foreach (Element _switch in ElementStore.Switches)
            {
                foreach (Element _lna in ElementStore.LNAs)
                {
                    foreach (Element _mixer in ElementStore.Mixers)
                    {
                        foreach (Element _filter in ElementStore.Filters)
                        {
                            RfSystem СurrentBruteSystem = new RfSystem(_switch, _lna, _mixer, _filter);

                            RfSystem AllListSystem = new RfSystem(_switch, _lna, _mixer, _filter);

                            AllList.Add(AllListSystem);

                            if (СurrentBruteSystem.TotalGain() >= MainBody.UserGain && СurrentBruteSystem.TotalNoise() <= MainBody.UserNoise)
                            {
                                BruteForceCheck.Add(item: new RfSystem(_switch, _lna, _mixer, _filter));
                            }
                        }
                    }
                }
            }         
            if (BruteForceCheck.Count == 0)
            {
                Console.WriteLine("Systems not found");
            }
            else
            {
              Console.WriteLine("==============Итого в работе =========" + AllList.Count);           
              Console.WriteLine("Количество найденных подходящих систем  === " + BruteForceCheck.Count);
              BruteForceCheck = BruteForceCheck.OrderBy(x => x.TotalCost()).ToList();
              Console.WriteLine("***************************************************");
              Console.WriteLine("Самая дешевая система по заданным условиям используя Полный перебор ");
              Console.WriteLine(BruteForceCheck[0].SystemDescription());
              Console.WriteLine("Усиление = " + BruteForceCheck[0].TotalGain() + " дБ ");
              Console.WriteLine("Шум = " + BruteForceCheck[0].TotalNoise() + " дБ ");
              Console.WriteLine("Цена = " + BruteForceCheck[0].TotalCost() + " $ ");
              bruteresult = (BruteForceCheck[0].SystemDescription());

                systemgain = BruteForceCheck[0].TotalGain();
                systemnoise = BruteForceCheck[0].TotalNoise();
                systemcost = BruteForceCheck[0].TotalCost();

               SystemSwitch = BruteForceCheck[0].SystemSwitch();

                SystemLNA = BruteForceCheck[0].SystemLNA();

                SystemMixer = BruteForceCheck[0].SystemMixer();

                SystemFilter = BruteForceCheck[0].SystemFilter();

            }
        }
    }
}

