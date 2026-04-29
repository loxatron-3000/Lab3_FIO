using System;

namespace Lab3_Triangle
{
    public class ConsoleUserInput : IUserInput
    {
        public (double a, double b, double c) GetTriangleSides()
        {
            Console.Write("Введите сторону a: ");
            double a = double.Parse(Console.ReadLine());

            Console.Write("Введите сторону b: ");
            double b = double.Parse(Console.ReadLine());

            Console.Write("Введите сторону c: ");
            double c = double.Parse(Console.ReadLine());

            return (a, b, c);
        }
    }
}