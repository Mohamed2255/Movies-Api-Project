namespace Movies_Api.Repository
{
    public interface IAuth
    {
       Task <Authmodel> Register(Register item);
       Task<Authmodel> Login(Loginmodel item);
        Task<string> AddRole(Rolemodel item);

    }


}
