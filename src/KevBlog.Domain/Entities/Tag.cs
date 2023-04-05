using System.ComponentModel.DataAnnotations.Schema;
namespace KevBlog.Domain.Entities
{
    [Table("Tags")]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}