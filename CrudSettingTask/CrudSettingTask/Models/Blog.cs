namespace CrudSettingTask.Models
{
    public class Blog : BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }   
        public string Description { get; set; } 
        public DateTime CreatedTime  { get; set; }
    }
}
