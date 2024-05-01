using Markdig;

namespace HBlog.WebClient.Services
{
    public class MarkdownService
    {
        public MarkdownPipeline _pipeline { get; set; }

        public MarkdownService()
        {
            var builder = new MarkdownPipelineBuilder();
            _pipeline = builder.UseBootstrap().UseAdvancedExtensions().Build();
        }
        public string RenderMarkdown(string markdownContent)
        {
            return Markdown.ToHtml(markdownContent, _pipeline);
        }
    }
}
