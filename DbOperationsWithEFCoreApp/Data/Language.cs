namespace DbOperationsWithEFCoreApp.Data
{
    public class Language
    {
        public int Id { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; }


        public ICollection<Book> Books { get; set; }
    }
}
