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
        public async Task<string> GetResponse(string input)
        {
            ChatClient chatClient = new AzureOpenAIClient(new Uri(ApiEndpoint), _apiCredential).GetChatClient(DeploymentName);

            var messages = new ChatMessage[]
            {
                    new SystemChatMessage("You are a financial advisor"),
                    new UserChatMessage($"Answer the following input: '{input}'")
            };
            var chatCompletionOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = 200,
            };
            ClientResult<ChatCompletion> result = await chatClient.CompleteChatAsync(messages, chatCompletionOptions);

            return result.Value.Content[0].Text;
        }
    }
}
