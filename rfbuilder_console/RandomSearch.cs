using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rfbuilder_console
{
    class RandomSearch
    {
        static public void TryRandomSearch()
        {
            var Random = new Random();
            List<RfSystem> RandomSearchList = new List<RfSystem>();
            Console.WriteLine("Поиск случайным образом ");        
            for (int i = 0; i <115980; i++)
            {
                int random_index_switch = Random.Next(0, ElementStore.Switches.Count);
                int random_index_lna = Random.Next(0, ElementStore.LNAs.Count);
                int random_index_mixer = Random.Next(0, ElementStore.Mixers.Count);
                int random_index_filter = Random.Next(0, ElementStore.Filters.Count);

                RfSystem RandomSystem = new RfSystem(
                    ElementStore.Switches[random_index_switch],
                    ElementStore.LNAs[random_index_lna],
                    ElementStore.Mixers[random_index_mixer],
                    ElementStore.Filters[random_index_filter]  );

                if (RandomSystem.TotalGain() > MainBody.UserGain && RandomSystem.TotalNoise() < MainBody.UserNoise)
                {
                    RandomSearchList.Add(new RfSystem(
                        ElementStore.Switches[random_index_switch],
                        ElementStore.LNAs[random_index_lna],
                        ElementStore.Mixers[random_index_mixer],
                        ElementStore.Filters[random_index_filter]
                                                       ));                  
                }               
            }
            if (RandomSearchList.Count() > 0)
            {
                RandomSearchList = RandomSearchList.OrderBy(x => x.TotalCost()).ToList();              
                 // Console.WriteLine("*************Результат рандомного поиска**********************");
                  Console.WriteLine("Подходящие системы, количество = " + RandomSearchList.Count);
                 // Console.WriteLine("Самая дешевая система по заданным условиям");
                  Console.WriteLine(RandomSearchList[0].SystemDescription());
                 // Console.WriteLine("Усиление = " + RandomSearchList[0].TotalGain() + " дБ ");
                 // Console.WriteLine("Шум = " + RandomSearchList[0].TotalNoise() + " дБ ");
                  //Console.WriteLine("Цена = " + RandomSearchList[0].TotalCost() + " $ ");                                 
            }
            else
            {
                Console.WriteLine("Число найденных систем =" + RandomSearchList.Count);
                Console.WriteLine( "Видимо не повезло, испытай удачу еще раз!"  );
            }           
           
        }
    }
}