using VeloxDB.ObjectInterface;

namespace Debaters.API;

public class CommentDTO
{
	public long Id { get; set; }
	public long Parent { get; set; }

	[AutomapperIgnore]
	public string? Author { get; set; }

	[AutomapperIgnore]
	public VoteStatus MyVote { get; set; }

	public int Upvotes { get; set; }
	public int Downvotes { get; set; }

	public string? Content { get; set; }
}
