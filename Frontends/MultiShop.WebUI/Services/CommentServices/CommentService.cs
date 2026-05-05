using MultiShop.DtoLayer.CommentDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CommentServices
{
    public class CommentService :ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCommentDto>("comments", createCommentDto);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _httpClient.DeleteAsync("comments?id=" + id);
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            var responseMessage = await _httpClient.GetAsync("comments");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
            return values;
        }

        public async Task<UpdateCommentDto> GetByIdCommentAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("comments/" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<UpdateCommentDto>(jsonData);
            return values;
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCommentDto>("comments", updateCommentDto);
        }

        public async Task ApproveCommentAsync(string id)
        {
            // Mevcut yorumu çek, Status=true yap, geri gönder
            var responseMessage = await _httpClient.GetAsync("comments/" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonData)) return;
            var comment = JsonConvert.DeserializeObject<UpdateCommentDto>(jsonData);
            if (comment == null) return;
            comment.Status = true;
            await _httpClient.PutAsJsonAsync<UpdateCommentDto>("comments", comment);
        }

        public async Task<List<ResultCommentDto>> CommentListByProductId(string id)
        {
            var responseMessage = await _httpClient.GetAsync($"comments/CommentListByProductId/{id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsonData);
            return values ?? new List<ResultCommentDto>();
        }

        public async Task<int> GetTotalCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("comments/GetTotalCommentCount");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonData)) return 0;
            return JsonConvert.DeserializeObject<int>(jsonData);
        }

        public async Task<int> GetActiveCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("comments/GetActiveCommentCount");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonData)) return 0;
            return JsonConvert.DeserializeObject<int>(jsonData);
        }

        public async Task<int> GetPassiveCommentCount()
        {
            var responseMessage = await _httpClient.GetAsync("comments/GetPassiveCommentCount");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonData)) return 0;
            return JsonConvert.DeserializeObject<int>(jsonData);
        }
    }
}
