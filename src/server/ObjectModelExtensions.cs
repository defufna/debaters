
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

	public static bool CommunityExists(this ObjectModel om, string name)
	{
		return TryGetCommunity(om, name, out _);
	}

	public static bool TryGetCommunity(this ObjectModel om, string name, [MaybeNullWhen(false)] out Community? community)
	{
		HashIndexReader<Community, string> index = Community.GetCommunityNameIndex(om);
		community = index.GetObject(name.ToLower());
		return community != null;
	}
}
