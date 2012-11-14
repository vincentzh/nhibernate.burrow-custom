using System;
using System.Runtime.CompilerServices;

namespace NHibernate.Burrow.AppBlock.EntityBases
{
    public abstract class ObjectWHashIdBase : ObjWithIdNBizKeyBase<int>
    {
        private long hashId;

        /// <summary>
        /// 
        /// </summary>
        protected ObjectWHashIdBase()
        {
            var result = (long) (DateTime.Now - new DateTime(2000, 1, 1)).TotalMilliseconds;
            HashId = result*131237 + RuntimeHelpers.GetHashCode(this)%4355463;
        }

        protected virtual long HashId
        {
            get { return hashId; }
            set { hashId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// return the HashId
        /// </remarks>
        public override IComparable BusinessKey
        {
            get { return HashId; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// overriden to use the HashId
        /// This method is kept as virtual for ORM framework to dynamically create proxy
        /// But it should never be overriden by developer
        /// </remarks>
        public override int GetHashCode()
        {
            return hashId.GetHashCode();
        }

      
        
    }
}