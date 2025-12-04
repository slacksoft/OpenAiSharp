
我将根据代码库内容生成一个结构清晰的README文档：

# OpenAISharp

## 简介
OpenAISharp 是一个用于与 OpenAI API 交互的 C# 库，提供了简单易用的接口来发送聊天请求、获取模型列表等功能。

## 功能特性
- 支持同步聊天完成请求
- 支持流式聊天响应
- 提供可用模型列表查询
- 灵活的配置系统
- 完善的错误处理机制

## 快速开始

### 1. 安装依赖
```bash
dotnet add package Newtonsoft.Json
```

### 2. 配置设置
创建配置实例：
```csharp
var config = new Configuration
{
    ApiKey = "your-api-key",
    ApiUrl = "https://api.openai.com/v1/chat/completions",
    Model = "gpt-3.5-turbo"
};
```

### 3. 基本使用

#### 初始化 OpenAI 实例
```csharp
var openAI = new OpenAI(config);
```

#### 发送聊天请求
```csharp
var request = new ChatCompletionRequest
{
    Messages = new List<Message>
    {
        new Message { Role = "user", Content = "Hello!" }
    }
};

var response = openAI.completions(request);
Console.WriteLine(response.Choices[0].Message.Content);
```

#### 使用流式响应
```csharp
var stream = openAI.completions_stream(request);
while (!stream.EndOfStream)
{
    var chunk = OpenAI.readStreamRender(stream);
    if (chunk == null) break;
    Console.Write(chunk.Choices[0].Delta.Content);
}
```

#### 获取可用模型列表
```csharp
var models = openAI.getModels();
foreach (var model in models.Data)
{
    Console.WriteLine(model.Id);
}
```

## API 参考

### Configuration 类
- `ApiKey`: OpenAI API 密钥
- `ApiUrl`: API 端点 URL
- `Model`: 默认使用的模型名称

### ChatCompletionRequest 类
- `Model`: 使用的模型（可选，默认使用配置中的模型）
- `Messages`: 对话消息列表

### ChatCompletionResponse 类
- `Choices`: 响应选项列表
- `Choices[0].Message`: 响应消息内容

## 注意事项
1. 使用前请确保已配置有效的 API 密钥
2. 建议在生产环境中使用适当的错误处理
3. 流式响应需要正确处理数据块读取和结束标记