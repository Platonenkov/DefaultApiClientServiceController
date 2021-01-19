using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultApiClientServiceController.Interface;
using Domain;
using Infrastucture;

namespace ApiServer.Test.Service
{
    public interface IStudentService : IBaseDataService<Student, int>
    {
        
    }
}
