namespace GivealittleV2.API.Auth.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string email, string password);
        bool Verify(string email, string hash, string password);
    }
}
