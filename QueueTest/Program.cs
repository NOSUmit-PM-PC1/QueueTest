using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueTest
{
    class Man
    {
        // время прихода
        public int TIn { get; set;}

        // время предполагаемого обслуживания
        public int T { get; set; }

        // время выхода из банка
        public int TOut { get; set; }
        
        // эффективность обслуживания предполагаемое/реальное
        // чем меньше, тех хуже обслуживание       
        public double Effect { get; set; }
        public string Name { get; set; }

        public void SetTimeOut(int timeFreeKassa)
        {
            TOut = Math.Max(TIn, timeFreeKassa) + T;
            Effect = (double)(T) / (TOut - TIn);
        }

        public Man(int tIn, int t, string name)
        {
            TIn = TIn;
            T = t;
            Name = name;  
        }
    }

    class Kassa
    {
        static public void SelectKassa(Kassa k1, Kassa k2, Man m)
        {
            if (k1.FreeKassa() <= k2.FreeKassa())
                k1.AddMan(m);
            else
                k2.AddMan(m);
        }

        Queue<Man> k = new Queue<Man>();

        public void AddMan(Man m)
        {
            if (FreeKassa() == 0)
                m.SetTimeOut(0);
            else
                m.SetTimeOut(k.Last().TOut);
            k.Enqueue(m);
        }

        // возвращает число характеризующее свободность кассы
        // чем меньше, тем более свободна
        public int FreeKassa()
        {
            return k.Count();
        }

        public string Print()
        {
            if (k.Count > 0)
            {
                Man m = k.Dequeue();
                return (m.Name + " уйдет в " + m.TOut + " эффективность " + m.Effect);
            }
            else
                return string.Empty;
        }

        // проверяем клиентов время обслуживания которых пришло
        public bool CheckTimeService(int t)
            {
                return k.Count() > 0 && k.Peek().T < t;
            }
    }

    class Program
    {
        static void print(Queue<int> q)
        { 
            foreach(var qq in q)
                Console.Write(qq + " ");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Kassa k1 = new Kassa();
            Kassa k2 = new Kassa();

            string s = Console.ReadLine();
            while (!string.IsNullOrEmpty(s))
            {
                
                int t = Convert.ToInt32(s.Split()[0]);
                int min = Convert.ToInt32(s.Split()[1]);
                string name = s.Split()[2];

                while (k1.CheckTimeService(t))
                {
                    Console.WriteLine(k1.Print());
                }
                while (k2.CheckTimeService(t))
                {
                    Console.WriteLine(k2.Print());
                }

                Man m = new Man(t, min, name);
                Kassa.SelectKassa(k1, k2, m);

               
                s = Console.ReadLine();
            }
            while (k1.FreeKassa() > 0)
            {
                Console.WriteLine(k1.Print());
            }
            while (k2.FreeKassa() > 0)
            {
                Console.WriteLine(k2.Print());
            }
        }
    }
}
