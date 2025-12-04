
# OpenAISharp

## Introduction
OpenAISharp is a C# library for interacting with the OpenAI API, providing simple and intuitive interfaces for sending chat requests, retrieving model lists, and more.

## Features
- Support for synchronous chat completion requests
- Support for streaming chat responses
- Available model list query
- Flexible configuration system
- Comprehensive error handling

## Quick Start

### 1. Install Dependencies
```bash
dotnet add package Newtonsoft.Json
```

### 2. Configuration
Create a configuration instance:
```csharp
var config = new Configuration
{
    ApiKey = "your-api-key",
    ApiUrl = "https://api.openai.com/v1/chat/completions",
    Model = "gpt-3.5-turbo"
};
```

### 3. Basic Usage

#### Initialize OpenAI Instance
```csharp
var openAI = new OpenAI(config);
```

#### Send Chat Request
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

#### Use Streaming Response
```csharp
var stream = openAI.completions_stream(request);
while (!stream.EndOfStream)
{
    var chunk = OpenAI.readStreamRender(stream);
    if (chunk == null) break;
    Console.Write(chunk.Choices[0].Delta.Content);
}
```

#### Get Available Models
```csharp
var models = openAI.getModels();
foreach (var model in models.Data)
{
    Console.WriteLine(model.Id);
}
```

## API Reference

### Configuration Class
- `ApiKey`: OpenAI API key
- `ApiUrl`: API endpoint URL
- `Model`: Default model name to use

### ChatCompletionRequest Class
- `Model`: Model to use (optional, defaults to model from configuration)
- `Messages`: List of conversation messages

### ChatCompletionResponse Class
- `Choices`: List of response choices
- `Choices[0].Message`: Response message content

## Notes
1. Ensure a valid API key is configured before use
2. It's recommended to implement proper error handling in production
3. Streaming responses require proper handling of data chunk reading and termination markers