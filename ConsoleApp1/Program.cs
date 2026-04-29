using System;

namespace Lab3_Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = new Database();
            var userInput = new ConsoleUserInput();
            var emailService = new MockEmailService();

            var controller = new TriangleController(database, userInput, emailService);

            Console.WriteLine("Проверка треугольника");
            var (result, error) = controller.ProcessTriangle();

            if (!string.IsNullOrEmpty(error))
                Console.WriteLine($"Результат: {error}");
            else
                Console.WriteLine($"Результат: {result}");
        }
    }
}