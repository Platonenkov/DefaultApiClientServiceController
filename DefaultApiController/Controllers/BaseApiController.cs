using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DefaultApiClientServiceController.Entity;
using DefaultApiClientServiceController.Interface;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace DefaultApiClientServiceController.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //[Produces("application/json")]
    public abstract class BaseApiController<T, T2,T3> : ControllerBase where T : IBaseDataService<T2,T3> where T2 : BaseEntity<T3>
    {
        protected readonly T Service;

        protected BaseApiController(T service)
        {
            Service = service;
        }
        // GET: api/[controller]/Count
        [HttpGet("Count")]
        public virtual async Task<ActionResult<int>> GetCount()
        {
            return await Service.GetTotalCountAsync();
        }
        // GET: api/[controller]/All
        [HttpGet]
        [EnableQuery]
        public virtual IQueryable<T2> GetAll() => Service.GetAll();

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        [EnableQuery]
        public virtual async Task<T2> Get(T3 id)
        {
            var data = await Service.GetAsync(id);

            //if (data == null)
            //{
            //    return NotFound();
            //}

            return data;
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        [EnableQuery]
        public virtual async Task<ActionResult<T2>> Put(T3 id, T2 data)
        {
            if (!id.Equals(data.ID))
            {
                return BadRequest();
            }

            var entity = await Service.UpdateAsync(id, data);
            if (entity is null && !await Exists(id))
            {
                return NotFound();
            }

            return entity;
        }

        // POST: api/[controller]/
        [HttpPost]
        [EnableQuery]
        public virtual async Task<T2> Post(T2 data)
        {
            var entity = await Service.AddNewAsync(data);

            return entity;
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        [EnableQuery]
        public virtual async Task<ActionResult<T2>> Delete(T3 id)
        {
            var entity = await Service.DeleteAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        // GET: api/[controller]/Exist/5
        [HttpGet("Exist/{id}")]
        [EnableQuery]
        public virtual async Task<bool> Exists(T3 id) => await Service.ExistsAsync(id);
        // GET: api/[controller]/SaveChanges
        [HttpGet("SaveChanges")]
        public virtual async Task<bool> Save() => await Service.SaveChangesAsync();

    }
}
