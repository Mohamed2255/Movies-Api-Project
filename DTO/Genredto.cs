namespace Movies_Api.DTO
{
    public class Genredto
    {
        [MaxLength(100, ErrorMessage = "Genere Name Must be less than 100 ")]
        public string Name { get; set; }
    }
}
