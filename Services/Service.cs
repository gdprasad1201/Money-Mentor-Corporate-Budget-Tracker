using System.ClientModel;
using System.Text.Json.Nodes;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Text.Json;

namespace Expense_Tracker.Services
{
    public class OpenAIService
    {
        private readonly string _apiKey;
        private const string ApiEndpoint = "https://fall2024-gdprasad-final-openai2.openai.azure.com/";
        private const string DeploymentName = "gpt-35-turbo";
        private ApiKeyCredential _apiCredential;
        public OpenAIService(IConfiguration configuration)
        {
            _apiKey = configuration["ApiSettings:ApiKey"] ?? throw new Exception("OpenAI API key not found in the configuration.");
            _apiCredential = new(_apiKey);
        }
        public async Task<List<(string Review, double Sentiment)>> GetResponse()
        {
            var reviewsWithSentiment = new List<(string Review, double Sentiment)>();
            ChatClient chatClient = new AzureOpenAIClient(new Uri(ApiEndpoint), _apiCredential).GetChatClient(DeploymentName);

            string[] personas = { "is harsh", "loves romance", "loves comedy", "loves thrillers", "loves fantasy", "is a sci-fi fan", "adores historical dramas", "enjoys indie films", "loves action-packed blockbusters", "appreciates artistic and experimental films" };
            var reviews = new List<string>();
            foreach (string persona in personas)
            {
                var messages = new ChatMessage[]
                {
                    new SystemChatMessage($"You are a film reviewer and film critic who {persona}."),
                    new UserChatMessage($"How would you rate the movie out of 10 in less than 105 words?")
                };
                var chatCompletionOptions = new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 200,
                };
                ClientResult<ChatCompletion> result = await chatClient.CompleteChatAsync(messages, chatCompletionOptions);

                reviews.Add(result.Value.Content[0].Text);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            return reviewsWithSentiment;
        }
    }
}
