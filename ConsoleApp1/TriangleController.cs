namespace Lab3_Triangle
{
    public class TriangleController
    {
        private readonly IDatabase _database;
        private readonly IUserInput _userInput;
        private readonly IExternalService _externalService;

        public TriangleController(IDatabase database, IUserInput userInput, IExternalService externalService)
        {
            _database = database;
            _userInput = userInput;
            _externalService = externalService;
        }

        public (string result, string message) ProcessTriangle()
        {
            var (a, b, c) = _userInput.GetTriangleSides();

            string triangleType;
            string errorMessage;

            var (exists, existingType, existingError) = _database.GetTriangle(a, b, c);

            if (exists)
            {
                triangleType = existingType;
                errorMessage = existingError;
            }
            else
            {
                (triangleType, errorMessage) = TriangleChecker.CheckTriangle(a, b, c);
               
                _database.AddTriangle(a, b, c, triangleType, errorMessage);
            }

            string outputMessage = string.IsNullOrEmpty(errorMessage)
                ? $"Треугольник со сторонами {a}, {b}, {c} - {triangleType}"
                : errorMessage;

            _externalService.SendResult(outputMessage);

            return (triangleType, errorMessage);
        }
    }
}