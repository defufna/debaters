using VeloxDB.Client;
using VeloxDB.Protocol;

namespace Debaters.API;

[DbAPI(Name = "UserAPI")]
public interface IUserAPI
{
    [DbAPIOperation]
    DatabaseTask<string?> Login(string? username, string? password);

    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> Register(string? username, string? password, string? email);

    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> LogOut(string sid);
}
