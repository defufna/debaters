using Debaters.API;
using Microsoft.AspNetCore.Mvc;
using VeloxDB.AspNet;
using VeloxDB.Client;
using VeloxDB.Protocol;

namespace Debaters.WebAPI;

[Forward(typeof(RouteAttribute), "/api/User/[action]")]
[DbAPI(Name = "UserAPI")]
public interface IUserAPI
{
    [DbAPIOperation]
    DatabaseTask<string?> Login(string username, string password);

    [DbAPIOperation]
    DatabaseTask<RegisterResult> Register(string username, string password, string email);

    [DbAPIOperation]
    DatabaseTask<ResultCode> LogOut(string sid);
}
