using VeloxDB.Client;
using VeloxDB.Protocol;

namespace Debaters.API;

[DbAPI(Name = "UserAPI")]
public interface IUserAPI
{
    DatabaseTask<string?> Login(string username, string password);
    DatabaseTask<RegisterResult> Register(string username, string password, string email);
}
