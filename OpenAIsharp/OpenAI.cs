using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAISharp
{
    /// <summary>
    /// OpenAI class for handling interactions with AI models / OpenAI类，用于处理与AI模型的交互
    /// </summary>
    public class OpenAI
    {
        // Configuration instance / 配置实例
        public Configuration configuration = new Configuration();

        /// <summary>
        /// Constructor for the OpenAI class / OpenAI类的构造函数
        /// </summary>
        /// <param name="_configuration">Configuration settings / 配置设置</param>
        /// <param name="showInfo">Whether to show startup info / 是否显示启动信息</param>
        public OpenAI(Configuration _configuration, bool showInfo = true)
        {
            if (showInfo)
            {
                // Display startup banner / 显示启动横幅
                Console.WriteLine("Powered by");
                Console.WriteLine("  ____                   ___    ____ ____ __                 \r\n / __ \\ ___  ___  ___   / _ |  /  _// __// /  ___ _ ____ ___ \r\n/ /_/ // _ \\/ -_)/ _ \\ / __ | _/ / _\\ \\ / _ \\/ _ `// __// _ \\\r\n\\____// .__/\\__//_//_//_/ |_|/___//___//_//_/\\_,_//_/  / .__/\r\n     /_/                                              /_/    ");
                // Display configuration info / 显示配置信息
                Console.WriteLine($"Source:{configuration.ApiUrl}\r\nModel:{configuration.Model}");
            }
            configuration = _configuration;
        }

        /// <summary>
        /// Synchronously sends a chat completion request / 同步发送聊天完成请求
        /// </summary>
        /// <param name="request">Request model object / 请求模型对象</param>
        /// <returns>Response model object / 响应模型对象</returns>
        public ChatCompletionResponse completions(ChatCompletionRequest request)
        {
            // Check if model is specified in request / 检查请求中是否指定了模型
            if (request.Model == "")
            {
                // Set default model if not specified / 如果未指定则设置默认模型
                request.Model = configuration.Model;
            }

            // Create HTTP client / 创建HTTP客户端
            using (var client = new HttpClient())
            {
                // Set authorization header / 设置授权头
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", configuration.ApiKey);

                // Serialize request to JSON / 将请求序列化为JSON
                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send POST request / 发送POST请求
                HttpResponseMessage response = client.PostAsync(configuration.ApiUrl, content).GetAwaiter().GetResult();

                // Handle successful response / 处理成功响应
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Read and deserialize response / 读取并反序列化响应
                        var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var responseObject = JsonConvert.DeserializeObject<ChatCompletionResponse>(jsonResponse);
                        return responseObject;
                    }
                    catch (Exception e)
                    {
                        // Return error message in response format / 以响应格式返回错误消息
                        return new ChatCompletionResponse
                        {
                            Choices = new List<Choice>() {
                                new Choice() {
                                    Message = new ResponseMessage() {
                                        Role = "System",
                                        Content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
                                    }
                                }
                            }
                        };
                    }
                }
                else
                {
                    // Handle failed response / 处理失败响应
                    return new ChatCompletionResponse
                    {
                        Choices = new List<Choice>() {
                            new Choice() {
                                Message = new ResponseMessage() {
                                    Role = "System",
                                    Content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
                                }
                            }
                        }
                    };
                }
            }
        }

        /// <summary>
        /// Creates a streaming chat completion request / 创建流式聊天完成请求
        /// </summary>
        /// <param name="request">Request model object / 请求模型对象</param>
        /// <returns>StreamReader for response / 响应的StreamReader</returns>
        public StreamReader completions_stream(ChatCompletionRequest request)
        {
            // Check if model is specified in request / 检查请求中是否指定了模型
            if (request.Model == "")
            {
                // Set default model if not specified / 如果未指定则设置默认模型
                request.Model = configuration.Model;
            }

            // Create HTTP client / 创建HTTP客户端
            using (var client = new HttpClient())
            {
                // Set authorization header / 设置授权头
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", configuration.ApiKey);

                // Serialize request to JSON / 将请求序列化为JSON
                var jsonRequest = JsonConvert.SerializeObject(request);

                // Create HTTP request message / 创建HTTP请求消息
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, configuration.ApiUrl);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", configuration.ApiKey);
                requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send request with response headers read option / 使用响应头读取选项发送请求
                var response = client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();

                // Handle successful response / 处理成功响应
                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStreamAsync().Result;
                    return new StreamReader(stream);
                }
                else
                {
                    // Convert error response to stream / 将错误响应转换为流
                    Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content.ReadAsStringAsync().GetAwaiter().GetResult()));
                    return new StreamReader(stream);
                }
            }
        }

        /// <summary>
        /// Reads and parses a single chunk from the stream / 从流中读取并解析单个数据块
        /// </summary>
        /// <param name="streamReader">StreamReader object / StreamReader对象</param>
        /// <returns>ChatCompletionChunk if successful, null otherwise / 成功返回ChatCompletionChunk，否则返回null</returns>
        public static ChatCompletionChunk? readStreamRender(StreamReader streamReader)
        {
            // Read a line from the stream / 从流中读取一行
            var line = streamReader.ReadLineAsync().Result;
            try
            {
                // Process valid lines / 处理有效行
                if (line.IndexOf("[DONE]") == -1 && line != "")
                {
                    // Parse JSON after removing "data:" prefix / 移除"data:"前缀后解析JSON
                    var linejson = JsonConvert.DeserializeObject<ChatCompletionChunk>(line.Replace("data:", ""));
                    return linejson;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions / 处理异常
                return new ChatCompletionChunk { Choices = new List<Choice> { new Choice { Delta = new Delta { Content = line } } } };
            }
            return null;
        }

        /// <summary>
        /// Gets the list of available AI models / 获取可用的AI模型列表
        /// </summary>
        /// <returns>Model list response object / 模型列表响应对象</returns>
        public ModelListResponse getModels()
        {
            // Parse API URL / 解析API URL
            Uri originalUri = new Uri(configuration.ApiUrl);

            // Build models endpoint URL / 构建模型端点URL
            var uriBuilder = new UriBuilder(originalUri)
            {
                Path = "/v1/models"
            };
            string modelsUrl = uriBuilder.ToString();

            try
            {
                // Create HTTP client / 创建HTTP客户端
                var client = new HttpClient();
                // Set authorization header / 设置授权头
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", configuration.ApiKey);

                // Send GET request / 发送GET请求
                HttpResponseMessage response = client.GetAsync(modelsUrl).GetAwaiter().GetResult();

                // Read and deserialize response / 读取并反序列化响应
                var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var responseObject = JsonConvert.DeserializeObject<ModelListResponse>(jsonResponse);

                // Return response or throw exception / 返回响应或抛出异常
                return responseObject!;

            }
            catch (Exception ex)
            {
                return new ModelListResponse { Data = new List<ModelInfo> { new ModelInfo { Id = "Invalid request" } } };
            }
        }
    }
}
