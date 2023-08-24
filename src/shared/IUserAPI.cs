using VeloxDB.Client;
using VeloxDB.Protocol;

namespace Debaters.API;

[DbAPI(Name = "UserAPI")]
public interface IUserAPI
{
    [DbAPIOperation]
    DatabaseTask<string?> Login(string? username, string? password);

    [DbAPIOperation]
    DatabaseTask<RegisterResult> Register(string? username, string? password, string? email);

    [DbAPIOperation]
    DatabaseTask<ResultCode> LogOut(string sid);
}
