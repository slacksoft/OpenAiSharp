using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAISharp
{
    // Models.cs

    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    /// <summary>
    /// 配置文件对象
    /// </summary>
    class Setting
    {
        [JsonProperty("apiurl")]
        public string ApiUrl = "https://open.bigmodel.cn/api/paas/v4/chat/completions";
        [JsonProperty("apikey")]
        public string apiKey = "";
        [JsonProperty("model")]
        public string model = "GLM-4.5-Flash";
    }
    // ====================
    // 请求模型
    // ====================

    /// <summary>
    /// 聊天完成请求的根对象
    /// </summary>
    /// <summary>
    /// 聊天完成请求的根对象
    /// </summary>
    public class ChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; } = "";

        [JsonProperty("messages")]
        public List<MessageRequest> Messages { get; set; } = new List<MessageRequest>();

        [JsonProperty("temperature")]
        public double Temperature { get; set; } = 1.0;

        [JsonProperty("stream")]
        public bool Stream { get; set; } = false;
    }

    /// <summary>
    /// 请求中的消息对象
    /// </summary>
    public class MessageRequest
    {
        [JsonProperty("role")]
        public string Role { get; set; } // "system", "user", "assistant"

        [JsonProperty("content")]
        public string Content { get; set; }
    }


    // ====================
    // 响应模型
    // ====================

    /// <summary>
    /// 聊天完成响应的根对象
    /// </summary>
    public class ChatCompletionResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("request_id")]
        public string RequestId { get; set; } = string.Empty;

        [JsonProperty("created")]
        public long Created { get; set; }

        /// <summary>
        /// AI模型，如果不手动设置默认采用全局配置文件指定模型
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("choices")]
        public List<Choice> Choices { get; set; } = new List<Choice>();

        [JsonProperty("usage")]
        public Usage Usage { get; set; } = new Usage();

        // 其他可选字段根据需要添加
        [JsonProperty("video_result")]
        public List<VideoResult>? VideoResult { get; set; }

        [JsonProperty("web_search")]
        public List<WebSearch>? WebSearch { get; set; }

        [JsonProperty("content_filter")]
        public List<ContentFilter>? ContentFilter { get; set; }
    }

    public class Choice
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("message")]
        public ResponseMessage Message { get; set; } = new ResponseMessage();

        [JsonProperty("finish_reason")]
        public string? FinishReason { get; set; }
    }

    public class ResponseMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;

        [JsonProperty("content")]
        public string Content { get; set; } = string.Empty;

        [JsonProperty("reasoning_content")]
        public string? ReasoningContent { get; set; }

        // tool_calls, audio 等字段根据需要添加
        [JsonProperty("tool_calls")]
        public List<ToolCall>? ToolCalls { get; set; }

        [JsonProperty("audio")]
        public Audio? Audio { get; set; }

        /// <summary>
        /// 转换成对话记录
        /// </summary>
        /// <returns></returns>
        public MessageRequest toMessageRequest() {
            return new MessageRequest {Role = this.Role,Content = this.Content };
        }
    }

    public class Usage
    {
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }

        [JsonProperty("prompt_tokens_details")]
        public PromptTokensDetails PromptTokensDetails { get; set; } = new PromptTokensDetails();
    }

    public class PromptTokensDetails
    {
        [JsonProperty("cached_tokens")]
        public int CachedTokens { get; set; }
    }

    // 为了完整性，添加其他响应中用到的模型类
    public class Audio { [JsonProperty("id")] public string Id { get; set; } = string.Empty; [JsonProperty("data")] public string Data { get; set; } = string.Empty; [JsonProperty("expires_at")] public string ExpiresAt { get; set; } = string.Empty; }
    public class ToolCall { [JsonProperty("id")] public string Id { get; set; } = string.Empty; [JsonProperty("type")] public string Type { get; set; } = string.Empty; [JsonProperty("function")] public Function Function { get; set; } = new Function(); [JsonProperty("mcp")] public Mcp? Mcp { get; set; } }
    public class Function { [JsonProperty("name")] public string Name { get; set; } = string.Empty; [JsonProperty("arguments")] public string Arguments { get; set; } = string.Empty; }
    public class Mcp { /* ... 省略复杂嵌套 ... */ }
    public class VideoResult { [JsonProperty("url")] public string Url { get; set; } = string.Empty; [JsonProperty("cover_image_url")] public string CoverImageUrl { get; set; } = string.Empty; }
    public class WebSearch { [JsonProperty("icon")] public string Icon { get; set; } = string.Empty; [JsonProperty("title")] public string Title { get; set; } = string.Empty; /* ... */ }
    public class ContentFilter { [JsonProperty("role")] public string Role { get; set; } = string.Empty; [JsonProperty("level")] public int Level { get; set; } }


    public class ModelListResponse
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("data")]
        public List<ModelInfo> Data { get; set; }
    }

    public class ModelInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("owned_by")]
        public string OwnedBy { get; set; }
    }





}
