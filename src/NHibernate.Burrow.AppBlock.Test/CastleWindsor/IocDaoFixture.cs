using System.Collections;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate.Burrow.AppBlock.DAOBases;
using NHibernate.Burrow.AppBlock.EntityBases;
using NHibernate.Burrow.Facilities;
using NUnit.Framework;

namespace NHibernate.Burrow.AppBlock.Test.CastleWindsor
{
    public class IocDaoFixture : TestCase
    {
        protected IWindsorContainer container;

        protected override IList Mappings
        {
            get { return new[] {"CastleWindsor.Boo.hbm.xml"}; }
        }

        public IWindsorContainer BootstrapContainer()
        {
            var container = new WindsorContainer();
            container.AddFacility<DAOFacility>(facility =>
                                                   {
                                                       facility.IsWeb = false;
                                                       facility.AssemblyNamed = MappingsAssembly;
                                                   });
             container.Register(Component.For<ISession>().Instance(OpenSession()));
            return container;
        }

        protected override void OnSetUp()
        {
            base.OnSetUp();
            container = BootstrapContainer();
        }

        [Test]
        public void IocDAO()
        {
            using (ISession session = container.Resolve<BooDAO>().Session)
            {
                int id;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var boo = new Boo();
                   
                    Assert.IsNotNull(container.Resolve<BooDAO>());
                    Assert.IsNotNull(container.Resolve<BooDAO>().DAO);
                    boo.Name = "Boo";
                    container.Resolve<BooDAO>().Save(boo);
                    transaction.Commit();
                    id = boo.Id;
                }
                session.Clear();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Boo boo = container.Resolve<BooDAO>().Get(id);
                    Assert.IsNotNull(boo);
                    Assert.AreEqual("Boo", boo.Name);
                    transaction.Commit();
                }
                session.Clear();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Boo boo = container.Resolve<BooDAO>().Get(id);
                    container.Resolve<BooDAO>().Delete(boo);
                    transaction.Commit();
                    boo = container.Resolve<BooDAO>().Get(id);
                    Assert.IsNull(boo);
                }
                session.Clear();
            }
        }

        protected override void OnTearDown()
        {
            container.Dispose();
            base.OnTearDown();
            
        }
    }
}