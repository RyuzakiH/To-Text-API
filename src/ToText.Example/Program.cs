using System.Threading.Tasks;
using ToText.API;

namespace ToText.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Synchronous
            Test();

            // Asynchronous
            TestAsync().Wait();

        }


        private static void Test()
        {
            var client = new ToTextClient();

            var result0 = client.Convert(@"..\..\..\assets\sample0.png", Languages.English);

            var result1 = client.Convert(@"..\..\..\assets\sample1.png", Languages.English);

            var result2 = client.Convert(@"..\..\..\assets\sample2.png", Languages.English);

            var result3 = client.Convert(@"..\..\..\assets\sample3.jpg", Languages.English);
        }


        private async static Task TestAsync()
        {
            var client = new ToTextClient();

            var result0 = await client.ConvertAsync(@"..\..\..\assets\sample0.png", Languages.English);

            var result1 = await client.ConvertAsync(@"..\..\..\assets\sample1.png", Languages.English);

            var result2 = await client.ConvertAsync(@"..\..\..\assets\sample2.png", Languages.English);

            var result3 = await client.ConvertAsync(@"..\..\..\assets\sample3.jpg", Languages.English);
        }

    }
}
