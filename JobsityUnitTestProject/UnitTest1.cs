using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Business.Classes;
using Business.Interfaces;
using Moq;
using Xunit;

namespace JobsityUnitTestProject
{
    public class BotService_MessageHaveCommands
    {

     

        [Fact]
        public void Message_HaveCommands_ReturnTrue()
        {

            var mockFactory = new Mock<IHttpClientFactory>();

            var chat = new Chat(mockFactory);
            var message = "/stock=stock_code /stock=stock_code /stock=appl.us";
            ICollection<string> result=new List<string>();
            var botSupervisor = new Mock<Chat>();
            botSupervisor.Setup(x => x.GetStocksFromMessage(message)).Returns(result);
            

            Assert.True(result.Count>0, "Message have commands");
        }

        [Fact]
        public void Message_HaveMultipleCommands_ReturnTrue()
        {
            var message = "/stock=stock_code /stock=stock_code /stock=appl.us"

            var result = _botSupervisor.GetStocksFromMessage(message);

            Assert.True(result.Count > 1, "Message have multiple commands");
        }

        [Fact]
        public void Message_HaveOneCommand_ReturnTrue()
        {
            var message = "message start /stock=stock_code message complement";
            var result = _botSupervisor.GetStocksFromMessage(message);

            Assert.True(result.Count == 1, "Message have one commands");
        }


        [Fact]
        public void Message_HaveNotCommands_ReturnFalse()
        {
            var message = "This is a message without commands";
            var result = _botSupervisor.GetStocksFromMessage(message);

            Assert.False(result.Count > 0, "Message have no commands");
        }
    }
}
