using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DakarRally.IntregrationTests
{
    public class RallyControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public RallyControllerIntegrationTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateRace_SentWrongModel_ReturnsBadRequest()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Rally/CreateRace");
            postRequest.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(postRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Year is required", responseContent); 
        }

        [Fact]
        public async Task CreateRace_SentWrongModel_ReturnsRaceDTO()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Rally/CreateRace");
            postRequest.Content = new StringContent(JsonConvert.SerializeObject(new { year = 2020 }), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(postRequest);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("year", responseContent);
            Assert.Contains("id", responseContent);
        }

    }
}
