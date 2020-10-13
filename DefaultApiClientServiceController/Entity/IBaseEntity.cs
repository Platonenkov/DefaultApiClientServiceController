namespace DefaultApiClientServiceController.Entity
{
    public interface IBaseEntity<T>
    {
        /// <summary> FOREIGN KEY </summary>
        T ID { get; set; }
    }
}