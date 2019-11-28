using System;
using System.Collections.Generic;
using System.Text;

namespace FCms.Exceptions
{
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException()
        {
        }

        public RepositoryNotFoundException(string message) : base(message)
        {
        }

        public RepositoryNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
