using System;
using System.Threading;

namespace ConsoleApp22
{
    public delegate bool ContrainsDelegate(double[,] matrix, double target);

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ЛР 22. Вариант 25 — асинхронный поиск в матрице ===\n");

            Console.WriteLine("Число строк: ");
            int rows = int.Parse(Console.ReadLine());
            Console.WriteLine("Число столбцов: ");
            int cols = int.Parse(Console.ReadLine());

            double[,] matrix = CreateRandomMatrix(rows, cols);

            Console.WriteLine("\nСгенерированная матрица:");
            PrintMatrix(matrix);

            Console.WriteLine("\nИскомый элемент: ");
            double target = double.Parse(Console.ReadLine().Replace(',', '.'),
                System.Globalization.CultureInfo.InvariantCulture);

            ContrainsDelegate del = ContainsElement;

            Console.WriteLine("\nЗапуск асинхронного поиска...");
            IAsyncResult ar = del.BeginInvoke(matrix, target, null, null);

            Console.Write("Ожидание");
            while (!ar.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(80);
            }
            Console.WriteLine(" готово. ");

            bool found = del.EndInvoke(ar);

            Console.WriteLine(found
                ? "\nРезультат: элемент {0} НАЙДЕН в матрице."
                : "\nРезультат: элемент {0} НЕ найден в матрице.", target);

            Console.WriteLine("\nНажмите Enter для выхода...");
            Console.ReadLine();
        }

        static bool ContainsElement(double[,] matrix, double target)
        {
            Console.WriteLine("\n[Поток делегата] Поиск начат...");

            Thread.Sleep(1500);

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            const double eps = 1e-9;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (Math.Abs(matrix[i, j] - target) < eps)
                    {
                        Console.WriteLine("[Поток делегата] Поиск завершён (найден).");
                        return true;
                    }
                }
            }

            Console.WriteLine("[Поток делегата] Поиск завершён (не найден).");
            return false;
        }

        static double[,] CreateRandomMatrix(int rows, int cols)
        {
            var rnd = new Random();
            var m = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0;j < cols; j++)
                    m[i, j] = Math.Round(rnd.NextDouble() * 100, 2);
            return m;
        }

        static void PrintMatrix(double[,] m)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);
            for (int i = 0;i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.WriteLine("{0,8:F2}", m[i, j]);
                Console.WriteLine();
                
            }
        }
    }
}