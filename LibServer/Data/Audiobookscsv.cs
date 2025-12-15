namespace LibServer.Data
{
    public class Audiobookscsv
    {
        public int? id { get; set; }
        public int? bookid { get; set; }
        public required string title { get; set; }
        //public int? abavailable { get; set; }
        public required string author { get; set; }
        public required string language { get; set; }
        public required string narrator { get; set; }
        public required string publisher { get; set; }
        public required string publicationYear { get; set; }
         
    }
}
