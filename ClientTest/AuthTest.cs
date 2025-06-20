


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
        public Task Login()
        {
            try
            {
                var client = GetHttpClient();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to create HttpClient: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
