namespace Debaters.Server.Model;

using VeloxDB.ObjectInterface;

[DatabaseClass]
[HashIndex("username", true, nameof(User.Username))]
public abstract class User : DatabaseObject
{
	[DatabaseProperty]
	public abstract string Username { get; set; }

	[DatabaseProperty]
	public abstract DatabaseArray<byte> Password {get; set;}
}

