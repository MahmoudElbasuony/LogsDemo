using System;

namespace LogsDemo.SharedKernel.Exceptions
{
    public class CustomException : Exception
    {

        private string _errorMessage { get; set; }

        public override string Message => this._errorMessage;

        public CustomException() : base()
        {
            _errorMessage = "Unexpected Error Happened ! Please Try Again Later .";
        }

        public CustomException(string errorMesage) : base(errorMesage)
        {
            _errorMessage = errorMesage;
        }

        public CustomException(Exception exception) : base(exception.Message, exception.InnerException)
        {
            _errorMessage = exception.Message + (exception.InnerException != null ? $" : {exception.InnerException.Message}" : string.Empty);
        }

        public CustomException(string errorMessage, Exception exception) : base(exception.Message, exception.InnerException)
        {
            _errorMessage = $"{errorMessage} :  {exception.Message}  {(exception.InnerException != null ? $" : {exception.InnerException.Message}" : string.Empty)}";
        }

    }
}
