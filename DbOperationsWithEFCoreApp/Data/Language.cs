namespace DbOperationsWithEFCoreApp.Data
{
    public class Language
    {
        public int Id { get; set; } 
        public int Title { get; set; } 
        public int Description { get; set; }


        public ICollection<Book> Books { get; set; }
    }
}
