namespace DefaultApiClientServiceController.Entity
{
    /// <summary>
    /// base entity interface
    /// </summary>
    /// <typeparam name="T">type of Id (int, long, Guid)</typeparam>
    public interface IBaseEntity<T>
    {
        /// <summary> FOREIGN KEY </summary>
        T ID { get; set; }
    }
}