using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using WireMock.Logging;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace WireMockTests
{
    [TestClass]
    public class MockBaseTest
    {
        private TestContext _testContext;
        private FluentMockServer _mockServer;

        public TestContext TestContext
        {
            get => _testContext;
            set => _testContext = value;
        }

        public FluentMockServer MockServer
        {
            get { return _mockServer; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (_mockServer != null)
            {
                _mockServer.Reset();
            }
            else
            {
                _mockServer = FluentMockServer.Start();
            }
        }

        protected void ConfigMockForDemoTest()
        {
            _mockServer
                .Given(
                    Request.Create().WithPath("/some/thing").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody("Hello world!")
                );

            _mockServer
                .Given(
                    Request.Create().WithPath("/some/thing/1").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody("Hello world 1!")
                );

            _mockServer
                .Given(
                    Request.Create().WithPath("/some/thing/2").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody("Hello world 2!")
                );
        }

        protected void ConfigMockForFailedDemoTest()
        {
            _mockServer
                .Given(
                    Request.Create().WithPath("/some/thing").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody("FooBar!")
                );
        }

        protected void ConfigMochEventHandling()
        {
            _mockServer.LogEntriesChanged += new NotifyCollectionChangedEventHandler(MockServer_LogEntrieChanged);
        }

        private void MockServer_LogEntrieChanged(object sender, NotifyCollectionChangedEventArgs ars)
        {
            var newEntries = ars.NewItems;
            Console.WriteLine($"Mock Server received new requests");

            foreach (object obj in newEntries)
            {
                LogEntry entry = obj as LogEntry;
                Console.WriteLine(
                    $"Request {newEntries.IndexOf(entry) + 1}. Method '{entry.RequestMessage.Method}'. Path '{entry.RequestMessage.Path}");
            }
        }

    }
}
