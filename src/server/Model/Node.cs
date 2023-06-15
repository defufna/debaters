namespace Debaters.Server.Model;

using VeloxDB.ObjectInterface;

[DatabaseClass]
public abstract class Node : DatabaseObject
{

	[DatabaseProperty]
	public abstract int Upvotes { get; set; }
	
	[DatabaseProperty]
	public abstract int Downvotes { get; set; }	

	[DatabaseProperty]
	public abstract DateTime Posted { get; set; }
}

