namespace Lab3_Triangle
{
    public interface IDatabase
    {
        void AddTriangle(double a, double b, double c, string triangleType, string errorMessage);
        (bool exists, string triangleType, string errorMessage) GetTriangle(double a, double b, double c);
        void DeleteTriangle(double a, double b, double c);
        void ClearAll();
    }
}