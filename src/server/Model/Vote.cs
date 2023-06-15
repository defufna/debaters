namespace Debaters.Server.Model;

using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
[HashIndex("voted", true, nameof(Vote.User), nameof(Vote.Node))]
public abstract class Vote : DatabaseObject
{
	[DatabaseProperty]
	public abstract bool Upvote { get; set; }

	[DatabaseReference(false, DeleteTargetAction.CascadeDelete, false)]
	public abstract User User { get; set; }

	[DatabaseReference(false, DeleteTargetAction.CascadeDelete, false)]
	public abstract Node Node { get; set; }
}

