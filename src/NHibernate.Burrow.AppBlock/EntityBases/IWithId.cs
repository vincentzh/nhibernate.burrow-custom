using System;

namespace NHibernate.Burrow.AppBlock.EntityBases{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Set the type of Id</typeparam>
    public interface IWithId<T>:IEntity{
        T Id { get; set; }
    }
}