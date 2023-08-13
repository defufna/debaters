namespace Debaters.Server.Model;

using Debaters.API;
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

	[InverseReferences(nameof(Comment.Parent))]
	public abstract InverseReferenceSet<Comment> Comments { get;  }

	public PostDTO ToDTO(VoteStatus upVoted = VoteStatus.NoVote, bool includeContent = true)
	{
		PostDTO result = new PostDTO()
		{
			Author = Author.Username,
			Community = Community.Name,
			Downvotes = Downvotes,
			Upvotes = Upvotes,
			Upvoted = upVoted,
			Id = Id,
			Title = Title,
		};

		if (includeContent)
			result.Content = Content;

		return result;
	}
}

