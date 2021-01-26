using System.Linq;
using System.Threading.Tasks;
using Domain;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TestCommon.Service
{
    public class TestContextInitializer
    {
        private readonly ILogger<TestContextInitializer> _Logger;
        private readonly TestContext _db;

        public TestContextInitializer(ILogger<TestContextInitializer> logger, TestContext db)
        {
            _Logger = logger;
            _db = db;
            _Logger.LogInformation("Инициализация контекста Базы данных");
        }

        public async Task InitializeAsync()
        {
            _Logger.LogInformation("Проверка миграций БД");
            await _db.Database.MigrateAsync();
            if (_db.Students.Any())
            {
                _Logger.LogInformation($"В БД есть записи");

                return;
            }
            Student[] students = new[]
            {
                new Student(){Age = 20, Name = "Alex"},
                new Student(){Age = 22, Name = "Nik"},
                new Student(){Age = 28, Name = "Tom"},
                new Student(){Age = 21, Name = "Jhon"},
                new Student(){Age = 19, Name = "Dex"},
                new Student(){Age = 32, Name = "Bado"},
                new Student(){Age = 18, Name = "Alex"},
                new Student(){Age = 25, Name = "Josh"},
                new Student(){Age = 25, Name = "Tom"}
            };
            _Logger.LogInformation($"Добавляю записи БД"); 
            await _db.Students.AddRangeAsync(students);
            _Logger.LogInformation($"Сохраняю изменения БД"); 
            await _db.SaveChangesAsync();
            _Logger.LogInformation($"БД инициализировано");
        }
    }
}
