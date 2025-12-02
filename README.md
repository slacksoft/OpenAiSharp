# OpenAISharp
A simple, lightweight C# library for easily interacting with OpenAI-compatible APIs. This project wraps the core functionalities of chat completions and model listing, aiming to streamline the process of integrating AI capabilities into your C# projects.
## Features
- ✨ **Chat Completions**: Send conversation requests and get responses from AI models.
- 📋 **List Models**: Retrieve a list of all available models provided by the API service.
- ⚙️ **Configuration-Driven**: Easily manage your API key, endpoint, and default model through a `setting.json` file.
- 🚀 **Ready to Use**: Automatically generates a configuration file template on first run to guide you through the setup.
## To-Do
- ❌ **Streaming Chat**: Implement asynchronous, streaming output for conversations.
- ❌ **Dynamic Configuration**: The current file-based configuration is not suitable for dynamic configuration changes.
## Quick Start
### 1. Configure the Settings File
This library relies on a `setting.json` file to store your API information.
1.  **First Run**: When your application calls `OpenAISharp` for the first time, it will automatically create a `setting.json` file in the same directory as the executable and prompt you to configure it.
2.  **File Content**: Open the generated `setting.json` file and modify it according to your API service provider's information. The file content is as follows:
    ```json
    {
      "apiKey": "YOUR_API_KEY",
      "ApiUrl": "https://api.openai.com/v1/chat/completions",
      "model": "gpt-3.5-turbo"
    }
    ```
    **Field Descriptions:**
    - `apiKey`: Your API key, which is required for authentication.
    - `ApiUrl`: The full endpoint URL for the API.
        - For the official OpenAI API, this is typically `https://api.openai.com/v1/chat/completions`.
        - For other compatible services (like Azure OpenAI or other proxies), use the URL provided by your service.
    - `model`: The name of the model you want to use by default (e.g., `gpt-3.5-turbo`, `gpt-4`). This can also be specified dynamically in your code.
### 2. Install Dependencies
Your project needs the `Newtonsoft.Json` package to handle JSON serialization and deserialization.
You can install it via the NuGet Package Manager Console:
```powershell
Install-Package Newtonsoft.Json
```
Or add the following `ItemGroup` to your `.csproj` file:
```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```
### 3. Use in Code
#### Initialize `AIModel`
Create an instance of the `AIModel` class. It will automatically load the `setting.json` file.
```csharp
using OpenAISharp;
// Create an AIModel instance
var ai = new AIModel();
```
#### Send a Chat Request
Create a `ChatCompletionRequest` object, populate it with the conversation content, and then call the `Request` method.
```csharp
// 1. Prepare the request
var request = new ChatCompletionRequest
{
    // If Model is not specified, the default model from setting.json will be used
    // Model = "gpt-4", 
    Messages = new List<Message>
    {
        new Message { Role = "system", Content = "You are a helpful assistant." },
        new Message { Role = "user", Content = "Explain quantum computing in one sentence." }
    },
    Temperature = 0.7,
    MaxTokens = 150
};
// 2. Send the request and get the response
try
{
    ChatCompletionResponse response = ai.Request(request);
    // 3. Process the response
    if (response.Choices != null && response.Choices.Count > 0)
    {
        string reply = response.Choices[0].Message.Content;
        Console.WriteLine($"AI Reply: {reply}");
    }
    else
    {
        Console.WriteLine("No valid reply received.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
```
#### Get Available Model List
Call the `getModels` method to retrieve all available models.
```csharp
try
{
    ModelListResponse modelsResponse = ai.getModels();
    if (modelsResponse.Data != null)
    {
        Console.WriteLine("Available Models:");
        foreach (var model in modelsResponse.Data)
        {
            Console.WriteLine($"- ID: {model.Id}, Owned By: {model.OwnedBy}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to get model list: {ex.Message}");
}
```
## Notes
- **Synchronous Calls**: The current `Request` and `getModels` methods are implemented synchronously, using `.GetAwaiter().GetResult()`. This can lead to deadlocks in UI threads or certain synchronization contexts. If your project is asynchronous (e.g., ASP.NET Core, WPF/WinForms UI), it is recommended to create asynchronous versions of these methods (`RequestAsync`, `getModelsAsync`) for better performance and stability.
- **HttpClient Management**: The current code creates a new `HttpClient` instance for each call. For high-frequency calls, this can lead to socket exhaustion. The recommended practice is to use a static `HttpClient` or a dependency injection framework (like .NET Core's `IHttpClientFactory`) to manage the `HttpClient`'s lifecycle.
- **Error Handling**: The current error handling is basic, primarily outputting error messages to the console. In a production environment, you may need a more robust exception handling and logging mechanism.