using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using NHibernate.Burrow.AppBlock.DAOBases;

namespace NHibernate.Burrow.Facilities
{
    public class DAOFacility : AbstractFacility
    {
        private string _assemblyNamed;
        private bool _isWeb;
        private ISession session;

        public ISession Session
        {
            get { return session; }
            set { session = value; }
        }

        public bool IsWeb
        {
            get { return _isWeb; }
            set { _isWeb = value; }
        }

        public string AssemblyNamed
        {
            get { return _assemblyNamed; }
            set { _assemblyNamed = value; }
        }


        protected override void Init()
        {
            Kernel.Register(RegistrationDao());
        }

        private BasedOnDescriptor RegistrationDao()
        {
            BasedOnDescriptor regDao =
                AllTypes.FromAssemblyNamed(_assemblyNamed).BasedOn(typeof (IGenericDAO<>)).WithService.DefaultInterfaces();
            if (_isWeb)
                regDao.LifestylePerWebRequest();
            else
            {
                regDao.LifestyleSingleton();
            }
            return regDao;
        }
    }
}