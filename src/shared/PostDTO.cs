namespace Debaters.API;

public class PostDTO
{
	public long Id { get; set; }
	public VoteStatus Upvoted { get; set; }

	public long Upvotes { get; set; }
	public long Downvotes { get; set; }

	public string? Title { get; set; }
}
