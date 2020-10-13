using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultApiClientServiceController.Entity
{
    public abstract class BaseEntity<T> : IBaseEntity<T>
    {
        #region Implementation of IBaseEntity
        /// <summary> Id of base entity on database </summary>
        public T ID { get; set; }

        #endregion
    }
}
