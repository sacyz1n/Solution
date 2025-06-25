using Log.NetNotifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Log.NetNotifications
{
    /// <summary>
    /// 텔레그램 봇 관련 API Document Url : https://core.telegram.org/bots/api
    /// 봇 생성 과정 
    /// - @botFater 검색
    /// - /start (대화 시작)
    /// - /newbot (새 봇 생성)
    /// - /setprivacy disable 설정 (그룹 채팅을 받을 수 있도록 설정)
    /// </summary>
    public class TelegramNotification : INetNotifiication
    {
        private HttpClient _httpClient = null;

        // Telegram Bot Token
        private readonly string _botToken = $"8052627830:AAHN_kyYnTyZeWG3zpPIDarbDtxXgoByMc4";

        private readonly string _chatId = string.Empty;
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

        public async Task SendNotification(string message)
        {
            try
            {
                var sendMessageUrl = $"{_httpClient.BaseAddress}/sendMessage";
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "chat_id", _chatId }, // 채팅 ID
                    { "text", message }, // 메시지 내용
                });
                await _httpClient.PostAsync(sendMessageUrl, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TelegramNotification] Error sending notification: {ex.Message}");
            }
        }
    }
}
