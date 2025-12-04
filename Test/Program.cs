using Newtonsoft.Json;
using OpenAISharp;

namespace Test
{
    /// <summary>
    /// Program class / 程序类
    /// Main entry point for the application / 应用程序的主入口点
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method / 主方法
        /// Application entry point / 应用程序入口点
        /// </summary>
        /// <param name="args">Command line arguments / 命令行参数</param>
        static void Main(string[] args)
        {
            // Initialize configuration / 初始化配置
            Configuration configuration = new Configuration();
            // Set API endpoint URL / 设置API端点URL
            configuration.ApiUrl = "https://api.siliconflow.cn/v1/chat/completions";
            // Set model name / 设置模型名称
            configuration.Model = "THUDM/GLM-Z1-9B-0414";
            // Set API key / 设置API密钥
            configuration.ApiKey = "your apikey";
            // Create OpenAI instance / 创建OpenAI实例
            OpenAI api = new OpenAI(configuration);

            // Initialize streaming request / 初始化流式请求
            var request_stream = new ChatCompletionRequest
            {
                // Add conversation messages / 添加对话消息
                Messages = new List<MessageRequest>
                {
                    // System message to set AI's role and behavior / 系统消息，用于设定AI的角色和行为
                    new MessageRequest { Role = "system", Content = "你是个ai助手" },
                },
                // Enable streaming response / 启用流式响应
                Stream = true
            };

            // Main conversation loop / 主对话循环
            while (true)
            {
                // Prompt for user input / 提示用户输入
                Console.Write("User:");
                // Read user message / 读取用户消息
                string usermessage = Console.ReadLine();
                // Add user message to request / 将用户消息添加到请求中
                request_stream.Messages.Add(new MessageRequest { Role = "user", Content = usermessage });
                // Get streaming response reader / 获取流式响应读取器
                var reader = api.completions_stream(request_stream);
                // Initialize assistant message / 初始化助手消息
                string assistantmessage = "";
                
                // Process streaming response / 处理流式响应
                while (!reader.EndOfStream)
                {
                    // Read and parse stream data / 读取并解析流数据
                    var linejson = OpenAI.readStreamRender(reader);
                    if (linejson != null)
                    {
                        // Handle content message / 处理内容消息
                        if (linejson.Choices[0].Delta.Content != null)
                        {
                            // Set text color to white / 设置文本颜色为白色
                            Console.ForegroundColor = ConsoleColor.White;
                            // Write content / 写入内容
                            Console.Write(linejson.Choices[0].Delta.Content);
                            // Accumulate assistant message / 累积助手消息
                            assistantmessage += linejson.Choices[0].Delta.Content;
                        }
                        // Handle reasoning content / 处理推理内容
                        else
                        {
                            // Set text color to red / 设置文本颜色为红色
                            Console.ForegroundColor = ConsoleColor.Red;
                            // Write reasoning content / 写入推理内容
                            Console.Write(linejson.Choices[0].Delta.ReasoningContent);
                        }
                    }
                }
                // Reset text color to white / 重置文本颜色为白色
                Console.ForegroundColor = ConsoleColor.White;
                // Add assistant response to message history / 将助手响应添加到消息历史
                request_stream.Messages.Add(new MessageRequest { Role = "assistant", Content = assistantmessage });
                // Write new line / 写入新行
                Console.WriteLine();
            }
        }
    }
}
