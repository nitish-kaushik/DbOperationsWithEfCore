namespace DbOperationsWithEFCoreApp.Data
{
    public class Currency
    {
        public int Id { get; set; }
        public int Title { get; set; }
        public int Description { get; set; }

        public ICollection<BookPrice> BookPrices { get; set; }
    }
}
