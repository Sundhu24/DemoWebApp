using System.ComponentModel.DataAnnotations;
namespace DemoWebApp.Model
{
    public class Country
    {
            public int id { get; set; }
            [Required]
            [MaxLength(10)]
            public string cname { get; set; }
            public int? population { get; set; }
            public int? area { get; set; }
          
        
    }
}
