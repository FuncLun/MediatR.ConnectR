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
            if (!(exception.InnerException is null))
                InnerExceptionDetail = new ExceptionDetail(exception.InnerException, true);

            if (exception is AggregateException aggregate)
                AggregateExceptionDetails = aggregate
                    .InnerExceptions
                    .Select(ae => new ExceptionDetail(ae, true))
                    .ToList();
        }
        public string Message { get; }
        public string ExceptionType { get; }
        public string StackTrace { get; }
        public ExceptionDetail InnerExceptionDetail { get; }
        public IEnumerable<ExceptionDetail> AggregateExceptionDetails { get; }
    }
}
