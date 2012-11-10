using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Burrow.AppBlock.EntityBases
{
    public interface IEntity
    {
        IComparable BusinessKey { get; }
        bool IsTransient { get; }
    }
}
