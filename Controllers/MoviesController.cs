using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Movies_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        IRepository<Movie> repository;
        IMapper _mapper;
        private new List<string> _allowedexstension=new List<string>() { ".jpg",".png"};
        private long max = 1048576;
        public MoviesController(IRepository<Movie> repository ,IMapper mapper)
        {
            this.repository = repository;
            _mapper=mapper;

        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin_Role)]
        public IActionResult create([FromForm]CreateMovieDto dto)
        {
            if (!_allowedexstension.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("Only jpg or png images");
            }
            if (dto.Poster.Length > max)
            {
                return BadRequest("Only 1 megabyte");
            }
            if (ModelState.IsValid)
            {
                using var datastream = new MemoryStream();
                dto.Poster.CopyTo(datastream);

                var data = _mapper.Map<Movie>(dto);

                data.Poster = datastream.ToArray();

               bool insearted= repository.inseart(data);
                if (insearted)
                {
                return Ok(data);
                }
            }
            return BadRequest("Genere Id Not found");
        }
        [HttpGet]
        public IActionResult getall()
        {
            var movie = repository.Getall();
            var data = _mapper.Map<List<MovieinfoDto>>(movie);
            return Ok(data);
        }
        [HttpGet("{id:int}")]
        public IActionResult getbyid([FromRoute]int id)
        {
            var mov = repository.GetById(id);
            if (mov!=null)
            {
                var data = _mapper.Map<MovieinfoDto>(mov);
                return Ok(data);
            }
            return BadRequest("ID Not Found");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin_Role)]
        public IActionResult update([FromRoute] int id, [FromForm] Updatemoviedto dto)
        {
            
            if (ModelState.IsValid)
            {
                var data= _mapper.Map<Movie>(dto);
                if (dto.Poster != null)
                {

                    if (!_allowedexstension.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    {
                        return BadRequest("Only jpg or png images");
                    }
                    if (dto.Poster.Length > max)
                    {
                        return BadRequest("Only 1 megabyte");
                    }
                     using var datastream = new MemoryStream();
                     dto.Poster.CopyTo(datastream);
                    data.Poster = datastream.ToArray();
                    
                
                }

                bool updated=repository.update(id,data);
                if (!updated)
                {
                    return BadRequest("ID Not Found");
                }
            }
                return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin_Role)]
        public IActionResult delete([FromRoute] int id)
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
