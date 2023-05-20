using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests.Repositories
{
    internal class MockIMessageRepository
    {
        public static Mock<IMessageRepository> GetMock()
        {
            var mock = new Mock<IMessageRepository>();
            IEnumerable<Message> messages = GenerateData(5);
            mock.Setup(m => m.GetMessage(5).Result).Returns(() => messages);
            return mock;
        }
        public static IEnumerable<Message> GenerateData(int count)
        {
            var messages = new List<Message>();
            for(int i=0; i< count; i++)
            {
                Message msg = new()
                {
                    Id = i,
                    Content = "Message information #" + i,
                    DateRead = DateTime.Now,
                };

                messages.Add(msg);
            }
            return messages;
        }
    }
}
