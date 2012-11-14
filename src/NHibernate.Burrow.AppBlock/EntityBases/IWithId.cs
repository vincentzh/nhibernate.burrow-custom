using System;

namespace NHibernate.Burrow.AppBlock.EntityBases{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Set the type of Id</typeparam>
    public interface IWithId<T>{
        T Id { get; set; }
    }
}