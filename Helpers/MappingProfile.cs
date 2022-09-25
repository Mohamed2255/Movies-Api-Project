using AutoMapper;

namespace Movies_Api.Helpers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieinfoDto>();
            CreateMap<CreateMovieDto,Movie>()
                .ForMember(src=>src.Poster,opt=>opt.Ignore());
            CreateMap<Updatemoviedto, Movie>()
         .ForMember(src => src.Poster, opt => opt.Ignore());
            CreateMap<Register,ApplicationUser>();
        }
    }
}
