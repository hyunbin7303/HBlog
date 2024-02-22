using Markdig;

namespace KevBlog.WebClient.Services
{
    public class MarkdownService
    {
        public string RenderMarkdown(string markdownContent)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(markdownContent, pipeline);
        }
    }
}
