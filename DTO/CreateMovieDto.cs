namespace Movies_Api.DTO
{
    public class CreateMovieDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storyline { get; set; }
        public IFormFile Poster { get; set; }

        public byte genere_id { get; set; }
    }
}
