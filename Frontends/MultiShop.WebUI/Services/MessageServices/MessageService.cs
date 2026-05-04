using MultiShop.DtoLayer.MessageDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultInboxMessageDto>> GetInboxMessageAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("http://localhost:5000/services/Message/UserMessage/GetMessageInbox?id=" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultInboxMessageDto>>(jsonData);
            return values ?? new List<ResultInboxMessageDto>();
        }

        public async Task<List<ResultSendboxMessageDto>> GetSendboxMessageAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("http://localhost:5000/services/Message/UserMessage/GetMessageSendbox?id=" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultSendboxMessageDto>>(jsonData);
            return values ?? new List<ResultSendboxMessageDto>();
        }

        public async Task<int> GetTotalMessageCountByReceiverId(string id)
        {
            var responseMessage = await _httpClient.GetAsync("UserMessage/GetTotalMessageCountByReceiverId?id=" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonData)) return 0;
            return JsonConvert.DeserializeObject<int>(jsonData);
        }
    }
}
