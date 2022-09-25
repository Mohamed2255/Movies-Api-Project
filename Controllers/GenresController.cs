using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Movies_Api.Controllers
{

    [Authorize(Roles = Roles.Admin_Role)]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {

        IRepository<Genre> repository;
        public GenresController(IRepository<Genre> repository)
        {
            this.repository = repository;
        }

        [HttpGet("getall")]

        public IActionResult GetallGeneres()
        {
            var geners = repository.Getall();
            List<Genreinfodto> info = new List<Genreinfodto>();
            foreach (var item in geners)
            {
                Genreinfodto genreinfodto = new Genreinfodto()
                {
                    Genere_Id = item.Id,
                    Genere_Name = item.Name
                };

                info.Add(genreinfodto);
            }
            return Ok(info);
        }

        [HttpGet]
        public IActionResult getbyid(int id)
        {
            var generes = repository.GetById(id);
            Genreinfodto genreinfodtos = new Genreinfodto()
            {
                Genere_Id = generes.Id,
                Genere_Name = generes.Name
            };

            return Ok(genreinfodtos);
        }
        [Authorize(Roles = Roles.Admin_Role)]
        [HttpPost]
        public IActionResult add(Genredto genredto)
        {
            if (ModelState.IsValid)
            {
                Genre genre = new Genre()
                {
                    Name = genredto.Name,
                };
                repository.inseart(genre);
                return Ok(genre);
            }

            return BadRequest();
        }
        [Authorize(Roles = Roles.Admin_Role)]
        [HttpPut("{id}")]

        public IActionResult update([FromRoute]int id, [FromBody] Genredto genredto)
        {
            Genre genre = new Genre()
            {
                Name = genredto.Name
            };
            bool updated = repository.update(id, genre);
            if (!updated)
            {
                return NotFound("ID Not Found");
            }
            return Ok("Updated");


        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin_Role)]
        public IActionResult delete([FromRoute]int id)
        {


            bool deleted = repository.delete(id);
            if (!deleted)
            {
                return NotFound("ID Not Found");
            }
            return Ok();



        }
    }
}
