namespace Movies_Api.Repository
{
    public interface IRepository<T>
    {
        List<T> Getall();
        T GetById(int id);
        bool inseart(T item);
        bool update(int id, T item);
        bool delete(int id);

    }
}
