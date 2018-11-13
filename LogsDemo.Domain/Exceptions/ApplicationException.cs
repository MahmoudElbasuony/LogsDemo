using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Domain.Exceptions
{
    public class ApplicationException : Exception
    {

        private string _errorMessage { get; set; }

        public override string Message => this._errorMessage;

        public ApplicationException() : base()
        {
            _errorMessage = "Unexpected Error Happened ! Please Try Again Later .";
        }

        public ApplicationException(string errorMesage) : base(errorMesage)
        {
            _errorMessage = errorMesage;
        }

        public ApplicationException(Exception exception) : base(exception.Message, exception.InnerException)
        {
            _errorMessage = exception.Message + (exception.InnerException != null ? $" : {exception.InnerException.Message}" : string.Empty);
        }

    }
}
