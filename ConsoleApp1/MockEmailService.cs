using System;

namespace Lab3_Triangle
{
    public class MockEmailService : IExternalService
    {
        public bool SendResult(string message)
        {
            Console.WriteLine($"[МОК] Отправка email: {message}");
            return true;
        }
    }
}