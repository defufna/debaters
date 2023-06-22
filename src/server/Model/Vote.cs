namespace Debaters.Server.Model;

using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
[HashIndex(VotedHashIndex, true, nameof(Vote.User), nameof(Vote.Node))]
public abstract class Vote : DatabaseObject
{
	private const string VotedHashIndex = "voted";
	[DatabaseProperty]
	public abstract bool Upvote { get; set; }

	[DatabaseReference(false, DeleteTargetAction.CascadeDelete, false)]
	public abstract User User { get; set; }

	[DatabaseReference(false, DeleteTargetAction.CascadeDelete, false)]
	public abstract Node Node { get; set; }

	public static HashIndexReader<Vote, long, long> GetUsernameHashIndex(ObjectModel om)
	{
		return om.GetHashIndex<Vote, long, long>(VotedHashIndex);
	}
}

