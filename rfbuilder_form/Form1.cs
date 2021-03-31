using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



namespace rfbuilder_form
{
    // работа формы - вывод графики здесь
    public partial class Form1 : Form
    {
        public  static List<Element> Switches { get { return  rfbuilder_console.ElementStore.Switches; } }
        public static List<Element> LNAs { get { return rfbuilder_console.ElementStore.LNAs; } }
        public static List<Element> Mixers { get { return rfbuilder_console.ElementStore.Mixers; } }
        public static List<Element> Filters { get { return rfbuilder_console.ElementStore.Filters; } }
        public static RfSystem CurrentSystem { get; set; }

        public static int total_widht { get; set; }
        public static int total_height { get; set; }


        //Сериализация -классы данных
        [Serializable]
        public class PADs
        {
            public string Name { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            // стандартный конструктор без параметров
            public PADs()
            { }
            public PADs(string name, int x, int y)
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
            public ICSketch(string name, int size, int widht, int height, int rfinx, int rfiny, int rfoutx, int rfouty, List<PADs> pads)
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


     
        public Form1()
        {
            InitializeComponent();
           
            comboBox1.DataSource = Switches;
            comboBox1.DisplayMember = "Name";

            comboBox2.DataSource = LNAs;
            comboBox2.DisplayMember = "Name";

            comboBox3.DataSource = Mixers;
            comboBox3.DisplayMember = "Name";

            comboBox4.DataSource = Filters;
            comboBox4.DisplayMember = "Name";



            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;


        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Element Switches = comboBox1.SelectedItem as Element;
            label8.Text = Switches.name;
            label1.Text = "Gain = "+Convert.ToString(Switches.gain)+ " dB";
            label2.Text = "NF = " + Convert.ToString(Switches.noise) + " dB";
            label3.Text = "Cost = " + Convert.ToString(Switches.cost) + " $";
        }

        void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Element LNAs = comboBox2.SelectedItem as Element;
            label9.Text = LNAs.name;

            label18.Text = "Gain = " + Convert.ToString(LNAs.gain) + " dB";
            label17.Text = "NF = " + Convert.ToString(LNAs.noise) + " dB";
            label4.Text = "Cost = " + Convert.ToString(LNAs.cost) + " $";

        }

        void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Element Mixers = comboBox3.SelectedItem as Element;
            label10.Text = Mixers.name;

            label21.Text = "Gain = " + Convert.ToString(Mixers.gain) + " dB";
            label20.Text = "NF = " + Convert.ToString(Mixers.noise) + " dB";
            label19.Text = "Cost = " + Convert.ToString(Mixers.cost) + " $";

        }

        void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Element Filters = comboBox4.SelectedItem as Element;
            label11.Text = Filters.name;

            label24.Text = "Gain = " + Convert.ToString(Filters.gain) + " dB";
            label23.Text = "NF = " + Convert.ToString(Filters.noise) + " dB";
            label22.Text = "Cost = " + Convert.ToString(Filters.cost) + " $";
        }



        //Методы расчетов


        public void SystemCalc()
        {
            CurrentSystem = new RfSystem(comboBox1.SelectedItem as Element, comboBox2.SelectedItem as Element, 
                                          comboBox3.SelectedItem as Element, comboBox4.SelectedItem as Element);
            
            label14.Text = "Усиление системы = "+Convert.ToString(CurrentSystem.TotalGain()) + " dB";
            label15.Text = "Шум системы = " + Convert.ToString(CurrentSystem.TotalNoise()) + " dB";
            label16.Text = "Цена системы = " + Convert.ToString(CurrentSystem.TotalCost()) + " $";          

        } // Расчет текущей системы

