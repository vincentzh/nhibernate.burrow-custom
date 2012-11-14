using System;

namespace NHibernate.Burrow.AppBlock.EntityBases{
    public interface IPersistantObj<T> : IWithId<T>, IEntity, ITransient, IBusinessKey, IEquatable<IBusinessKey> { }
}