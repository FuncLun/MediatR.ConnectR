using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public class ExceptionDetail
    {
        public ExceptionDetail(Exception exception, bool includeDetails)
        {
            Message = exception.Message;
            if (!includeDetails)
                return;

            ExceptionType = exception.GetType().FullName;
            StackTrace = exception.StackTrace;
            if (exception is AggregateException aggregateException)
            {
                InnerExceptionDetails = aggregateException
                    .InnerExceptions
                    .Select(ae => new ExceptionDetail(ae, true))
                    .ToList();
            }
            else if (!(exception.InnerException is null))
            {
                InnerExceptionDetails = new[]
                {
                    new ExceptionDetail(exception.InnerException, true),
                };
            }
        }
        public string Message { get; }
        public string ExceptionType { get; }
        public string StackTrace { get; }
        public IEnumerable<ExceptionDetail> InnerExceptionDetails { get; }
    }
}
