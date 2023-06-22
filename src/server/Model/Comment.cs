namespace Debaters.Server.Model;

using System;
using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;

[DatabaseClass]
public abstract partial class Comment : Node
{
	[DatabaseReference(true, DeleteTargetAction.SetToNull)]
	public abstract User Author { get; set; }

	[DatabaseProperty]
	public abstract string Content { get; set; }

	[DatabaseReference(true, DeleteTargetAction.PreventDelete)]
	public abstract Node Parent { get; set; }

	[InverseReferences(nameof(Comment.Parent))]
	public abstract InverseReferenceSet<Comment> Comments { get;  }

	public partial CommentDTO ToDTO();
}

