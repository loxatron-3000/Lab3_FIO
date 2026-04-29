using System;

namespace Lab3_Triangle
{
    public class TriangleChecker
    {
        public static (string triangleType, string errorMessage) CheckTriangle(double a, double b, double c)
        {
           
            if (a <= 0 || b <= 0 || c <= 0)
                return ("", "Ошибка: Длины сторон должны быть положительными числами");

            
            if (a + b <= c || a + c <= b || b + c <= a)
                return ("", "Ошибка: Треугольник с такими сторонами не существует");

            if (a == b && b == c)
                return ("Равносторонний", "");
            else if (a == b || a == c || b == c)
                return ("Равнобедренный", "");
            else
                return ("Разносторонний", "");
        }
    }
}