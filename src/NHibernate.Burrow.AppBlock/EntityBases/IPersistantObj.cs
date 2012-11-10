using System;

namespace NHibernate.Burrow.AppBlock.EntityBases{
    public interface IPersistantObj<T> : IWithId<T>, IEquatable<ObjWithIdNBizKeyBase<T>> {}
}