using HVS.Api.Core.Common.Enums;
using System;

namespace HVS.Api.Core.Business.Exceptions
{
    /// <summary>
    /// Guard against a database exception
    /// </summary>
    public class DatabaseException : Exception
    {
        /// <summary>
        /// Type of exception thrown by the database
        /// </summary>
        public DatabaseExceptionType ExceptionType { private set; get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="message"></param>
        public DatabaseException(DatabaseExceptionType exceptionType, string message = "") : base(message)
        {
            ExceptionType = exceptionType;
        }
    }
}
