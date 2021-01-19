using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultApiClientServiceController.Service;
using Domain;
using Infrastucture;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Test.Service
{
    public class StudentService: BaseDataService<Student, int>, IStudentService
    {
        public StudentService(TestContext db) : base(db)
        {
        }
    }
}
