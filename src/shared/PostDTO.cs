using VeloxDB.ObjectInterface;

namespace Debaters.API;

public class PostDTO
{
	public long Id { get; set; }
	public VoteStatus Upvoted { get; set; }

	public int Upvotes { get; set; }
	public int Downvotes { get; set; }

	public string? Title { get; set; }

	[AutomapperIgnore]
	public string? Community { get; set; }
	[AutomapperIgnore]
	public string? Author { get; set; }
	public string? Content { get; set; }
}
