# OpenAISharp
一个简单、轻量级的 C# 库，用于方便地与 OpenAI 兼容的 API 进行交互。本项目封装了聊天完成和模型列表获取的核心功能，旨在简化在 C# 项目中集成 AI 能力的流程。
## 功能特性
- ✨ **聊天完成 (Chat Completions)**: 发送对话请求并获取 AI 模型的响应。
- 📋 **模型列表 (List Models)**: 获取 API 服务商提供的所有可用模型列表。
- ⚙️ **配置驱动**: 通过 `setting.json` 文件轻松管理 API 密钥、端点和默认模型。
- 🚀 **开箱即用**: 首次运行时自动生成配置文件模板，引导用户完成设置。
## 待开发
- ❌ **流式对话**: 以异步的形式，流式输出对话。
- ❌ **动态读取配置**: 当前用配置文件的方式不适合动态操控配置。
## 快速开始
### 1. 配置设置文件
本库依赖于一个名为 `setting.json` 的配置文件来存储您的 API 信息。
1.  **首次运行**: 当您的应用程序第一次调用 `OpenAISharp` 时，它会在可执行文件的相同目录下自动创建一个 `setting.json` 文件，并提示您进行配置。
2.  **配置内容**: 打开生成的 `setting.json` 文件，并根据您的 API 服务商信息进行修改。文件内容如下：
    ```json
    {
      "apiKey": "你的API密钥",
      "ApiUrl": "https://api.openai.com/v1/chat/completions",
      "model": "gpt-3.5-turbo"
    }
    ```
    **字段说明:**
    - `apiKey`: 您的 API 密钥，这是身份验证的必需项。
    - `ApiUrl`: API 的完整端点 URL。
        - 对于官方 OpenAI API，通常为 `https://api.openai.com/v1/chat/completions`。
        - 对于其他兼容服务（如 Azure OpenAI 或其他代理），请使用服务商提供的 URL。
    - `model`: 您希望默认使用的模型名称（例如 `gpt-3.5-turbo`, `gpt-4`）。在代码中也可以动态指定。
### 2. 安装依赖
您的项目需要安装 `Newtonsoft.Json` 包来处理 JSON 序列化和反序列化。
您可以通过 NuGet 包管理器控制台安装：
```powershell
Install-Package Newtonsoft.Json
```
或者在您的 `.csproj` 文件中添加以下 `ItemGroup`：
```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
</ItemGroup>
```
### 3. 在代码中使用
#### 初始化 `AIModel`
创建 `AIModel` 类的实例。它会自动加载 `setting.json` 文件。
```csharp
using OpenAISharp;
// 创建 AIModel 实例
var ai = new AIModel();
```
#### 发送聊天请求
创建一个 `ChatCompletionRequest` 对象，填充对话内容，然后调用 `Request` 方法。
```csharp
// 1. 准备请求
var request = new ChatCompletionRequest
{
    // 如果不指定 Model，将使用 setting.json 中的默认模型
    // Model = "gpt-4", 
    Messages = new List<Message>
    {
        new Message { Role = "system", Content = "你是一个有帮助的助手。" },
        new Message { Role = "user", Content = "用一句话解释什么是量子计算。" }
    },
    Temperature = 0.7,
    MaxTokens = 150
};
// 2. 发送请求并获取响应
try
{
    ChatCompletionResponse response = ai.Request(request);
    // 3. 处理响应
    if (response.Choices != null && response.Choices.Count > 0)
    {
        string reply = response.Choices[0].Message.Content;
        Console.WriteLine($"AI 回复: {reply}");
    }
    else
    {
        Console.WriteLine("未收到有效回复。");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"发生错误: {ex.Message}");
}
```
#### 获取可用模型列表
调用 `getModels` 方法来获取所有可用的模型。
```csharp
try
{
    ModelListResponse modelsResponse = ai.getModels();
    if (modelsResponse.Data != null)
    {
        Console.WriteLine("可用模型列表:");
        foreach (var model in modelsResponse.Data)
        {
            Console.WriteLine($"- ID: {model.Id}, Owned By: {model.OwnedBy}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"获取模型列表失败: {ex.Message}");
}
```

## 注意事项
- **同步调用**: 当前 `Request` 和 `getModels` 方法是同步实现的，使用了 `.GetAwaiter().GetResult()`。这在 UI 线程或某些同步上下文中可能导致死锁。如果您的项目是异步的（如 ASP.NET Core、WPF/WinForms UI），建议创建异步版本的方法（`RequestAsync`, `getModelsAsync`）以获得更好的性能和稳定性。
- **HttpClient 管理**: 当前代码在每次调用时都创建了一个新的 `HttpClient` 实例。对于高频调用，这可能导致端口耗尽问题。推荐的做法是使用静态 `HttpClient` 或依赖注入框架（如 .NET Core 的 `IHttpClientFactory`）来管理 `HttpClient` 的生命周期。
- **错误处理**: 当前的错误处理比较基础，主要是在控制台输出错误信息。在生产环境中，您可能需要更健壮的异常处理和日志记录机制。