namespace Debaters.Server.Model;

using Debaters.API;
using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
public abstract partial class Post : Node
{
	[DatabaseProperty]
	public abstract string Title { get; set; }

	[DatabaseProperty]
	public abstract string Content { get; set;}

	[DatabaseReference(false, DeleteTargetAction.CascadeDelete)]
	public abstract Community Community { get; set; }

	[InverseReferences(nameof(Comment.Parent))]
	public abstract InverseReferenceSet<Comment> Comments { get;  }

	public partial PostDTO ToDTO();
}

