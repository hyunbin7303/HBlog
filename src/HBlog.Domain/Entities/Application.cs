using System.ComponentModel.DataAnnotations.Schema;

namespace HBlog.Domain.Entities
{
    [Table("Applications")]
    public class Application
    {
        public int ApplicationId { get; set; }
        public string AppName { get; set; }
        public string AppDesc { get; set; }
        public string AppUsage { get; set; }
        public string Type { get; set; }
        public List<CliCommand> CliCommands { get; set; } = new();
    }
}