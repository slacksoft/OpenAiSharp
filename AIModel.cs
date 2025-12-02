using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAISharp
{
    public class AIModel
    {
        private static bool showStartLogo = true;
        private Setting setting = new Setting();
        public AIModel()
        {
            if (showStartLogo)
            {
                Console.WriteLine("Powered by");
                Console.WriteLine("  ____                   ___    ____ ____ __                 \r\n / __ \\ ___  ___  ___   / _ |  /  _// __// /  ___ _ ____ ___ \r\n/ /_/ // _ \\/ -_)/ _ \\ / __ | _/ / _\\ \\ / _ \\/ _ `// __// _ \\\r\n\\____// .__/\\__//_//_//_/ |_|/___//___//_//_/\\_,_//_/  / .__/\r\n     /_/                                              /_/    ");
            }

            string exeDir = AppContext.BaseDirectory;
            string ssettingPath = exeDir + @"\setting.json";
            if (!File.Exists(ssettingPath)) {
                File.WriteAllText(ssettingPath, JsonConvert.SerializeObject(setting));
                
                Console.WriteLine("The start up for the first time.");
                Console.WriteLine("You need to set up a settings file.");
                Console.WriteLine("./setting.json");
                Console.ReadLine();
                Process.GetCurrentProcess().Kill();
            }
            setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(ssettingPath))!;
            Console.WriteLine($"Source:{setting.ApiUrl}\r\nModel:{setting.model}");
        }

        /// <summary>
        /// 同步发送聊天完成请求
        /// </summary>
        /// <param name="apiKey">你的API密钥</param>
        /// <param name="request">请求模型对象</param>
        /// <returns>响应模型对象</returns>
        public ChatCompletionResponse Request(ChatCompletionRequest request)
        {
            if (request.Model == "") {
                request.Model = setting.model;
            }
            // 对于频繁调用，应考虑将 HttpClient 实例化为静态成员以复用
            using (var client = new HttpClient())
            {
                // 1. 设置请求头
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", setting.apiKey);

                // 2. 将请求对象序列化为 JSON 字符串
                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // 3. 发送 POST 请求并强制同步等待结果
                // 注意：.GetAwaiter().GetResult() 会阻塞当前线程直到任务完成。
                // 这满足了同步要求，但有死锁风险！
                HttpResponseMessage response = client.PostAsync(setting.ApiUrl, content).GetAwaiter().GetResult();

                // 4. 确保请求成功
                if (response.IsSuccessStatusCode)
                {

                    // 5. 读取响应内容并强制同步等待
                    var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    // 6. 使用 Newtonsoft.Json 将 JSON 响应反序列化为 C# 对象
                    var responseObject = JsonConvert.DeserializeObject<ChatCompletionResponse>(jsonResponse);

                    return responseObject ?? throw new InvalidOperationException("Failed to deserialize response.");
                }
                else {
                    Console.WriteLine(JsonConvert.SerializeObject(response));
                    return new ChatCompletionResponse { Id = response.StatusCode.ToString()};
                }

            }

        }

        public ModelListResponse getModels() {
      
            // 将字符串解析为 Uri 对象
            Uri originalUri = new Uri(setting.ApiUrl);
            // 使用 UriBuilder 来修改 Uri
            var uriBuilder = new UriBuilder(originalUri)
            {
                // 只修改 Path 属性
                Path = "/v1/models"
            };
            // 从 UriBuilder 获取新的、完整的URL字符串
            string modelsUrl = uriBuilder.ToString();
            using (var client = new HttpClient())
            {
                // 1. 设置请求头
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", setting.apiKey);
                // 3. 发送 POST 请求并强制同步等待结果
                // 注意：.GetAwaiter().GetResult() 会阻塞当前线程直到任务完成。
                // 这满足了同步要求，但有死锁风险！
                HttpResponseMessage response = client.GetAsync(modelsUrl).GetAwaiter().GetResult();
                var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var responseObject = JsonConvert.DeserializeObject<ModelListResponse>(jsonResponse);
                return responseObject ?? throw new InvalidOperationException("Failed to deserialize response.");
            }
            
        }

    }
}
