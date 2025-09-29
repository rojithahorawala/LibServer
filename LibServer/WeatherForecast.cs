namespace LibServer
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int Author { get; set; }

        public int CoAuthor => 32 + (int)(Author / 0.5556);

        public string? Summary { get; set; }
    }
}
