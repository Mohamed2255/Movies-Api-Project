namespace Movies_Api.Model
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storyline { get; set; }
        public byte[] Poster { get; set; }
        [ForeignKey("genere")]
        public byte genere_id { get; set; }

        public Genre genere { get; set; }


    }
}
