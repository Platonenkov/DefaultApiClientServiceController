using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using TestCommon.Service;

namespace BlazorServerWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _Service;

        public StudentController(ILogger<StudentController> logger, IStudentService service)
        {
            _logger = logger;
            _Service = service;
        }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
             
            //var t = new Student() {Age = 27, Name = "new student"};
            //var t = _Service.Get(10);
            //t.Name = "new student";
            //var new_s = _Service.Update(10,t);
            var d = _Service.GetAll();
            var data = new List<Student>(d);
            return data;
        }
    }
}
