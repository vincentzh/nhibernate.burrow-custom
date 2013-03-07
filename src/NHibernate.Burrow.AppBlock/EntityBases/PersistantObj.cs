/*
 * Created by: Kai Wang
 * Created: Tuesday, February 06, 2007
 */

using System;
using NHibernate.Burrow.AppBlock.DAOBases;

namespace NHibernate.Burrow.AppBlock.EntityBases{
    /// <summary>
    /// Targeted to be the standard PersistantObj
    /// </summary>
    public abstract class PersistantObj<T> : ObjWithIdNBizKeyBase<T>{
        /// <summary>
        /// a helper for inheritance to perform DAO functions
        /// </summary>
        [Obsolete]
        protected ObjectDAOHelper<PersistantObj<T>> dao;

 
        /// <summary>
        /// 
        /// </summary>
        public PersistantObj(){
            dao = new ObjectDAOHelper<PersistantObj<T>>(this);
            dao.PreDeleted += OnPreDeleted;
            
        }
        /// <summary>
        /// will be called when this object is going to be deleted;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Obsolete]
        protected virtual void OnPreDeleted(object sender, EventArgs e) {}
        [Obsolete("The recommended alternative is GenericDAO<T>.Save(T)")]
        public virtual void Save(){
            dao.Save();
        }
        [Obsolete("The recommended alternative is GenericDAO<T>.Delete(T)")]
        public virtual bool Delete(){
            return dao.Delete();
        }
    }
}