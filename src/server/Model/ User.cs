namespace Debaters.Server.Model;

using System;
using Debaters.API;
using VeloxDB.ObjectInterface;

[DatabaseClass]
[HashIndex(UsernameHashIndex, true, nameof(User.UsernameLower))]
public abstract class User : DatabaseObject
{
	private const string UsernameHashIndex = "username";

	[DatabaseProperty]
	public abstract string EMail { get; set; }

	[DatabaseProperty]
	public abstract string Username { get; set; }

	[DatabaseProperty]
	public abstract string UsernameLower { get; set; } // Needed for index until VeloxDB adds support for case insensitive indices

	[DatabaseProperty]
	public abstract DatabaseArray<byte> Password {get; set;}

	public static HashIndexReader<User, string> GetUsernameHashIndex(ObjectModel om)
	{
		return om.GetHashIndex<User, string>(UsernameHashIndex);
	}

    public UserDTO? ToDTO()
    {
		return new UserDTO(Username);
    }
}

