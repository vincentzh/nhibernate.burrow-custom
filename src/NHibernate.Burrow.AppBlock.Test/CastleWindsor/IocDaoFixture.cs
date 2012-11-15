using System.Collections;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NHibernate.Burrow.AppBlock.DAOBases;
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
            return new WindsorContainer().Register(
                AllTypes.FromThisAssembly().BasedOn(typeof (GenericDAO<>)).WithService.DefaultInterfaces())
                .Register(
                    Component.For<ISession>().Instance(OpenSession()));
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