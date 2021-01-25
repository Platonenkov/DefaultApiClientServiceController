using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultApiClientServiceController.Client;
using Domain;
using Microsoft.Extensions.Configuration;
using TestCommon.Service;

namespace TestCommon
{
    public class StudentsClient : BaseApiClient<Student, int>, IStudentService
    {
        public StudentsClient(IConfiguration configuration) : base(configuration, "api/students")
        {
        }
    }
}
