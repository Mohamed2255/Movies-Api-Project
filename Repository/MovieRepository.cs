using Microsoft.EntityFrameworkCore;

namespace Movies_Api.Repository
{
    public class MovieRepository : IRepository<Movie>
    {
        ApplicationDbContext dbContext;
        public MovieRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool delete(int id)
        {
            try
            {
                Movie movie = GetById(id);
                dbContext.Movies.Remove(movie);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Movie> Getall()
        {
            return dbContext.Movies.Include(c=>c.genere).ToList();
        }

        public Movie GetById(int id)
        {

            return dbContext.Movies.Include(c => c.genere).FirstOrDefault(c=>c.Id==id);

        }

        public bool inseart(Movie item)
        {
            try
            {
                var genere_id = dbContext.Genres.Any(c=>c.Id==item.genere_id);
                if (!genere_id)
                {
                    return false;
                }
            dbContext.Movies.Add(item);
            dbContext.SaveChanges();
            return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool update(int id, Movie item)
        {
            try
            {
                var old_movie = GetById(id);
                old_movie.Title = item.Title;
                old_movie.Rate = item.Rate;
                old_movie.Storyline = item.Storyline;
                if (item.Poster!=null)
                {
                old_movie.Poster = item.Poster;
                }
                old_movie.Year = item.Year;
                var genere_id = dbContext.Genres.Any(c => c.Id == item.genere_id);
                if (!genere_id)
                {
                    return false;
                }
                old_movie.genere_id = item.genere_id;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
