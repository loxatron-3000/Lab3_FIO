using Moq;
using NUnit.Framework;
using System;

namespace Lab3_Triangle.Tests
{
    [TestFixture]
    public class TriangleTests
    {
        

        [Test]
        public void Database_AddAndGetTriangle_ShouldWorkCorrectly()
        {
          
            var db = new Database();

            
            db.AddTriangle(3, 4, 5, "Разносторонний", "");
            var (exists, type, error) = db.GetTriangle(3, 4, 5);

      
            Assert.IsTrue(exists);
            Assert.AreEqual("Разносторонний", type);
            Assert.AreEqual("", error);
        }

        [Test]
        public void Database_DeleteTriangle_ShouldRemoveRecord()
        {
    
            var db = new Database();
            db.AddTriangle(2, 2, 3, "Равнобедренный", "");

            db.DeleteTriangle(2, 2, 3);
            var (exists, _, _) = db.GetTriangle(2, 2, 3);

            Assert.IsFalse(exists);
        }

        [Test]
        public void Database_ClearAll_ShouldRemoveAllRecords()
        {
       
            var db = new Database();
            db.AddTriangle(3, 4, 5, "Разносторонний", "");
            db.AddTriangle(2, 2, 2, "Равносторонний", "");

            db.ClearAll();

            
            var (exists1, _, _) = db.GetTriangle(3, 4, 5);
            var (exists2, _, _) = db.GetTriangle(2, 2, 2);
            Assert.IsFalse(exists1);
            Assert.IsFalse(exists2);
        }

        [Test]
        public void MockEmailService_SendResult_AlwaysReturnsTrue()
        {
            // Arrange
            var emailService = new MockEmailService();

            // Act
            var result = emailService.SendResult("Test message");

            // Assert
            Assert.IsTrue(result);
        }

        // ===== ИНТЕГРАЦИОННЫЕ ТЕСТЫ =====

        [Test]
        public void IntegrationTest_NewTriangle_ShouldCalculateAndSaveToDatabase()
        {
            // Arrange
            var mockInput = new Mock<IUserInput>();
            mockInput.Setup(x => x.GetTriangleSides()).Returns((3, 4, 5));

            var database = new Database();
            var mockEmail = new Mock<IExternalService>();
            mockEmail.Setup(x => x.SendResult(It.IsAny<string>())).Returns(true);

            var controller = new TriangleController(database, mockInput.Object, mockEmail.Object);

            // Act
            var (result, error) = controller.ProcessTriangle();

            // Assert
            Assert.AreEqual("Разносторонний", result);
            Assert.AreEqual("", error);

            // Проверяем, что данные сохранились в БД
            var (exists, dbType, dbError) = database.GetTriangle(3, 4, 5);
            Assert.IsTrue(exists);
            Assert.AreEqual("Разносторонний", dbType);

            // Проверяем, что email был отправлен
            mockEmail.Verify(x => x.SendResult(It.Is<string>(msg => msg.Contains("Разносторонний"))), Times.Once);
        }

        [Test]
        public void IntegrationTest_ExistingTriangle_ShouldGetFromDatabaseNotRecalculate()
        {
            // Arrange
            var mockInput = new Mock<IUserInput>();
            mockInput.Setup(x => x.GetTriangleSides()).Returns((2, 2, 2));

            var database = new Database();
            // Предварительно сохраняем запись
            database.AddTriangle(2, 2, 2, "Равносторонний", "");

            var mockEmail = new Mock<IExternalService>();
            mockEmail.Setup(x => x.SendResult(It.IsAny<string>())).Returns(true);

            var controller = new TriangleController(database, mockInput.Object, mockEmail.Object);

            // Act
            var (result, error) = controller.ProcessTriangle();

            // Assert
            Assert.AreEqual("Равносторонний", result);
            Assert.AreEqual("", error);
            mockEmail.Verify(x => x.SendResult(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void IntegrationTest_InvalidTriangle_ShouldSaveErrorToDatabase()
        {
            // Arrange
            var mockInput = new Mock<IUserInput>();
            mockInput.Setup(x => x.GetTriangleSides()).Returns((1, 1, 3));

            var database = new Database();
            var mockEmail = new Mock<IExternalService>();
            mockEmail.Setup(x => x.SendResult(It.IsAny<string>())).Returns(true);

            var controller = new TriangleController(database, mockInput.Object, mockEmail.Object);

            // Act
            var (result, error) = controller.ProcessTriangle();

            // Assert
            Assert.AreEqual("", result);
            Assert.IsTrue(error.Contains("не существует"));

            // Проверяем, что ошибка сохранилась
            var (exists, dbType, dbError) = database.GetTriangle(1, 1, 3);
            Assert.IsTrue(exists);
            Assert.IsTrue(dbError.Contains("не существует"));
        }

        [Test]
        public void IntegrationTest_WithRealEmailService_ShouldWork()
        {
            // Arrange
            var mockInput = new Mock<IUserInput>();
            mockInput.Setup(x => x.GetTriangleSides()).Returns((5, 5, 5));

            var database = new Database();
            var realEmailService = new MockEmailService(); // Реальная реализация

            var controller = new TriangleController(database, mockInput.Object, realEmailService);

            // Act
            var (result, error) = controller.ProcessTriangle();

            // Assert
            Assert.AreEqual("Равносторонний", result);

            // Проверяем сохранение в БД
            var (exists, dbType, _) = database.GetTriangle(5, 5, 5);
            Assert.IsTrue(exists);
            Assert.AreEqual("Равносторонний", dbType);
        }
    }
}