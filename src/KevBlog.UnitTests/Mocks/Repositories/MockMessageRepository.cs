using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests.Mocks.Repositories
{
    public class MockMessageRepository : Mock<IMessageRepository>
    {
        public MockMessageRepository MockGetMessages(List<Message> results)
        {
            Setup(x => x.GetMessages()).ReturnsAsync(results);
            return this;
        }
        public MockMessageRepository MockGetMessage(Message message)
        {
            Setup(x => x.GetMessage(It.IsAny<int>())).ReturnsAsync(message);
            return this;
        }
        public MockMessageRepository MockGetMessageThread(List<Message> results)
        {
            Setup(x => x.GetMessageThread(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(results);
            return this;
        }
    }
}
