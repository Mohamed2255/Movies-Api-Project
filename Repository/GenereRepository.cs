

using Microsoft.AspNetCore.Authorization;

namespace Movies_Api.Repository
{
    public class GenereRepository : IRepository<Genre>
    {
        ApplicationDbContext dbContext;

        public GenereRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool delete(int id)
        {
            try
            {
                Genre genre = GetById(id);
                dbContext.Genres.Remove(genre);
                dbContext.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        
        public List<Genre> Getall()
        {         
            return dbContext.Genres.OrderBy(c=>c.Name).ToList();
        }

        public Genre GetById(int id)
        {
            return dbContext.Genres.FirstOrDefault(e=>e.Id==id);
        }
      
        public bool inseart(Genre item)
        {
            try
            {
            dbContext.Genres.Add(item);
            dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public bool update(int id, Genre item)
        {
            try
            {
                var oldgenre =GetById(id);
                oldgenre.Name=item.Name;
                dbContext.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
          
        }
    }
}
