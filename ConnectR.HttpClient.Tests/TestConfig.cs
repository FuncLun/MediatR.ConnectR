using System;

namespace MediatR.ConnectR.HttpClient
{
    public interface ITestConfig
    {
        Uri BaseUri { get; }
    }
    public class TestConfig : ITestConfig
    {
        public Uri BaseUri { get; set; }
    }
}