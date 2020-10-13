### DefaultApiClientServiceController
#### Install-Package DefaultApiClientServiceController -Version 1.0.0
 
this contains default Api realization for EF Interface, EF Service, Api Client, Api Controller 

For use Api Clients add to you server appsettings.json row ->  "ClientAddress": "https://localhost:your_api_port/"

#### 1 Create Base entity whit id type what you need

Create you BaseEntity or use DefaultApiClientServiceController.DefaultApiClientServiceController.Entity.BaseEntity

```C#
public abstract class BaseEntity : DefaultApiClientServiceController.Entity.BaseEntity<long>, IBaseEntity
{
}
```
In this will be create FK ID type of "long" in database for you entity

```C#
public class HeaderInfo:BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
```

### 2 Create interface whith entity type and id type

Use base interface
```C#
    public interface IHeaderService:IBaseDataService<HeaderInfo,long>
    {

    }
    
    // Add more methods if you need
    // ore override base
```
Ore add what you need

```C#
    public class FaultService: BaseDataService<FaultComplexInfo,long>, IFaultService
    {
        public FaultService(BstoContext db) : base(db)
        {
        }

        #region Implementation of IFaultService

        public async Task<IEnumerable<FaultComplexInfo>> GetFlightFaultAsync(long headerid)
        {
            var header = await db.Set<HeaderInfo>().FindAsync(headerid);

            return header?.Faults.ToList();

        }

        #endregion
    }
```

### 3 Create service implementation, don't foget to send you database context !!!

```C#
    public class HeaderService: BaseDataService<HeaderInfo,long>, IHeaderService
    {
        public HeaderService(BstoContext db) : base(db)
        {
        }
    // Add more methods if you need
    // ore override base
    }
```
### 4 Create Client implementation 

send you controller address to base client

```C#
    public class HeadersClient : BaseApiClient<HeaderInfo,long>, IHeaderService
    {
        public HeadersClient(IConfiguration configuration) : base(configuration, "api/HeaderInfoes")
        {
        }
    // Add more methods if you need
    // ore override base
    }
```

### 5 Create Controller implementation

Create controller with base realization

```C#
    public class HeaderInfoesController : BaseApiController<IHeaderService,HeaderInfo,long>
    {
         public HeaderInfoesController(IHeaderService service) : base(service)
        {

        }
    }
```

Or take service what you need and override if you need
```C#
    public class HeaderInfoesController : BaseApiController<IHeaderService,HeaderInfo,long>
    {
        private readonly ILogger<BaseApiController<IHeaderService, HeaderInfo, long>> Logger;

        public HeaderInfoesController(IHeaderService service, ILogger<BaseApiController<IHeaderService,
        HeaderInfo,long>> logger) : base(service)
        {
            Logger = logger;
        }
        
        [HttpDelete("{id}")]
        public override async Task<ActionResult<HeaderInfo>> Delete(long id)
        {
            var headerInfo = await Service.DeleteAsync(id);
            if (headerInfo == null)
            {
                return NotFound();
            }

            Logger.LogInformation($"Delete information {headerInfo.RegistrationNo},"
                       + $" flight: {headerInfo.FlightNo}"
                       + $" from {headerInfo.Departure} to {headerInfo.Destination}"
                       + $" time: from {headerInfo.StartTime} to {headerInfo.EndTime}");
            
            return headerInfo;
        }
    }
```

## BaseEntity<T>
```C#
        /// <summary> FOREIGN KEY </summary>
        T ID { get; set; }
```

## BaseDataSerivce<T,T2> and Client methods

```C#
        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public Task<int> GetTotalCountAsync();
        
        /// <summary> Get count of entities in database </summary>
        /// <returns>int count</returns>
        public int GetTotalCount();
        
        /// <summary> Get all entities of T type from database </summary>
        /// <returns>IEnumerable Entities</returns>
        public Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary> Get all entities of T type from database </summary>
        /// <returns>IEnumerable Entities</returns>
        public IEnumerable<T> GetAll();
        
        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public Task<T> GetAsync(T2 id);
        
        /// <summary> Get entity of T type from database by id</summary>
        /// <returns>IEnumerable Entities</returns>
        public T Get(T2 id);
        
        /// <summary> Add new entity to database </summary>
        /// <param name="data">entity</param>
        /// <returns>database entity with id</returns>
        public Task<T> AddNewAsync(T data);
        
        /// <summary> Add new entity to database </summary>
        /// <param name="data">entity</param>
        /// <returns>database entity with id</returns>
        public T AddNew(T data);
        
        /// <summary> Delete entity from database </summary>
        /// <param name="id">entity id</param>
        /// <returns>database entity which was deleted</returns>
        public Task<T> DeleteAsync(T2 id);
        
        /// <summary> Delete entity from database </summary>
        /// <param name="id">entity id</param>
        /// <returns>database entity which was deleted</returns>
        public T Delete(T2 id);
        
        /// <summary> Update entity data in database </summary>
        /// <param name="id">entity id</param>
        /// <param name="data">entity data</param>
        /// <returns>entity after update</returns>
        public Task<T> UpdateAsync(T2 id, T data);
        
        /// <summary> Update entity data in database </summary>
        /// <param name="id">entity id</param>
        /// <param name="data">entity data</param>
        /// <returns>entity after update</returns>
        public T Update(T2 id, T data);
        
        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public Task<bool> ExistsAsync(T2 id);
        
        /// <summary> Check database to exist entity </summary>
        /// <param name="id">entity id</param>
        /// <returns>bool result</returns>
        public bool Exists(T2 id);
        
        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public Task<bool> SaveChangesAsync();
        
        /// <summary> Save database changes if u need </summary>
        /// <returns>bool result</returns>
        public bool SaveChanges();
```

## Base Api Methods:

```C#
        // GET: api/[controller]/Count
        // GET: api/[controller]/All
        // GET: api/[controller]/id
        // PUT: api/[controller]/5
        // POST: api/[controller]/
        // DELETE: api/[controller]/5
        // GET: api/[controller]/Exist/5
        // GET: api/[controller]/SaveChanges
```
