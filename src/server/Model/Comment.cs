namespace Debaters.Server.Model;

using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
public abstract class Comment : Node
{
	[DatabaseReference(true, DeleteTargetAction.SetToNull)]
	public abstract User Author { get; set; }

	[DatabaseProperty]
	public abstract string Content { get; set; }

	[DatabaseReference(true, DeleteTargetAction.SetToNull)]
	public abstract Node Parent { get; set; }
}

