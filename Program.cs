using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lesson_6_GB// Осипенко В.О.
{
    // Описываем делегат. В делегате описывается сигнатура методов, на
    // которые он сможет ссылаться в дальнейшем (хранить в себе)
    public delegate double Fun(double x, double a);

    internal class Program
    {
        #region Tusk 1 Изменить вывод функции
        // Создаем метод, который принимает делегат
        // На практике этот метод сможет принимать любой метод
        // с такой же сигнатурой, как у делегата
        public static void Table(Fun F, double x, double b)
        {
           
            Console.WriteLine("----- X ----- Y -----");
            while (x <= b)
            {
                Console.WriteLine("| {0,8:0.000} | {1,8:0.000} |", x, F(x, b));
                x += 1;
                
            }
            Console.WriteLine("---------------------");
        }
        // Создаем метод для передачи его в качестве параметра в Table
        public static double MyFunc_1(double x, double a)
        {
            return a * Math.Pow(x,2);
        }

        public static double MyFunc_2(double x, double a)
        {
            return a * Math.Sin(x);
        }
        #endregion


        #region Tusk 2 Модифицировать функцию нахождения минимума 

        
        public delegate double function(double x);

        public static double F_1(double x)
        {
            return x * x - 50 * x + 10;
        }
        public static double F_2(double x)
        {
            return x * x - 10 * x + 50;
        }
        public static void SaveFunc(string fileName, double a, double b, double h, function F)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            double x = a;
            while (x <= b)
            {
                bw.Write(F(x));
                x += h;// x=x+h;
            }
            bw.Close();
            fs.Close();
        }
        
        public static double Load(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);
            double min = double.MaxValue;
            double d;
            for (int i = 0; i < fs.Length / sizeof(double); i++)
            {
                // Считываем значение и переходим к следующему
                d = bw.ReadDouble();
                if (d < min) min = d;
            }
            bw.Close();
            fs.Close();
            return min;
        }

        #endregion



        static void Main()
        {
            Console.WriteLine(@"
Изменить программу вывода таблицы функции так, 
чтобы можно было передавать функции типа double (double, double). 
Продемонстрировать работу на функции с функцией a*x^2 и функцией a*sin(x).
");
            Console.WriteLine("Создаем таблицу по функции a * x^2: ");
            Table(new Fun(MyFunc_1), 0, 5);
            Console.WriteLine("Создаем таблицу по функции a * sin(x): ");
            Table(new Fun(MyFunc_2), -2, 5);

            Console.ReadLine();

            Console.WriteLine(@"
Модифицировать программу нахождения минимума функции так, чтобы можно было передавать функцию в виде делегата.
а) Сделать меню с различными функциями и представить пользователю выбор, для какой функции и на каком отрезке находить минимум. Использовать массив (или список) делегатов, в котором хранятся различные функции.
б) *Переделать функцию Load, чтобы она возвращала массив считанных значений. Пусть она возвращает минимум через параметр (с использованием модификатора out)
");//Честно, так и не понял как это сделать, да и в принципе в теье еще не разобрался
            function[] F = { F_1, F_2 };
            Console.WriteLine("Сделайте выбор: 1 - функция F_1(Минимальное значение), 2 - функция F_2(Максимальное значение)");
            int index = int.Parse(Console.ReadLine());
            SaveFunc("data.bin", -100, 100, 0.5, F[index - 1]);
            Console.WriteLine(Load("data.bin"));
            Console.ReadKey();
        }
    }
}
