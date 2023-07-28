namespace Debaters.Server.Model;

using System;
using VeloxDB.Descriptor;
using VeloxDB.ObjectInterface;
using Debaters.API;

[DatabaseClass]
public abstract partial class Comment : Node
{
	[DatabaseProperty]
	public abstract string Content { get; set; }

	[DatabaseReference(true, DeleteTargetAction.PreventDelete)]
	public abstract Node Parent { get; set; }

	[InverseReferences(nameof(Comment.Parent))]
	public abstract InverseReferenceSet<Comment> Comments { get;  }

	public partial CommentDTO ToDTO();
}

