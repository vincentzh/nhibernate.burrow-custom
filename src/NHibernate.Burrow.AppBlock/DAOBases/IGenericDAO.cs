using System.Collections.Generic;
using NHibernate.Burrow.AppBlock.EntityBases;

namespace NHibernate.Burrow.AppBlock.DAOBases
{
    public interface IGenericDAO<ReturnT> where ReturnT : IEntity
    {
        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, or null if there is no such persistent instance. (If the instance, or a proxy for the instance, is already associated with the session, return that instance or proxy.)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a persistent instance or null</returns>
        ReturnT Get(object id);

        /// <summary>
        /// Return the persistent instance of the given entity class with the given identifier, assuming that the instance exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The persistent instance or proxy</returns>
        /// <remarks>
        /// You should not use this method to determine if an instance exists (use a query or NHibernate.ISession.Get(System.Type,System.Object) instead). Use this only to retrieve an instance that you assume exists, where non-existence would be an actual error.
        /// </remarks>
        ReturnT Load(object id);

        /// <summary>
        /// Delete the record of an entity from Database and thus the entity becomes transient
        /// </summary>
        /// <param name="t"></param>
        void Delete(ReturnT t);

        /// <summary>
        /// Re-read the state of the entity from the database
        /// </summary>
        /// <param name="t"></param>
        void Refresh(ReturnT t);

        /// <summary>
        /// Persist the entity <paramref name="t"/> to DB if it has not been persisted before 
        /// </summary>
        /// <param name="t"></param>
        /// <remarks>
        /// By default the instance is always saved. 
        /// This behaviour may be adjusted by specifying an unsaved-value attribute of the identifier property mapping 
        /// </remarks>
        void SaveOrUpdate(ReturnT t);

        /// <summary>
        /// Persist the given transient instance, first assigning a generated identifier.  
        /// </summary>
        /// <param name="t">the given transient instance</param>
        /// <returns>The generated identifier
        /// </returns>
        /// <remarks>
        /// Save will use the current value of the identifier property if the Assigned generator is used.
        /// </remarks>
        object Save(ReturnT t);

      

        ///<summary>
        /// This will fire a NHibernate Session Flush
        ///</summary>
        void Flush();
    }
}