namespace DbOperationsWithEFCoreApp.Data
{
    public class BookPrice
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int CurrencyId { get; set; }
        public int Amount { get; set; }

        public virtual Book Book { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
