
using HBlog.Domain.Entities;

namespace HBlog.TestUtilities
{
    public class PostBuilder
    {
        private string title;
        private string slug;
        private string desc;
        private string status;
        private string content;
        private string type;
        private DateTime created;
        private DateTime updated;
        private int userId;
        private int categoryId;
        private List<PostTags> postTags;

        public PostBuilder WithTitle(string title)
        {
            this.title = title;
            return this;
        }
        public PostBuilder WithSlug(string slug)
        {
            this.slug = slug;
            return this;
        }

    }
}
