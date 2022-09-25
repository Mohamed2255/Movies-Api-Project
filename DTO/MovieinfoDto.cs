namespace Movies_Api.DTO
{
    public class MovieinfoDto
    {

        public int Id { get; set; }
       
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }

        public string Storyline { get; set; }
        public byte[] Poster { get; set; }

        public byte genere_id { get; set; }

       public string GenereName { get; set; }

    }
}
