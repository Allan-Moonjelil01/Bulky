using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Category Name")]
        public String Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Please enter between 1 and 100")]
        public int DisplayOrder { get; set; }
    }
}
