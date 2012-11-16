using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Burrow.AppBlock.DAOBases;

namespace NHibernate.Burrow.AppBlock.Test.CastleWindsor
{
    public class BooDAO:GenericDAO<Boo>
    {
        public Boo1DAO DAO { get; set; }
    }
}
