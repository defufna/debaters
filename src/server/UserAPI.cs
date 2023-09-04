
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Debaters.API;
using Debaters.Server.Model;
using Debaters.Server.Utils;
using VeloxDB.ObjectInterface;
using VeloxDB.Protocol;
namespace Debaters.Server;

[DbAPI(Name = "UserAPI")]
public class UserAPI
{
    [DbAPIOperation]
    public OperationResultDTO Register(ObjectModel om, string username, string password, string email)
    {
        if (string.IsNullOrEmpty(password) || !PasswordUtils.CheckStrength(password))
            return ResultCode.InvalidPassword;
        if (string.IsNullOrEmpty(email) || !IsValidEMail(email))
            return ResultCode.InvalidEmail;
        if (string.IsNullOrEmpty(username) || !IsValidUsername(username))
            return ResultCode.InvalidUsername;

        if (om.UserExists(username))
        {
            return ResultCode.AlreadyExists;
        }

        byte[] hash = PasswordUtils.HashPassword(password);

        User user = om.CreateObject<User>();

        user.Username = username;
        user.UsernameLower = username.ToLower();

        user.EMail = email;
        user.Password = DatabaseArray<byte>.Create(hash);

        return ResultCode.Success;
    }

    [DbAPIOperation]
    public string? Login(ObjectModel om, string username, string password)
    {
        if (username == null || !om.TryGetUser(username, out var user))
            return null;

        if (password == null)
            return null;

        Debug.Assert(user != null);

        byte[] hashedPass = new byte[user.Password.Count];
        user.Password.CopyTo(hashedPass, 0);

        if (!PasswordUtils.CheckPassword(password, hashedPass))
            return null;

        Session session = om.CreateObject<Session>();
        Span<byte> sessionId = new byte[16];
        RandomNumberGenerator.Fill(sessionId);
        session.SidHigh = MemoryMarshal.Read<long>(sessionId);
		session.SidLow = MemoryMarshal.Read<long>(sessionId.Slice(8));

        session.User = user;

        return Convert.ToBase64String(sessionId);
    }

    [DbAPIOperation]
    public OperationResultDTO LogOut(ObjectModel om, string sid)
    {
        if(sid == null || !om.TryGetSession(sid, out var session))
            return ResultCode.InvalidSession;

        session.Delete();
        return ResultCode.Success;
    }
    private static bool IsValidUsername(string username) => username.IsAlphanumeric();
    private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    private static bool IsValidEMail(string email) => EmailRegex.IsMatch(email);

}
