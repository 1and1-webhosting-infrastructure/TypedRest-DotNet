using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Xunit;

namespace TypedRest
{
    [Collection("Endpoint")]
    public class ProducerEndpointTest : EndpointTestBase
    {
        private readonly IProducerEndpoint<MockEntity> _endpoint;

        public ProducerEndpointTest()
        {
            _endpoint = new ProducerEndpoint<MockEntity>(EntryEndpoint, "endpoint");
        }

        [Fact]
        public async Task TestInvoke()
        {
            Mock.Expect(HttpMethod.Post, "http://localhost/endpoint")
                .Respond(JsonMime, "{\"id\":2,\"name\":\"result\"}");

            var result = await _endpoint.InvokeAsync();
            result.Should().Be(new MockEntity(2, "result"));
        }
    }
}