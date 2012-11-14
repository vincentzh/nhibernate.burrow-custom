using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Burrow.AppBlock.EntityBases
{
   public interface IBusinessKey 
   {
        IComparable BusinessKey { get; }
   }
}
