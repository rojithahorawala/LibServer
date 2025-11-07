namespace LibServer.DTO
{
    public class Inventory
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required int PublicationYear { get; set; }

        public required string Author { get; set; }

        public required int SumOfBooks { get; set; }

    }
}
 