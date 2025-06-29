


using Client.Shared;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClientTest
{
    [TestClass]
    public sealed class AuthTest
    {
        private static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");
            return client;
        }

        [TestMethod]
        public async Task Login()
        {
            try
            {
                var client = GetHttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new LoginRequest()
                {
                    MemberId = "lee",
                };

                var data = JsonSerializer.Serialize(request);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{client.BaseAddress?.ToString()}login", content);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    Assert.Fail($"Login failed with status code: {response.StatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                int i = 0;
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to create HttpClient: {ex.Message}");
            }
        }
    }
}
