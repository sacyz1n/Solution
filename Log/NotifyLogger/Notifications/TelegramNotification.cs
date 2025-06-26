using System.Text.Json;

namespace Log
{
    /// <summary>
    /// 텔레그램 봇 관련 API Document Url : https://core.telegram.org/bots/api
    /// 봇 생성 과정 
    /// - @botFater 검색
    /// - /start (대화 시작)
    /// - /newbot (새 봇 생성)
    /// - /setprivacy disable 설정 (그룹 채팅을 받을 수 있도록 설정)
    /// </summary>
    public class TelegramNotification : INotifiication
    {
        private HttpClient _httpClient = null;

        // Telegram Bot Token
        private readonly string _botToken = $"8052627830:AAHN_kyYnTyZeWG3zpPIDarbDtxXgoByMc4";

        private readonly string _chatId = $"-1002702140444";
        // DEV_T1_BOT
        // 8052627830:AAGs9_x8E3kcf3OVItDZUp-8PZoWbziZSe4

        public TelegramNotification()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri($"https://api.telegram.org/bot{_botToken}");
        }

        private async Task updateChadId()
        {
            if (string.IsNullOrEmpty(_chatId))
            {
                // 웹 훅 제거 
                var deleteWebHookUrl = $"{_httpClient.BaseAddress}/deleteWebhook";
                var deleteWebHookResponse = await _httpClient.GetAsync(deleteWebHookUrl);

                var chatListUrl = $"{_httpClient.BaseAddress}/getUpdates";

                var response = await _httpClient.GetStringAsync(chatListUrl);

                using var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                if (root.GetProperty("ok").GetBoolean())
                {
                    foreach (var update in root.GetProperty("result").EnumerateArray())
                    {
                        var msg = update.GetProperty("message");
                        var chatId = msg.GetProperty("chat").GetProperty("id").GetInt64();

                        Console.WriteLine($"chat_id: {chatId}");
                    }
                }
            }
        }

        public async Task SendNotification(int eventId, string message)
        {
            try
            {
                var sendMessageUrl = $"{_httpClient.BaseAddress}/sendMessage?chat_id={_chatId}&text={message}";

                if (LoggerExtensions.NotifyLoggerResultSync == eventId)
                {
                    _httpClient.GetAsync(sendMessageUrl)
                               .ConfigureAwait(false)
                               .GetAwaiter()
                               .GetResult();
                }
                else
                {
                    await _httpClient.GetAsync(sendMessageUrl)
                               .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TelegramNotification] Error sending notification: {ex.Message}");
            }
        }
    }
}
