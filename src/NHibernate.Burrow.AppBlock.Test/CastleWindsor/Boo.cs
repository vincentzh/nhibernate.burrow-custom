using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Burrow.AppBlock.DAOBases;
using NHibernate.Burrow.AppBlock.EntityBases;

namespace NHibernate.Burrow.AppBlock.Test.CastleWindsor
{
    public class Boo:IntegerIdNBizKeyBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }
       
    }
}
