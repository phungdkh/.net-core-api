using System;

namespace HVS.Api.Core.Business.Exceptions
{
    /// <summary>
    /// Exceptions for Business Rules
    /// </summary>
    public class BusinessRuleException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public BusinessRuleException(string message = "") : base(message)
        {
        }
    }
}
