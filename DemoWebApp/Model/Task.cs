using System.ComponentModel.DataAnnotations;
namespace DemoWebApp.Model
{
    public class Task1
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Task_title { get; set; }
        public string Task_description { get; set; }
        public string Attachment { get; set; }
        [Required]
        public int Project_id { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime End_Date { get; set; }



    }
}
