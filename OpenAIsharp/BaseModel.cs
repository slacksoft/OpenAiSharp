using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenAISharp
{
    // ====================
    // Configuration / 配置类
    // ====================

    /// <summary>
    /// Configuration file object / 配置文件对象
    /// Contains API settings and model configuration / 包含API设置和模型配置
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// API endpoint URL / API端点URL
        /// Default: "https://open.bigmodel.cn/api/paas/v4/chat/completions"
        /// </summary>
        [JsonProperty("apiurl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ApiUrl = "https://open.bigmodel.cn/api/paas/v4/chat/completions";

        /// <summary>
        /// API authentication key / API认证密钥
        /// Default: empty string
        /// </summary>
        [JsonProperty("apikey", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ApiKey = "";

        /// <summary>
        /// Default model name / 默认模型名称
        /// Default: "GLM-4.5-Flash"
        /// </summary>
        [JsonProperty("model", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Model = "GLM-4.5-Flash";
    }

    // ====================
    // Request Models / 请求模型
    // ====================

    /// <summary>
    /// Function parameters object / 函数参数对象
    /// Contains parameters for function calls / 包含函数调用参数
    /// </summary>
    public class FunctionParameters
    {
        /// <summary>
        /// JSON object containing function parameters / 包含函数参数的JSON对象
        /// </summary>
        [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JObject Parameters { get; set; }
    }

    /// <summary>
    /// Response format object / 响应格式对象
    /// Defines the format of the response / 定义响应格式
    /// </summary>
    public class ResponseFormat
    {
        /// <summary>
        /// Type of response format / 响应格式类型
        /// Default: "text" / 默认："text"
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; } = "text";
    }

    /// <summary>
    /// Tool function object / 工具函数对象
    /// Represents a function that can be called by the model / 表示模型可以调用的函数
    /// </summary>
    public class ToolFunction
    {
        /// <summary>
        /// Function name / 函数名称
        /// </summary>
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Function description / 函数描述
        /// </summary>
        [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Function parameters in JSON format / JSON格式的函数参数
        /// Default: empty object / 默认：空对象
        /// </summary>
        [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JObject Parameters { get; set; } = new JObject();

        /// <summary>
        /// Whether to use strict mode for parameter validation / 是否使用严格模式进行参数验证
        /// Default: false / 默认：false
        /// </summary>
        [JsonProperty("strict", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Strict { get; set; } = false;
    }

    /// <summary>
    /// Tool object / 工具对象
    /// Represents a tool that can be used by the model / 表示模型可以使用的工具
    /// </summary>
    public class ToolUnit
    {
        /// <summary>
        /// Type of tool / 工具类型
        /// Default: "function" / 默认："function"
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; } = "function";

        /// <summary>
        /// Function definition for the tool / 工具的函数定义
        /// </summary>
        [JsonProperty("function", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ToolFunction Function { get; set; }
    }

    /// <summary>
    /// Chat completion request object / 聊天完成请求对象
    /// Contains all parameters for generating a response / 包含生成响应的所有参数
    /// </summary>
    public class ChatCompletionRequest
    {
        /// <summary>
        /// Model name to use for completion / 用于完成请求的模型名称
        /// Default: empty string / 默认：空字符串
        /// </summary>
        [JsonProperty("model", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Model { get; set; } = "";

        /// <summary>
        /// List of messages in the conversation / 对话中的消息列表
        /// Each message contains role and content / 每条消息包含角色和内容
        /// Default: empty list / 默认：空列表
        /// </summary>
        [JsonProperty("messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<MessageRequest> Messages { get; set; } = new List<MessageRequest>();

        /// <summary>
        /// Maximum number of tokens in the response / 响应中的最大令牌数
        /// Default: 4096 / 默认：4096
        /// </summary>
        [JsonProperty("max_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaxTokens { get; set; }

        /// <summary>
        /// Enable thinking mode for the model / 启用模型的思考模式
        /// Default: false / 默认：false
        /// </summary>
        [JsonProperty("enable_thinking", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool EnableThinking { get; set; }

        /// <summary>
        /// Number of tokens allocated for thinking / 分配给思考的令牌数
        /// Default: 4096 / 默认：4096
        /// </summary>
        [JsonProperty("thinking_budget", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ThinkingBudget { get; set; }

        /// <summary>
        /// Minimum probability threshold for token sampling / 令牌采样的最小概率阈值
        /// Lower values result in more diverse outputs / 较低的值会产生更多样化的输出
        /// Default: 0.05 / 默认：0.05
        /// </summary>
        [JsonProperty("min_p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double MinP { get; set; }

        /// <summary>
        /// Stop sequences for generation / 生成停止序列
        /// Default: empty list / 默认：空列表
        /// </summary>
        [JsonProperty("stop", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Stop { get; set; } = new List<string>();

        /// <summary>
        /// Controls randomness in output / 控制输出的随机性
        /// Range: 0.0 to 2.0 / 范围：0.0到2.0
        /// Default: 0.7 / 默认：0.7
        /// </summary>
        [JsonProperty("temperature", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double Temperature { get; set; }

        /// <summary>
        /// Controls diversity via nucleus sampling / 通过核采样控制多样性
        /// Lower values result in more focused outputs / 较低的值会产生更集中的输出
        /// Range: 0.0 to 1.0 / 范围：0.0到1.0
        /// Default: 0.7 / 默认：0.7
        /// </summary>
        [JsonProperty("top_p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double TopP { get; set; }

        /// <summary>
        /// Limits vocabulary to top K tokens / 将词汇限制在前K个令牌
        /// Lower values result in more conservative outputs / 较低的值会产生更保守的输出
        /// Default: 50 / 默认：50
        /// </summary>
        [JsonProperty("top_k", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TopK { get; set; }

        /// <summary>
        /// Reduces repetition by penalizing frequent tokens / 通过惩罚频繁令牌来减少重复
        /// Range: -2.0 to 2.0 / 范围：-2.0到2.0
        /// Default: 0.5 / 默认：0.5
        /// </summary>
        [JsonProperty("frequency_penalty", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double FrequencyPenalty { get; set; }

        /// <summary>
        /// Number of completions to generate / 要生成的完成数量
        /// Default: 1 / 默认：1
        /// </summary>
        [JsonProperty("n", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int N { get; set; }

        /// <summary>
        /// Format of the response / 响应格式
        /// Default: new ResponseFormat() / 默认：新的响应格式对象
        /// </summary>
        [JsonProperty("response_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseFormat ResponseFormat { get; set; } = new ResponseFormat();

        /// <summary>
        /// List of available tools / 可用工具列表
        /// Default: empty list / 默认：空列表
        /// </summary>
        [JsonProperty("tools", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ToolUnit> Tools { get; set; } = new List<ToolUnit>();

        /// <summary>
        /// Enable streaming response / 启用流式响应
        /// When true, responses are sent as they are generated / 为true时，响应在生成时发送
        /// Default: false / 默认：false
        /// </summary>
        [JsonProperty("stream", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Stream { get; set; } = false;
    }

    /// <summary>
    /// Message object in the request / 请求中的消息对象
    /// </summary>
    public class MessageRequest
    {
        /// <summary>
        /// Role of the message sender / 消息发送者的角色
        /// Values: "system", "user", "assistant" / 值："system"、"user"、"assistant"
        /// </summary>
        [JsonProperty("role", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Role { get; set; }

        /// <summary>
        /// Content of the message / 消息内容
        /// </summary>
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Content { get; set; }
    }

    // ====================
    // Response Models / 响应模型
    // ====================

    /// <summary>
    /// Chat completion response object / 聊天完成响应对象
    /// Root object containing the complete response / 包含完整响应的根对象
    /// </summary>
    public class ChatCompletionResponse
    {
        /// <summary>
        /// Unique identifier for the response / 响应的唯一标识符
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Request identifier / 请求标识符
        /// </summary>
        [JsonProperty("request_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of response creation / 响应创建时间戳
        /// </summary>
        [JsonProperty("created", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Created { get; set; }

        /// <summary>
        /// Model used for the response / 用于响应的模型
        /// If not set, uses the model from global configuration / 如果未设置，使用全局配置中的模型
        /// </summary>
        [JsonProperty("model", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// List of response choices / 响应选择列表
        /// </summary>
        [JsonProperty("choices", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<Choice> Choices { get; set; } = new List<Choice>();

        /// <summary>
        /// Token usage information / 令牌使用信息
        /// </summary>
        [JsonProperty("usage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Usage Usage { get; set; } = new Usage();

        /// <summary>
        /// Video result information / 视频结果信息
        /// </summary>
        [JsonProperty("video_result", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<VideoResult>? VideoResult { get; set; }

        /// <summary>
        /// Web search results / 网络搜索结果
        /// </summary>
        [JsonProperty("web_search", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<WebSearch>? WebSearch { get; set; }

        /// <summary>
        /// Content filter results / 内容过滤器结果
        /// </summary>
        [JsonProperty("content_filter", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ContentFilter>? ContentFilter { get; set; }
    }

    /// <summary>
    /// Choice object in the response / 响应中的选择对象
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// Index of the choice / 选择的索引
        /// </summary>
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Index { get; set; }

        /// <summary>
        /// Delta object for streaming responses / 流式响应的增量对象
        /// </summary>
        [JsonProperty("delta", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Delta Delta { get; set; }

        /// <summary>
        /// Message content / 消息内容
        /// </summary>
        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseMessage Message { get; set; } = new ResponseMessage();

        /// <summary>
        /// Reason for completion finish / 完成结束的原因
        /// </summary>
        [JsonProperty("finish_reason", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? FinishReason { get; set; }
    }

    /// <summary>
    /// Response message object / 响应消息对象
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// Role of the message sender / 消息发送者的角色
        /// </summary>
        [JsonProperty("role", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Content of the message / 消息内容
        /// </summary>
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Reasoning content of the message / 消息的推理内容
        /// </summary>
        [JsonProperty("reasoning_content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ReasoningContent { get; set; }

        /// <summary>
        /// Tool calls made in the message / 消息中进行的工具调用
        /// </summary>
        [JsonProperty("tool_calls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ToolCall>? ToolCalls { get; set; }

        /// <summary>
        /// Audio content in the message / 消息中的音频内容
        /// </summary>
        [JsonProperty("audio", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Audio? Audio { get; set; }

        /// <summary>
        /// Convert to conversation record / 转换为对话记录
        /// </summary>
        /// <returns>MessageRequest object / MessageRequest对象</returns>
        public MessageRequest toMessageRequest()
        {
            return new MessageRequest { Role = this.Role, Content = this.Content };
        }
    }

    /// <summary>
    /// Token usage information / 令牌使用信息
    /// </summary>
    public class Usage
    {
        /// <summary>
        /// Number of tokens in the prompt / 提示中的令牌数
        /// </summary>
        [JsonProperty("prompt_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int PromptTokens { get; set; }

        /// <summary>
        /// Number of tokens in the completion / 完成中的令牌数
        /// </summary>
        [JsonProperty("completion_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CompletionTokens { get; set; }

        /// <summary>
        /// Total number of tokens used / 使用的令牌总数
        /// </summary>
        [JsonProperty("total_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TotalTokens { get; set; }

        /// <summary>
        /// Details about prompt tokens / 提示令牌的详细信息
        /// </summary>
        [JsonProperty("prompt_tokens_details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PromptTokensDetails PromptTokensDetails { get; set; } = new PromptTokensDetails();

        /// <summary>
        /// Details about completion tokens / 完成令牌的详细信息
        /// </summary>
        [JsonProperty("completion_tokens_details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CompletionTokensDetails CompletionTokensDetails { get; set; }
    }

    /// <summary>
    /// Prompt tokens details / 提示令牌详细信息
    /// </summary>
    public class PromptTokensDetails
    {
        /// <summary>
        /// Number of cached tokens / 缓存令牌的数量
        /// </summary>
        [JsonProperty("cached_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CachedTokens { get; set; }
    }

    /// <summary>
    /// Completion tokens details / 完成令牌详细信息
    /// </summary>
    public class CompletionTokensDetails
    {
        /// <summary>
        /// Number of reasoning tokens / 推理令牌的数量
        /// </summary>
        [JsonProperty("reasoning_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ReasoningTokens { get; set; }
    }

    // ====================
    // Additional Response Models / 其他响应模型
    // ====================

    /// <summary>
    /// Audio content object / 音频内容对象
    /// </summary>
    public class Audio
    {
        /// <summary>
        /// Audio identifier / 音频标识符
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Audio data / 音频数据
        /// </summary>
        [JsonProperty("data", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Audio expiration time / 音频过期时间
        /// </summary>
        [JsonProperty("expires_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExpiresAt { get; set; } = string.Empty;
    }

    /// <summary>
    /// Tool call object / 工具调用对象
    /// </summary>
    public class ToolCall
    {
        /// <summary>
        /// Tool call identifier / 工具调用标识符
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Type of tool call / 工具调用类型
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Function called / 调用的函数
        /// </summary>
        [JsonProperty("function", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Function Function { get; set; } = new Function();

        /// <summary>
        /// MCP (Model Context Protocol) information / MCP（模型上下文协议）信息
        /// </summary>
        [JsonProperty("mcp", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Mcp? Mcp { get; set; }
    }

    /// <summary>
    /// Function object / 函数对象
    /// </summary>
    public class Function
    {
        /// <summary>
        /// Function name / 函数名称
        /// </summary>
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Function arguments / 函数参数
        /// </summary>
        [JsonProperty("arguments", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Arguments { get; set; } = string.Empty;
    }

    /// <summary>
    /// MCP (Model Context Protocol) object / MCP（模型上下文协议）对象
    /// Complex nested structure / 复杂嵌套结构
    /// </summary>
    public class Mcp { /* ... complex nesting omitted ... */ }

    /// <summary>
    /// Video result object / 视频结果对象
    /// </summary>
    public class VideoResult
    {
        /// <summary>
        /// Video URL / 视频URL
        /// </summary>
        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Cover image URL / 封面图片URL
        /// </summary>
        [JsonProperty("cover_image_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CoverImageUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Web search result object / 网络搜索结果对象
    /// </summary>
    public class WebSearch
    {
        /// <summary>
        /// Search result icon / 搜索结果图标
        /// </summary>
        [JsonProperty("icon", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// Search result title / 搜索结果标题
        /// </summary>
        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Title { get; set; } = string.Empty;
        /* ... other properties omitted ... */
    }

    /// <summary>
    /// Content filter result object / 内容过滤器结果对象
    /// </summary>
    public class ContentFilter
    {
        /// <summary>
        /// Filter role / 过滤器角色
        /// </summary>
        [JsonProperty("role", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Filter level / 过滤器级别
        /// </summary>
        [JsonProperty("level", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Level { get; set; }
    }

    // ====================
    // Model List Response / 模型列表响应
    // ====================

    /// <summary>
    /// Model list response object / 模型列表响应对象
    /// </summary>
    public class ModelListResponse
    {
        /// <summary>
        /// Object type / 对象类型
        /// </summary>
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; set; }

        /// <summary>
        /// List of model information / 模型信息列表
        /// </summary>
        [JsonProperty("data", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<ModelInfo> Data { get; set; }
    }

    /// <summary>
    /// Model information object / 模型信息对象
    /// </summary>
    public class ModelInfo
    {
        /// <summary>
        /// Model identifier / 模型标识符
        /// </summary>
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Object type / 对象类型
        /// </summary>
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; set; }

        /// <summary>
        /// Model creation timestamp / 模型创建时间戳
        /// </summary>
        [JsonProperty("created", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long Created { get; set; }

        /// <summary>
        /// Model owner / 模型所有者
        /// </summary>
        [JsonProperty("owned_by", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OwnedBy { get; set; }
    }

    // ====================
    // Streaming Response / 流式响应
    // ====================

    /// <summary>
    /// Delta object for streaming / 流式响应的增量对象
    /// </summary>
    public class Delta
    {
        /// <summary>
        /// Content delta / 内容增量
        /// </summary>
        [JsonProperty("content")]
        public object Content { get; set; }

        /// <summary>
        /// Reasoning content delta / 推理内容增量
        /// </summary>
        [JsonProperty("reasoning_content")]
        public string ReasoningContent { get; set; }

        /// <summary>
        /// Role delta / 角色增量
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }
    }

    /// <summary>
    /// Chat completion chunk object / 聊天完成块对象
    /// Used for streaming responses / 用于流式响应
    /// </summary>
    public class ChatCompletionChunk
    {
        /// <summary>
        /// Chunk identifier / 块标识符
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Object type / 对象类型
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// Chunk creation timestamp / 块创建时间戳
        /// </summary>
        [JsonProperty("created")]
        public long Created { get; set; }

        /// <summary>
        /// Model used for the chunk / 用于块的模型
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// List of choices in the chunk / 块中的选择列表
        /// </summary>
        [JsonProperty("choices")]
        public List<Choice> Choices { get; set; }

        /// <summary>
        /// System fingerprint / 系统指纹
        /// </summary>
        [JsonProperty("system_fingerprint")]
        public string SystemFingerprint { get; set; }

        /// <summary>
        /// Token usage for the chunk / 块的令牌使用情况
        /// </summary>
        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }
}
