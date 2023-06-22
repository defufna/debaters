namespace Debaters.Server.Model;

using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
public abstract class Node : DatabaseObject
{
	[DatabaseReference(true, DeleteTargetAction.SetToNull)]
	public abstract User Author { get; set; }

	[DatabaseProperty]
	public abstract int Upvotes { get; set; }
	
	[DatabaseProperty]
	public abstract int Downvotes { get; set; }	

	[DatabaseProperty]
	public abstract DateTime Posted { get; set; }
}

