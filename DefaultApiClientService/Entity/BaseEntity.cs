using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultApiClientServiceController.Entity
{
    /// <summary>
    /// base entity
    /// </summary>
    /// <typeparam name="T">type of Id (int, long, Guid)</typeparam>
    public abstract class BaseEntity<T> : IBaseEntity<T>
    {
        #region Implementation of IBaseEntity
        /// <summary> Id of base entity on database </summary>
        public virtual T ID { get; set; }

        #endregion
    }
}
