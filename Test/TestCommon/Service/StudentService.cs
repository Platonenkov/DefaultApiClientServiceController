using DefaultApiClientServiceController.Service;
using Domain;
using Infrastucture.Context;

namespace TestCommon.Service
{
    public class StudentService: BaseDataService<Student, int>, IStudentService
    {
        public StudentService(TestContext db) : base(db)
        {
        }
    }
}
