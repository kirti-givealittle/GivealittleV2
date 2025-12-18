namespace GivealittleV2.Server.Auth.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string email, string password);
        bool Verify(string email, string hash, string password);
    }
}
