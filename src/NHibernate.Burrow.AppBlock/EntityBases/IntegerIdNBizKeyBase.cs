using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Burrow.AppBlock.EntityBases
{
    /// <summary>
    /// Only work for Id is identity or native
    /// </summary>
    public abstract class IntegerIdNBizKeyBase:ObjWithIdNBizKeyBase<int>
    {
        public override IComparable BusinessKey
        {
            get { return Id; }
        }

       
    }
}
