using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TypedRest
{
    [TestFixture, Ignore("Server mock not implemented yet")]
    public class RestElementTest : RestEndpointTest
    {
        private IRestElement<MockEntity> _endpoint;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _endpoint = new RestElement<MockEntity>(EntryPoint, "endpoint");
        }

        [Test]
        public async Task TestRead()
        {
            //stubFor(get(urlEqualTo("/endpoint"))
            //        .withHeader("Accept", equalTo(jsonMime))
            //        .willReturn(aResponse()
            //                .withStatus(200)
            //                .withHeader("Content-Type", jsonMime)
            //                .withBody("{\"id\":5,\"name\":\"test\"}")));

            var result = await _endpoint.ReadAsync();
            result.Should().Be(new MockEntity {Id = 5, Name = "test"});
        }

        [Test]
        public async Task TestUpdate()
        {
            //stubFor(put(urlEqualTo("/endpoint"))
            //        .withRequestBody(equalToJson("{\"id\":5,\"name\":\"test\"}"))
            //        .willReturn(aResponse()
            //                .withStatus(204)));

            await _endpoint.UpdateAsync(new MockEntity {Id = 5, Name = "test"});
        }

        [Test]
        public async Task TestDelete()
        {
            //stubFor(delete(urlEqualTo("/endpoint"))
            //        .willReturn(aResponse()
            //                .withStatus(204)));

            await _endpoint.DeleteAsync();
        }
    }
}