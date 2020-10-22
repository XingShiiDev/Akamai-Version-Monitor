using Leaf.xNet;
using System;
using System.Threading;
namespace Akamai_Version_Checker
{
    class Program
    {
        const string currentVersion = "1.65";
        const string discordWebhook = "";
        const string targetEndpoint = "https://xero.com/";

        private static bool isUpdated = false;
        static void Main(string[] args)
        {
            versionCheck();
        }
        public static void versionCheck()
        {
            while (!isUpdated)
            {
                using (HttpRequest request = new HttpRequest())
                {
                    string readSource = request.Get(targetEndpoint).ToString();
                    string akamaiResource = Parse(readSource, "_cf.push(['_setAu', '/", "'])");

                    string readResource = request.Get(targetEndpoint + akamaiResource).ToString();
                    string akamaiVersion = Parse(readResource, "{ver:", ",");

                    if (akamaiVersion != currentVersion)
                    {
                        request.Post(discordWebhook, $"{{\"embeds\": [{{\"title\": \"Akamai Update Detected!\", \"color\": 14177041, \"description\": \"Akamai Update on {targetEndpoint} to version {akamaiVersion}\"}}]}}", "application/json");
                        isUpdated = true;
                    }
                    else
                        Console.WriteLine($"Still on {currentVersion}");
                }
                //Thread.Sleep(10000); /* Optional Sleep */
            }
        }

        public static string Parse(string source, string left, string right)
        {
            return source.Split(new string[1] { left }, StringSplitOptions.None)[1].Split(new string[1]
            {
                right
            }, StringSplitOptions.None)[0];
        }
    }
}
