using System;
using System.Runtime.Serialization;

namespace WasteProducts.DataAccess.Common.Exceptions
{

    [Serializable]
    public class LuceneSearchServiceException : Exception
    {
        public LuceneSearchServiceException()
        {
        }

        public LuceneSearchServiceException(string message) : base(message)
        {
        }

        public LuceneSearchServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LuceneSearchServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}