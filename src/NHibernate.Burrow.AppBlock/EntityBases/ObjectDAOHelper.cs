using System;
using NHibernate.Burrow.AppBlock.DAOBases;

namespace NHibernate.Burrow.AppBlock.EntityBases
{
    public class ObjectDAOHelper<T> where T : IEntity
    {
        private readonly T obj;
        private bool isDeleted;

        public ObjectDAOHelper(T obj)
        {
            this.obj = obj;
        }

        public IGenericDAO<T> GenericDao { get; set; }

        public bool IsDeleted
        {
            get { return isDeleted; }
        }


        public event EventHandler PreDeleted;

        /// <summary>
        /// Won't throw exception if the obj is either already deleted or still transient
        /// </summary>
        public bool Delete()
        {
            if (IsDeleted)
                return false;
            isDeleted = true;
            //need to make sure that even the object is transient the preDelete still gets fired
            if (PreDeleted != null)
                PreDeleted(this, new EventArgs());

            if (!obj.IsTransient)
            {
                if(GenericDao==null)
                    throw new NullReferenceException("GenericDAO property is Null");
                GenericDao.Delete(obj);
            }
            return true;
        }

        public void SaveOrUpdate()
        {
            if (isDeleted)
                throw new Exception("Can not saveorUpdate once deleted");
            {
                if (GenericDao == null)
                    throw new NullReferenceException("GenericDAO property is Null");
                GenericDao.SaveOrUpdate(obj);
            }
        }

        public void Save()
        {
            if (isDeleted)
                throw new Exception("Can not saveorUpdate once deleted");
            {
                if (GenericDao == null)
                    throw new NullReferenceException("GenericDAO property is Null");
                GenericDao.Save(obj);
            }
        }

        public void Refresh()
        {
            if (!obj.IsTransient && !IsDeleted)
            {
                if (GenericDao == null)
                    throw new NullReferenceException("GenericDAO property is Null");
                GenericDao.Refresh(obj);
            }
        }
    }
}