﻿
namespace KevBlog.Application.DTOs
{
    public class PostUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string LinkForPost { get; set; }
    }
}
