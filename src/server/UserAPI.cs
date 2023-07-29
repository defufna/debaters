
using System.Diagnostics;
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
    public RegisterResult Register(ObjectModel om, string username, string password, string email)
    {
        if (string.IsNullOrEmpty(password) || !PasswordUtils.CheckStrength(password))
            return RegisterResult.InvalidPassword;
        if (string.IsNullOrEmpty(email) || !IsValidEMail(email))
            return RegisterResult.InvalidEmail;
        if (string.IsNullOrEmpty(username) || !IsValidUsername(username))
            return RegisterResult.InvalidUsername;

        HashIndexReader<User, string> index = User.GetUsernameHashIndex(om);

        User? checkUser = index.GetObject(username);

        if (om.UserExists(username))
        {
            return RegisterResult.AlreadyExists;
        }

        byte[] hash = PasswordUtils.HashPassword(password);

        User user = om.CreateObject<User>();

        user.Username = username;
        user.UsernameLower = username.ToLower();

        user.EMail = email;
        user.Password = DatabaseArray<byte>.Create(hash);

        return RegisterResult.Success;
    }

    [DbAPIOperation]
    public bool Login(ObjectModel om, string username, string password)
    {
        if (username == null || !om.TryGetUser(username, out var user))
            return false;

        if (password == null)
            return false;

        Debug.Assert(user != null);

        byte[] hashedPass = new byte[user.Password.Count];
        user.Password.CopyTo(hashedPass, 0);

        if (!PasswordUtils.CheckPassword(password, hashedPass))
            return false;

        return true;
    }

    private bool IsValidUsername(string username) => username.IsAlphanumeric();

    private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    private bool IsValidEMail(string email) => EmailRegex.IsMatch(email);

}
