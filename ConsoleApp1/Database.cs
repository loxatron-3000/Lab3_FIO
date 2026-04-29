using System.Collections.Generic;

namespace Lab3_Triangle
{
    public class Database : IDatabase
    {
        private class TriangleRecord
        {
            public double A { get; set; }
            public double B { get; set; }
            public double C { get; set; }
            public string TriangleType { get; set; }
            public string ErrorMessage { get; set; }

            public string GetKey() => $"{A}:{B}:{C}";
        }

        private Dictionary<string, TriangleRecord> _storage = new Dictionary<string, TriangleRecord>();

        public void AddTriangle(double a, double b, double c, string triangleType, string errorMessage)
        {
            var record = new TriangleRecord
            {
                A = a,
                B = b,
                C = c,
                TriangleType = triangleType,
                ErrorMessage = errorMessage
            };
            _storage[record.GetKey()] = record;
        }

        public (bool exists, string triangleType, string errorMessage) GetTriangle(double a, double b, double c)
        {
            string key = $"{a}:{b}:{c}";
            if (_storage.ContainsKey(key))
            {
                var record = _storage[key];
                return (true, record.TriangleType, record.ErrorMessage);
            }
            return (false, "", "");
        }

        public void DeleteTriangle(double a, double b, double c)
        {
            string key = $"{a}:{b}:{c}";
            if (_storage.ContainsKey(key))
                _storage.Remove(key);
        }

        public void ClearAll()
        {
            _storage.Clear();
        }
    }
}