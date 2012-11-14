using System;
using System.Collections.Generic;
using NHibernate.Burrow.AppBlock.DAOBases;
using NHibernate.Criterion;

namespace NHibernate.Burrow.AppBlock.EntityBases{
    ///<summary>
    /// a base DAO for BizTransactionEntity
    ///</summary>
    ///<typeparam name="ReturnT"></typeparam>
    /// <remarks>
    /// This dao will help manage the life circle of BizTransactionEntity
    /// that is delete the timeout one and update the LastActivityInTransaction automatically and etc.
    /// </remarks>
    public abstract class BizTransactionEntityDAOBase<ReturnT> : GenericDAO<ReturnT> 
        where ReturnT : IBizTransactionEntity{
        private static readonly object lockObj = new object();
        private static readonly TimeSpan timeout = new TimeSpan(24, 0, 0);
        private static DateTime lastClearOutDatedTime = DateTime.Now;

        protected virtual string FinishedTransactionProperyName{
            get { return "FinishedTransaction"; }
        }

        protected virtual string LastActivityInTransactionProperyName{
            get { return "LastActivityInTransaction"; }
        }

        public override object Save(ReturnT t){
            ClearTimedOutEntities();
            return base.Save(t);
        }

        public override ReturnT Load(object id){
            ReturnT retVal = base.Load(id);
            IBizTransactionEntity entity = retVal;
            if (!entity.FinishedTransaction)
                entity.LastActivityInTransaction = DateTime.Now;
            return retVal;
        }

        private void ClearTimedOutEntities(){
            if (DateTime.Now > lastClearOutDatedTime + timeout){
                lastClearOutDatedTime = DateTime.Now;
                lock (lockObj){
                    
                    foreach (ReturnT t in FindTimedOutOnes())
                        t.Delete();
                }
            }
        }

        private IList<ReturnT> FindTimedOutOnes(){
            return CreateCriteria().Add(Restrictions.Eq(FinishedTransactionProperyName, false))
                .Add(Restrictions.Le(LastActivityInTransactionProperyName, DateTime.Now - timeout))
                .List<ReturnT>();
        }
        }
}