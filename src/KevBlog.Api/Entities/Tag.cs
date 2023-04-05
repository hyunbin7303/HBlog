using System.ComponentModel.DataAnnotations.Schema;
namespace API.Entities
{
    [Table("Tags")]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}