        private void BruteAlgo()
        {

            try
            {
                rfbuilder_console.MainBody.UserGain = double.Parse(maskedTextBox1.Text);
                rfbuilder_console.MainBody.UserNoise = double.Parse(maskedTextBox2.Text);

            }
            catch
            {
                groupBox3.Text = "Введите данные";
            }
            rfbuilder_console.BruteForceSearch.BruteForce(); //Запуск брутфорса

            //  string result = rfbuilder_console.BruteForceSearch.bruteresult;           

            groupBox3.Text = "Самая дешевая система, удволетворяющая требованиям (Полный перебор) :";

            label31.Text = rfbuilder_console.BruteForceSearch.SystemSwitch.name;

            label40.Text = "Gain = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemSwitch.gain) + " dB";
            label42.Text = "NF = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemSwitch.noise) + " dB";
            label43.Text = "Cost = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemSwitch.cost) + " $";


            label30.Text = rfbuilder_console.BruteForceSearch.SystemLNA.name;

            label41.Text = "Gain = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemLNA.gain) + " dB";
            label39.Text = "NF = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemLNA.noise) + " dB";
            label38.Text = "Cost = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemLNA.cost) + " $";



            label29.Text = rfbuilder_console.BruteForceSearch.SystemMixer.name;

            label37.Text = "Gain = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemMixer.gain) + " dB";
            label36.Text = "NF = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemMixer.noise) + " dB";
            label35.Text = "Cost = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemMixer.cost) + " $";

            label28.Text = rfbuilder_console.BruteForceSearch.SystemFilter.name;

            label34.Text = "Gain = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemFilter.gain) + " dB";
            label33.Text = "NF = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemFilter.noise) + " dB";
            label32.Text = "Cost = " + Convert.ToString(rfbuilder_console.BruteForceSearch.SystemFilter.cost) + " $";




            label14.Text = "Усиление системы = " + Convert.ToString(rfbuilder_console.BruteForceSearch.systemgain) + " dB";
            label15.Text = "Шум системы = " + Convert.ToString(rfbuilder_console.BruteForceSearch.systemnoise) + " dB";
            label16.Text = "Цена системы = " + Convert.ToString(rfbuilder_console.BruteForceSearch.systemcost) + " $";




        } //Полный перебор

        private void GeneticClick()
        {
            try
            {
                rfbuilder_console.MainBody.UserGain = double.Parse(maskedTextBox1.Text);
                rfbuilder_console.MainBody.UserNoise = double.Parse(maskedTextBox2.Text);

                rfbuilder_console.GeneticAlgorithm.PopulationSize = int.Parse(maskedTextBox4.Text);
                rfbuilder_console.GeneticAlgorithm.IterationNumber = int.Parse(maskedTextBox3.Text);


            }
            catch
            {
                groupBox3.Text = "Введите данные";
            }




            rfbuilder_console.GeneticAlgorithm.GeneticSearch(); //Запуск ген алгоритма

            for (int i = 0; i < 101; i++)
            {
                progressBar1.Value = i;

            }


            //  string result = rfbuilder_console.BruteForceSearch.bruteresult;           

            groupBox3.Text = "Самая дешевая система, удволетворяющая требованиям (Генетический алгоритм):";



            label31.Text = rfbuilder_console.GeneticAlgorithm.SystemSwitch.name;

            label40.Text = "Gain = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemSwitch.gain) + " dB";
            label42.Text = "NF = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemSwitch.noise) + " dB";
            label43.Text = "Cost = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemSwitch.cost) + " $";


            label30.Text = rfbuilder_console.GeneticAlgorithm.SystemLNA.name;

            label41.Text = "Gain = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemLNA.gain) + " dB";
            label39.Text = "NF = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemLNA.noise) + " dB";
            label38.Text = "Cost = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemLNA.cost) + " $";



            label29.Text = rfbuilder_console.GeneticAlgorithm.SystemMixer.name;

            label37.Text = "Gain = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemMixer.gain) + " dB";
            label36.Text = "NF = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemMixer.noise) + " dB";
            label35.Text = "Cost = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemMixer.cost) + " $";

            label28.Text = rfbuilder_console.GeneticAlgorithm.SystemFilter.name;

            label34.Text = "Gain = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemFilter.gain) + " dB";
            label33.Text = "NF = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemFilter.noise) + " dB";
            label32.Text = "Cost = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SystemFilter.cost) + " $";


            label14.Text = "Усиление системы = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SortedPopulation[0].ChromosomeTotalGain) + " dB";
            label15.Text = "Шум системы = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SortedPopulation[0].ChromosomeTotalNoise) + " dB";
            label16.Text = "Цена системы = " + Convert.ToString(rfbuilder_console.GeneticAlgorithm.SortedPopulation[0].ChromosomeTotalCost) + " $";


        } // Вызывает генетический алгоритм

        // Кнопки действия


        private void button2_Click(object sender, EventArgs e)
        {
            SystemCalc();
        }
        private void button1_Click(object sender, EventArgs e) 
        {
            BruteAlgo();
        }  
        private void button4_Click(object sender, EventArgs e)
        {
            GeneticClick();
        }

        //
      


        //Построение диаграммы.

