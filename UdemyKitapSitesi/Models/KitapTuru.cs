using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UdemyKitapSitesi.Models
{
    public class KitapTuru
    {
        [Key] //primary key
        public int Id { get; set; }

        [Required(ErrorMessage ="Bu alan boş bırakılamaz!")] //do not be  null
        [MaxLength(25)]
        [DisplayName("Kitap Türü Adı")]
        public string? Name { get; set; }
    }
}
