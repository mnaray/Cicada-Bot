using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;

namespace Cicada
{
    class Program
    {
        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += ClientOnMessageReceived;

            // enter your token here, or better still, read it from file
            string path = @"../../../TOKEN.txt";
            var token = File.ReadAllText(path);

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private static Task ClientOnMessageReceived(SocketMessage arg)
        {
            string prefix = "cic ";

            if (arg.Content.StartsWith(prefix + "helloworld"))
            {
                arg.Channel.SendMessageAsync($"User {arg.Author.Mention} successfully ran helloworld!");
            }

            if (arg.Content.StartsWith(prefix + "help"))
            {
                arg.Channel.SendMessageAsync("Use \"*cic commandlist*\" to see a list of commands.");
            }

            if (arg.Content.StartsWith(prefix + "commandlist"))
            {
                string path = @"../../../commandlist.txt";
                string text = File.ReadAllText(path);
                arg.Channel.SendMessageAsync(text);
            }

            if (arg.Content.StartsWith(prefix + "video"))
            {
                string url = "https://cdn.discordapp.com/attachments/818118715458584636/928444275002310736/video0-2-1_1.mov";
                arg.Channel.SendMessageAsync(url);
                arg.AddReactionAsync(new Emoji("😉"));
            }
            return Task.CompletedTask;
        }
    }
}
