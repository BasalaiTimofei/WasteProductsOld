using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Common.Exceptions
{
    [Serializable]
    public class LuceneSearchRepositoryException : Exception
    {
        public LuceneSearchRepositoryException()
        {
        }

        public LuceneSearchRepositoryException(string message) : base(message)
        {
        }

        public LuceneSearchRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LuceneSearchRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
