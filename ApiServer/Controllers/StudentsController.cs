using ApiServer.Test.Service;
using DefaultApiClientServiceController.Controllers;
using Domain;
using Infrastucture;
using Microsoft.Extensions.Logging;

namespace ApiServer.Controllers
{
    public class StudentsController : BaseApiController<IStudentService, Student, int>
    {
        private readonly ILogger<BaseApiController<IStudentService, Student, int>> _logger;

        public StudentsController(IStudentService service, ILogger<BaseApiController<IStudentService, Student, int>> logger) : base(service)
        {
            _logger = logger;
        }

    }
}