        private void button3_Click(object sender, EventArgs e) // Кнопка построения диаграммы
        {

            XmlSerializer formatter = new XmlSerializer(typeof(ICSketch));
            Stream switch_stream = new FileStream("Switch1.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            ICSketch SwitchIC = (ICSketch)formatter.Deserialize(switch_stream);
            switch_stream.Close();

            Stream lna_stream = new FileStream("LNA1.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            ICSketch LNAIC = (ICSketch)formatter.Deserialize(lna_stream);
            lna_stream.Close();

            Stream mixer_stream = new FileStream("Mixer1.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            ICSketch MixerIC = (ICSketch)formatter.Deserialize(mixer_stream);
            mixer_stream.Close();

            Stream filter_stream = new FileStream("Filter1.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            ICSketch FilterIC = (ICSketch)formatter.Deserialize(filter_stream);
            filter_stream.Close();



            int start_X_1 = 35;
            int start_Y_1 = 475;

            double scalefactor = 0.15;

            //////////////////////////Первый элемент
            Image Switch = Image.FromFile("Switch1.png");
            //Создаем новый элемент типа PictureBox.       
            //Задаем параметры PictureBox.
            pictureBox2.Image = Switch; //вызов картинки
            pictureBox2.Size =  new Size(Convert.ToInt16(SwitchIC.Width* scalefactor), Convert.ToInt16(SwitchIC.Height* scalefactor) ); // масштабировани
           
            // Заполняю из иксмль частные координаты  вч контактов  * в масштаб
            int RFINX1 = Convert.ToInt16(SwitchIC.RFINX* scalefactor);
            int RFINY1 = Convert.ToInt16(SwitchIC.RFINY*scalefactor);
            int RFOUTX1 = Convert.ToInt16(SwitchIC.RFOUTX*scalefactor);
            int RFOUTY1 = Convert.ToInt16(SwitchIC.RFOUTY*scalefactor);                      
            //начальное положение переключателя
            pictureBox2.Location = new Point(start_X_1, start_Y_1);
            pictureBox2.Visible = true;           




            //-----------------Второй элемеент----------------
            Image LNA = Image.FromFile("LNA1.png");
            //Создаем новый элемент типа PictureBox.                  
            pictureBox3.Image = LNA; //вызов картинки
            pictureBox3.Size = new Size(Convert.ToInt16(LNAIC.Width * scalefactor), Convert.ToInt16(LNAIC.Height * scalefactor));
            //// из ксмль
            int RFINX2 = Convert.ToInt16(LNAIC.RFINX * scalefactor);
            int RFINY2 = Convert.ToInt16(LNAIC.RFINY * scalefactor);
            int RFOUTX2 = Convert.ToInt16(LNAIC.RFOUTX * scalefactor);
            int RFOUTY2 = Convert.ToInt16(LNAIC.RFOUTY * scalefactor);
            ///
            // Отрисовка втрого компонента
            int start_X_2 = start_X_1 + Convert.ToInt16(SwitchIC.Width * scalefactor)+7;
            int start_Y_2 = start_Y_1+ Math.Abs(RFOUTY1- RFINY2);
            //начальное положение пикчербокса
            pictureBox3.Location = new Point(start_X_2, start_Y_2);
            pictureBox3.Visible = true;
                                     
            //------------------Третий элемент----------------------




            Image Mixer = Image.FromFile("Mixer1.png");
           
            pictureBox4.Image = Mixer; //вызов картинки
            pictureBox4.Size = new Size(Convert.ToInt16(MixerIC.Width * scalefactor), Convert.ToInt16(MixerIC.Height * scalefactor)); // масштабирование
            //// из ксмль
            int RFINX3 = Convert.ToInt16(MixerIC.RFINX * scalefactor);
            int RFINY3 = Convert.ToInt16(MixerIC.RFINY * scalefactor);
            int RFOUTX3 = Convert.ToInt16(MixerIC.RFOUTX * scalefactor);
            int RFOUTY3 = Convert.ToInt16(MixerIC.RFOUTY * scalefactor);
            //Расчет положения

            int start_X_3 = start_X_2 + Convert.ToInt16(LNAIC.Width * scalefactor)+7;
            int start_Y_3 = start_Y_2 + Math.Abs(RFOUTY2 - RFINY3);                       
            //начальное положение пикчербокса

            pictureBox4.Location = new Point(start_X_3, start_Y_3);
            pictureBox4.Visible = true;            




            //------------------Четвертый элемент-----------------
            Image Filter = Image.FromFile("Filter1.png");
            //Создаем новый элемент типа PictureBox.       
            //Задаем параметры PictureBox.
            pictureBox5.Image = Filter; //вызов картинки
            pictureBox5.Size = new Size(Convert.ToInt16(FilterIC.Width * scalefactor), Convert.ToInt16(FilterIC.Height * scalefactor)); // масштабирование
            //// из ксмль
            int RFINX4 = Convert.ToInt16(FilterIC.RFINX * scalefactor);
            int RFINY4 = Convert.ToInt16(FilterIC.RFINY * scalefactor);
            int RFOUTX4 = Convert.ToInt16(FilterIC.RFOUTX * scalefactor);
            int RFOUTY4 = Convert.ToInt16(FilterIC.RFOUTY * scalefactor);    
            
            int start_X_4 = start_X_3 + Convert.ToInt16(MixerIC.Width * scalefactor)+7;
            int start_Y_4 = start_Y_3 + Math.Abs(RFOUTY3 - RFINY4)-25;
            //начальное положение пикчербокса
            pictureBox5.Location = new Point(start_X_4, start_Y_4);
            pictureBox5.Visible = true;
            
            
            
            
            //Корпусирование
            total_widht = Convert.ToInt16((SwitchIC.Width + LNAIC.Width + MixerIC.Width + FilterIC.Width) * scalefactor) + 10;
            total_height = Convert.ToInt16(SwitchIC.Height * scalefactor) + 10;
            Bitmap housing = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics line = Graphics.FromImage(housing);
            Pen pen = new Pen(Color.Black, 2);
            pictureBox1.Location = new Point(start_X_1 - 25, start_Y_1 - 25);
            pictureBox1.Size = new Size(total_widht + 40, total_height + 35);
            pictureBox1.Visible = true;
            line.DrawRectangle(pen, 1, 1, pictureBox1.Width - 2, pictureBox1.Height - 2);
            //Текущие размеры площадки это picturebox1 size- икс и игрек от нуля до totalwidht+40, totalheight+35
            //Сейчас пад скейл фактор 0,16
            Pen cases = new Pen(Color.Black, 2);

            Pen gold = new Pen(Color.Red, 2);
            //Ground

            int firstlenght = Convert.ToInt16(SwitchIC.Width * scalefactor) + 30;

            int seclenght = Convert.ToInt16((LNAIC.Width + SwitchIC.Width) * scalefactor) + 35;

            int trlenght = Convert.ToInt16((SwitchIC.Width + LNAIC.Width + MixerIC.Width) * scalefactor) + 42;

       
            //разделение элементов
            line.DrawLine(cases, firstlenght, 1, firstlenght, pictureBox1.Height);
            line.DrawLine(cases, seclenght, 1, seclenght, pictureBox1.Height);
            line.DrawLine(cases, trlenght, 1, trlenght, pictureBox1.Height);

            //Разводка
           // line.DrawLine(gold, Convert.ToInt16(SwitchIC.PADs[0].X*0.21), pictureBox1.Height, Convert.ToInt16(SwitchIC.PADs[0].X*0.21), 1);

           // line.DrawLine(gold, Convert.ToInt16(SwitchIC.PADs[1].X * 0.21), Convert.ToInt16(SwitchIC.PADs[2].Y), Convert.ToInt16(SwitchIC.PADs[2].X * 0.21), 1);


            pictureBox1.Image = housing;







            // Площадь
            int switchL = 1350;
            int switchW = 850;

            label46.Text = "L = "+Convert.ToString(switchL)+" um";
            label47.Text = "W = "+Convert.ToString(switchW)+" um";

            int LNAL = 2300;
            int LNAW = 1350;


            label50.Text = "L = "+Convert.ToString(LNAL) +" um";
            label49.Text = "W = " + Convert.ToString(LNAW) + " um";


            int mixerL = 1160;
            int mixerW = 790;

            label52.Text = "L =  "+ Convert.ToString(mixerL) +" um";
            label51.Text = "W =  "+ Convert.ToString(mixerW) +" um";

            int filterL = 1580;
            int filterrW = 850;

            label56.Text = "L =" + Convert.ToString(filterL) + " um";
            label55.Text = "W =" + Convert.ToString(filterrW) + " um";

            double f = 10;
            double step = -6;

            double totalarea = (switchL * switchW + LNAL * LNAW + mixerL * mixerW + filterL * filterrW) * Math.Pow(f, step);
          

               label48.Text = "Total area ="+ Convert.ToString(totalarea) + " mm2";

        }
     }

}
