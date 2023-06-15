namespace Debaters.Server.Model;

using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
public abstract class Post : Node
{
	[DatabaseProperty]
	public abstract string Title { get; set; }

	[DatabaseProperty]
	public abstract string Content { get; set;}

	[DatabaseReference(false, DeleteTargetAction.CascadeDelete)]
	public abstract Community Community { get; set; }
}

