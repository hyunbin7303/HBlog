
using HBlog.Domain.Entities;

namespace HBlog.TestUtilities
{
    public class PostBuilder
    {
        private string title = "Post Title";
        private string slug = "post-title";
        private string desc = "post description";
        private string status = "active";
        private string content = "#hahaha";
        private string type = "normal";
        private DateTime created = DateTime.Now;
        private DateTime updated = DateTime.Now;
        private int userId = 1;
        private int categoryId = 1;
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
        public PostBuilder WithCategory(int categoryId)
        {
            this.categoryId = categoryId;
            return this;
        }

    }
}
