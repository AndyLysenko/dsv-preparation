using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMockTests.Helpers;

namespace WireMockTests
{
    [TestClass]
    public class SimpleMockTests : MockBaseTest
    {
        [TestMethod]
        public void DemoTest1()
        {
            // Assign
            ConfigMockForDemoTest();

            // Act
            string url = "http://localhost:" + MockServer.Ports[0];
            string getResponse1 = HttpClientHelper.Get(url + "/some/thing");

            // Assert
            Assert.AreEqual("Hello world!", getResponse1);
        }

        [TestMethod]
        public void DemoTest2()
        {
            // Assign
            ConfigMockForFailedDemoTest();

            // Act
            string url = "http://localhost:" + MockServer.Ports[0];
            string getResponse1 = HttpClientHelper.Get(url + "/some/thing");

            // Assert
            Assert.AreEqual("Hello world!", getResponse1);
        }

        [TestMethod]
        public void DemoTest3()
        {
            // Assign
            ConfigMockForDemoTest();

            // Act
            string url = "http://localhost:" + MockServer.Ports[0];
            string getResponse1 = HttpClientHelper.Get(url + "/some/thing");

            // Assert
            Assert.AreEqual("Hello world!", getResponse1);

            //Stage 2

            // Assign
            ConfigMockForFailedDemoTest();

            // Act
            url = "http://localhost:" + MockServer.Ports[0];
            getResponse1 = HttpClientHelper.Get(url + "/some/thing");

            // Assert
            Assert.AreEqual("Hello world!", getResponse1);
        }



        [TestMethod]
        public void DemoTest4()
        {
            string url = "http://localhost:" + MockServer.Ports[0];

            MockServer
                .Given(
                    Request.Create().WithPath("/some/thing").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody("Hello world! Your path is {{request.path}}.")
                        .WithTransformer()
                );

            string getResponse1 = HttpClientHelper.Get(url + "/some/thing");

        }

        [TestMethod]
        public void CheckServRequestTest()
        {
            // Assign 
            ConfigMockForDemoTest();
            string url = "http://localhost:" + MockServer.Ports[0];
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("TestHeader", "TestHeaderValue");

            // Act
            string getResponse1 = HttpClientHelper.Get(url + "/some/thing", headers);
            var customerRequest = MockServer.LogEntries
                .FirstOrDefault(e => e.RequestMessage.Path == "/some/thing");

            // Assert
            Assert.AreEqual(1, MockServer.LogEntries.Count(), "Requests count is incotrrect");
            Assert.IsTrue(customerRequest.RequestMessage.Headers.ContainsKey("TestHeader"));
            Assert.AreEqual("TestHeaderValue",
                customerRequest.RequestMessage.Headers["TestHeader"][0], "Header Value is incorrect!");
        }

        [TestMethod]
        public void CheckServRequestHandlingTest()
        {
            // Assign 
            ConfigMockForDemoTest();
            ConfigMochEventHandling();
            string url = "http://localhost:" + MockServer.Ports[0];
            Dictionary<string, string> headers = new Dictionary<string, string>();

            // Act
            string getResponse1 = HttpClientHelper.Get(url + "/some/thing", headers);
            string getResponse2 = HttpClientHelper.Get(url + "/some/thing/1", headers);
            string getResponse3 = HttpClientHelper.Get(url + "/some/thing/2", headers);

            // Assert
            Assert.AreEqual(3, MockServer.LogEntries.Count(), "Requests count is incotrrect");
        }
    }
}
