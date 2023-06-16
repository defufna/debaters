
using System.Diagnostics.CodeAnalysis;
using Debaters.Server.Model;
using VeloxDB.ObjectInterface;
namespace Debaters.Server;

public static class ObjectModelExtensions
{
	public static bool UserExists(this ObjectModel om, string username)
	{
		return TryGetUser(om, username, out _);
	}

	public static bool TryGetUser(this ObjectModel om, string username, [MaybeNullWhen(false)] out User? user)
	{
		HashIndexReader<User, string> index = User.GetUsernameHashIndex(om);
		user = index.GetObject(username.ToLower());
		return user != null;
	}
}
