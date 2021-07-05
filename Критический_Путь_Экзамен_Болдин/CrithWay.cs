using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Критический_Путь_Экзамен_Болдин
{
    public class CrithWay
    {
        public string s = "";
        /// <summary>
        /// Метод проведения диалога
        /// </summary>
        /// <returns></returns>
        [STAThread]
        static string Dialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV documents (.csv)|*.csv";
            dlg.ShowDialog();
            return dlg.FileName;
            if (dlg.FileName == "")
            {
                MessageBox.Show("Вы не указали файл");
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// Структура, содержащая все пути и расстояние между ними
        /// </summary>
        public struct Rastoyanie
        {
            public int point1;
            public int point2;
            public int length;
            public override string ToString()
            {
                return point1.ToString() + " - " + point2.ToString() + " " + length.ToString();
            }
        }
        /// <summary>
        /// Метод ввода данных в программу из csv-файла
        /// </summary>
        /// <returns></returns>
        public List<Rastoyanie> Input(string path)
        {

            List<Rastoyanie> Start = new List<Rastoyanie>();
            using (StreamReader sr = new StreamReader("Input.csv"))//Input-csv-файл, из которого берутся данные
            {
                while (sr.EndOfStream != true)
                {
                    string[] s1 = sr.ReadLine().Split(';');
                    string[] s2 = s1[0].Split('-');

                    Start.Add(new Rastoyanie { point1 = Convert.ToInt32(s2[0]), point2 = Convert.ToInt32(s2[1]), length = Convert.ToInt32(s1[1]) });

                }
            }
            return Start;
        }

        /// <summary>
        /// Главный метод класса решения
        /// </summary>
        public void Work()
        {
            List<Rastoyanie> ListWay;//лист путей
            MessageBox.Show("Выбери файл входных данных");
            List<Rastoyanie> Start = Input(Dialog());//лист исходных данных 
            Log(Start);
            ListWay = Start.FindAll(x => x.point1 == Start[MinPoint(Start)].point1);//запись точки начала в лист путей
            List<List<Rastoyanie>> ListWayLength = new List<List<Rastoyanie>>();//лист путей и функций
            foreach (Rastoyanie rb in ListWay)//построение путей из начальных возможных перемещений
            {
                CreateWay(Start, rb);//Построение пути
                ListWayLength.Add(Branch(Start, s));//Построение ветвей
                s = "";
            }
            int max = ListWayLength[0][0].length, maxind = 0;
            for (int i = 0; i < ListWay.Count; i++)// подсчет стоимости путей
            {
                if (LengthWay(ListWayLength[i]) >= max)// выбор самого большого
                {
                    max = LengthWay(ListWayLength[i]);
                    maxind = i;
                }
            }
            Debug.WriteLine("Максимум " + max);
            Debug.WriteLine("Номер максимума " + maxind);
            MessageBox.Show("Выбери файл выходных данных");
            Output(ListWayLength, maxind, max, Dialog());///Запись в файл решения
            Debug.Listeners.Clear();
        }


        /// <summary>
        /// Метод вывода результата решения в csv-файл
        /// </summary>
        /// <param name="ListWayLength"></param>
        /// <param name="maxind"></param>
        /// <param name="max"></param>
        public void Output(List<List<Rastoyanie>> ListWayLength, int maxind, int max,string path)
        {
            using (StreamWriter sr = new StreamWriter(@"Output.csv", false, Encoding.Default, 10))//Output-csv-файл, в который записывается окончательное решение
            {
                foreach (Rastoyanie Path in ListWayLength[maxind])
                {
                    sr.Write(Path.point1 + " - " + Path.point2 + "(" + Path.length + ")"+"\n");
                }
                sr.WriteLine("Длина " + max);
            }
        }
        /// <summary>
        /// Метод ввода всех возможных путей в файл txt
        /// </summary>
        /// <param name="Start"></param>
        public void Log(List<Rastoyanie> Start)
        {
            Debug.WriteLine("Все пути ");
            foreach (Rastoyanie r in Start)
            {
                Debug.Write(" Пункт " + r.point1 + " - Пункт " + r.point2 + " Расстояние " + r.length + "\n ");
            }
        }

        /// <summary>
        /// Поиск начальной точки.Поиск элемента, которого нет во втором столбце.
        /// </summary>
        /// <param name="Start"></param>
        /// <returns></returns>
        public int MinPoint(List<Rastoyanie> Start)
        {
            int min = Start[0].point1, minind = 0;
            foreach (Rastoyanie Path in Start)
            {
                if (Path.point1 <= min)
                {
                    min = Path.point1;
                    minind = Start.IndexOf(Path);
                }
            }
            return minind;
        }

        /// <summary>
        /// Поиск начальной точки.Поиск элемента, которого нет во втором столбце.
        /// </summary>
        /// <param name="Start"></param>
        /// <returns></returns>
        public int MaxPoint(List<Rastoyanie> Start)
        {
            int min = Start[0].point2, maxind = 0;
            foreach (Rastoyanie Path in Start)
            {
                if (Path.point2 >= min)
                {
                    min = Path.point1;
                    maxind = Start.IndexOf(Path);
                }
            }
            return maxind;
        }
        /// <summary>
        /// Метод построения пути
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="minel"></param>
        /// <returns></returns>
        public int CreateWay(List<Rastoyanie> Start, Rastoyanie minel)
        {
            int Lenght = 0;
            Rastoyanie Movements = Start.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);//Поиск возможных вариантов передвижения
            s += Movements.point1.ToString() + "-" + Movements.point2.ToString();//Пишем передвижение
            if (Movements.point2 == Start[MaxPoint(Start)].point2)//Смотрим не в конце ли мы
            {
                s += ";";
                return Movements.length;
            }
            else
            {
                for (int i = 0; i < Start.Count; i++)//Ищем стоимость перемещения в ту точку в которую мы пришли
                {
                    if (Start[i].point1 == Movements.point2)
                    {
                        s += ",";
                        Lenght = CreateWay(Start, Start[i]) + Movements.length;
                    }
                }
            }
            return Lenght;
        }

        /// <summary>
        /// Метод построения ветвлений.
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<Rastoyanie> Branch(List<Rastoyanie> Start, string s)
        {
            List<List<Rastoyanie>> ListBr = new List<List<Rastoyanie>>();//Лист ветвлений
            string[] s1 = s.Split(';');
            foreach (string st1 in s1)
            {
                if (st1 != "")
                {
                    ListBr.Add(new List<Rastoyanie>());
                    string[] s2 = st1.Split(',');
                    foreach (string str2 in s2)
                    {
                        if (str2 != "")
                        {
                            string[] str3 = str2.Split('-');
                            ListBr[ListBr.Count - 1].Add(Start.Find(x => x.point1 == Convert.ToInt32(str3[0]) && x.point2 == Convert.ToInt32(str3[1])));
                        }
                    }
                }
            }
            foreach (List<Rastoyanie> l in ListBr)
            {
                if (l[0].point1 != Start[MinPoint(Start)].point1)
                {
                    foreach (List<Rastoyanie> l1 in ListBr)
                    {
                        if (l1[0].point1 == Start[MinPoint(Start)].point1)
                        {
                            l.InsertRange(0, l1.FindAll(x => l1.IndexOf(x) <= l1.FindIndex(y => y.point2 == l[0].point1)));
                        }
                    }
                }
            }
            int max = ListBr[0][0].length, maxind = 0;
            for (int i = 0; i < ListBr.Count; i++)
            {
                if (LengthWay(ListBr[i]) >= max)
                {
                    max = LengthWay(ListBr[i]);
                    maxind = i;
                }
            }
            return ListBr[maxind];
        }

        
        /// <summary>
        /// Подсчет длины пути.
        /// </summary>
        /// <param name="Start"></param>
        /// <returns></returns>
        public int LengthWay(List<Rastoyanie> Start)
        {
            int Lenght = 0;
            foreach (Rastoyanie rb in Start)
            {
                Lenght += rb.length;
            }
            return Lenght;
        }

    }
}
