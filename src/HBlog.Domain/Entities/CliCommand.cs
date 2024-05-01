using System.ComponentModel.DataAnnotations.Schema;

namespace HBlog.Domain.Entities
{
    [Table("CliCommands")]
    public class CliCommand 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Command { get; set; }
        public string AppName { get; set; }
        public string Type { get; set; }
        public string Usage { get; set; }
        public int ApplicationId { get; set; }
        public Application Application {get; set; }
    }
}