using System;
using System.IO;
using System.Xml.Serialization;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace Serialization
{

    [Serializable]
    public class PADs
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        // стандартный конструктор без параметров
        public PADs()        
        { }
      public   PADs(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
        }
    }
  
    [Serializable]
    public class ICSketch
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int RFINX { get; set; }
        public int RFINY { get; set; }
        public int RFOUTX { get; set; }
        public int RFOUTY { get; set; }
        [XmlArrayItem("PADs")]
        public List<PADs> PADs { get; set; }   
        // стандартный конструктор без параметров
        public ICSketch()
        { }     
        public ICSketch(string name, int size, int widht, int height, int rfinx, int rfiny, int rfoutx, int rfouty, List<PADs> pads )
        {
            Name = name; //Название элемента
            Size = size; // размеры-площадь, или типа того
            Width = widht; // ширина
            Height = height; // высота
            RFINX = rfinx; //координаты входа ВЧ - Х
            RFINY = rfiny;//координаты входа ВЧ - У
            RFOUTX = rfoutx; //координаты выхода ВЧ - Х
            RFOUTY = rfouty;//координаты входа ВЧ - У
            PADs = pads;         //Лист с падами, названия и координата пада
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<PADs> Pads  = new List<PADs>();      
            
            PADs Power = new PADs("5Vdc", 32, 15);         
            PADs Gnd = new PADs("GND", 15, 30);
            PADs Control = new PADs("Control", 15, 30);
            PADs Additional = new PADs("Additional", 15, 30);

            Pads.Add(Power);
            Pads.Add(Control);
            Pads.Add(Additional);
            Pads.Add(Gnd);        

            // объект для сериализации
            ICSketch element = new ICSketch("Switch", 30, 25, 30, 15, 7, 30, 15, Pads); // Передача названия и всех параметров будщей картинки
            Console.WriteLine(element.Name + " Объект создан");      
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(ICSketch));
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("TestElement.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, element);
                Console.WriteLine("Объект сериализован");
            }

            // десериализация
            using (FileStream fs = new FileStream("TestElement.xml", FileMode.OpenOrCreate))
            {
                ICSketch Switch = (ICSketch)formatter.Deserialize(fs);
                Console.WriteLine("Объект десериализован");
                Console.WriteLine("Имя: {0} --- RFIN X: {1}", Switch.Name, Switch.Size, Switch.RFINX);
            }
            Console.ReadLine();
        }
    }
}