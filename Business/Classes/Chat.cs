using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Business.Classes
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Chat : Hub, IBotSupervisor
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly List<Message> _messages = new List<Message>();

        public Chat(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

    

        public async Task Send(MessageViewModel message)
        {
            message.messageTime = DateTime.Now;
            await Clients.All.SendAsync("Send", message);
            var stocks = GetStocksFromMessage(message.message);
            foreach (var stockCode in stocks)
            {
                var quote = await GetStockQuote(stockCode);
                if (quote.Contains("N/D"))
                    quote = "";
                var botMessage = new MessageViewModel { sender = "bot", message = "", messageTime = DateTime.Now };
                botMessage.message = quote != "" ? $"{stockCode} quote is ${quote}" : $"{stockCode} quote not found";
                await Send(botMessage);
            }

        }


        public ICollection<string> GetStocksFromMessage(string message)
        {
            var result = new List<string>();
            const string regex = @"/stock=[A-Za-z0-9.]*";
            var matches = Regex.Matches(message, regex, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (message == string.Empty || !match.Success) continue;
                var stockCode = match.Value.Replace(@"/stock=", "");
                result.Add(stockCode);
            }

            return result;

        }


        public async Task<string> GetStockQuote(string stockCode)
        {
            try
            {
                var serviceUrl = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";
                var request = new HttpRequestMessage(HttpMethod.Get,
                    serviceUrl);
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsByteArrayAsync();
                    using (var memStream = new MemoryStream(data))
                    {
                        var dataReader = new StreamReader(memStream,
                            System.Text.Encoding.UTF8,
                            true);
                        var currentLine = 1;
                        while (dataReader.Peek() >= 0)
                        {
                            var line = dataReader.ReadLine();
                            if (currentLine == 2)
                            {
                                if (line == null) continue;
                                var stockData = line.Split(',');
                                return stockData.Length < 7 ? null : stockData[6];
                            }
                            currentLine++;
                        }

                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

}

public class MessageViewModel
{
    public string sender { get; set; }
    public string message { get; set; }
    public DateTime? messageTime { get; set; }
}
}