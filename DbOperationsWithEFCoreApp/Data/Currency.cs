namespace DbOperationsWithEFCoreApp.Data
{
    public class Currency
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<BookPrice> BookPrices { get; set; }
    }
}
