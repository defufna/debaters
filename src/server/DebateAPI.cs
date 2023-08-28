using Debaters.Server.Model;
using Debaters.API;
using VeloxDB.ObjectInterface;
using VeloxDB.Protocol;
using Debaters.Server.Utils;
using System.Diagnostics;

namespace Debaters.Server;
[DbAPI(Name = "DebateAPI")]
public class DebateAPI
{
	private const int minTitleLength = 10;
	private const int minContentLength = 10;
	private const int maxTopComments = 10;
	private const int maxComments = 256;
	private const int maxDepth = 3;

	[DbAPIOperation]
	public ResultCode CreateCommunity(ObjectModel om, string sid, string communityName)
	{
		if(string.IsNullOrEmpty(sid) || !om.TryGetSession(sid, out var session))
		{
			return ResultCode.InvalidSession;
		}

		if(string.IsNullOrEmpty(communityName) || !communityName.IsAlphanumeric())
		{
			return ResultCode.InvalidName;
		}

		if(om.CommunityExists(communityName.ToLower()))
		{
			return ResultCode.AlreadyExists;
		}

		Community community = om.CreateObject<Community>();
		community.Name = communityName;
		community.NameLower = communityName.ToLower();
		return ResultCode.Success;
	}

	[DbAPIOperation]
	public ResultCode DeleteCommunity(ObjectModel om, string sid, string communityName)
	{
		if(string.IsNullOrEmpty(sid) || !om.TryGetSession(sid, out var session))
		{
			return ResultCode.InvalidSession;
		}

		if(string.IsNullOrEmpty(communityName) || !om.TryGetCommunity(communityName, out var community))
		{
			return ResultCode.InvalidName;
		}

		Debug.Assert(community != null);
		community.Delete();
		return ResultCode.Success;
	}

	[DbAPIOperation(OperationType = DbAPIOperationType.Read)]
	public GetPostsResultDTO GetTopPosts(ObjectModel om, string? sid, string? communityName = null)
	{
		bool loggedIn = sid != null && om.TryGetSession(sid, out var session);
		Community? community = null;

		if(!string.IsNullOrEmpty(communityName) && !om.TryGetCommunity(communityName, out community))
		{
			return ResultCode.InvalidCommunity;
		}

		LimitedHeap<PostDTO> topPosts = new LimitedHeap<PostDTO>(1000, (first, second) => (first.Upvotes - first.Downvotes) - (second.Upvotes - second.Downvotes));

		IEnumerable<Post> posts = (community == null) ? om.GetAllObjects<Post>() : community.Posts;

		foreach(Post post in posts)
		{
			int score = post.Upvotes - post.Downvotes;
			if(!topPosts.TryGetTop(out var top) || score >= (top.Upvotes - top.Downvotes))
			{
				topPosts.Add(post.ToDTO(includeContent:false));
			}

			post.Abandon();
		}

		return new GetPostsResultDTO(ResultCode.Success, new List<PostDTO>(topPosts), communityName);
	}

	[DbAPIOperation]
	public SubmitPostResultDTO SubmitPost(ObjectModel om, string sid, string communityName, string title, string content)
	{
		if(string.IsNullOrEmpty(sid) || !om.TryGetSession(sid, out var session))
		{
			return ResultCode.InvalidSession;
		}

		if(string.IsNullOrEmpty(communityName) || !om.TryGetCommunity(communityName, out var community))
		{
			return ResultCode.InvalidCommunity;
		}

		if(string.IsNullOrEmpty(title) || title.Length < minTitleLength)
		{
			return ResultCode.InvalidTitle;
		}

		if(string.IsNullOrEmpty(content) || content.Length < minContentLength)
		{
			return ResultCode.InvalidContent;
		}

		Debug.Assert(community != null && session != null);

		Post post = om.CreateObject<Post>();
		post.Community = community;
		post.Title = title;
		post.Content = content;
        post.Author = session.User;
        return new SubmitPostResultDTO(ResultCode.Success, post.Id);
	}

	[DbAPIOperation]
	public ResultCode DeletePost(ObjectModel om, string sid, long id)
	{
		if(string.IsNullOrEmpty(sid) || !om.TryGetSession(sid, out var session))
		{
			return ResultCode.InvalidSession;
		}

		Post? post = om.GetObject<Post>(id);
		if(post == null)
		{
			return ResultCode.DoesNotExist;
		}

		post.Delete();
		return ResultCode.Success;

	}

	private IEnumerable<Comment> DepthFirst(IEnumerable<Comment> comments, int maxDepth)
	{
		foreach(var comment in comments)
		{
			yield return comment;
			if(maxDepth != 0)
			{
				foreach(var inner in DepthFirst(comment.Comments, maxDepth - 1))
				{
					yield return inner;
				}
			}
		}
	}

	[DbAPIOperation(OperationType = DbAPIOperationType.Read)]
	public GetCommentsResultDTO GetComments(ObjectModel om, string? sid, long postId)
	{
		bool loggedIn = sid != null;

		if(loggedIn && !om.TryGetSession(sid!, out var session))
		{
			loggedIn = false;
			sid = null;
		}

		Post? post = om.GetObject<Post>(postId);

		if(post == null)
		{
			return ResultCode.InvalidPost;
		}

		LimitedHeap<Comment> topComments = new LimitedHeap<Comment>(maxComments, CompareTop);

		foreach(Comment comment in DepthFirst(post.Comments, maxDepth))
		{
			topComments.Add(comment);
		}

		HashSet<long> selected = new HashSet<long>(topComments.Count + 1);
		foreach(Comment comment in topComments)
		{
			selected.Add(comment.Id);
		}
		selected.Add(postId);

		List<CommentDTO> result = new List<CommentDTO>(topComments.Count);

		foreach(Comment comment in topComments)
		{
			AddWithParents(om, comment, selected, result, loggedIn);
		}

		return new GetCommentsResultDTO(ResultCode.Success, result, post.ToDTO());
	}

	private void AddWithParents(ObjectModel om, Comment comment, HashSet<long> selected, List<CommentDTO> result, bool loggedIn)
	{
		Comment current = comment;

		while(true)
		{
			CommentDTO dto = current.ToDTO();
			dto.Author = current.Author.Username;

			if(loggedIn)
			{
				dto.MyVote = om.GetVoteStatus(current.Author.Id, current.Id);
			}

			result.Add(dto);

			if(selected.Contains(current.Parent.Id))
			{
				break;
			}
			else
			{
				Debug.Assert(current.Parent is Comment, "Comment's post is already in selected; only comments should arrive in this branch.");

				selected.Add(current.Parent.Id);
				current = (Comment)current.Parent;
			}
		}
	}

	[DbAPIOperation(OperationType = DbAPIOperationType.Read)]
	public GetCommentsResultDTO GetCommentSubtree(ObjectModel om, string? sid, long commentId, int maxDepth = -1)
	{
		throw new NotImplementedException();
	}

	private int CompareTop(Comment first, Comment second)
	{
		return (first.Upvotes + first.Downvotes) - (second.Upvotes + second.Downvotes);
	}

	[DbAPIOperation]
	public SubmitCommentResultDTO SubmitComment(ObjectModel om, string sid, long parentId, string content)
	{
		if(string.IsNullOrEmpty(sid) || !om.TryGetSession(sid, out var session))
		{
			return ResultCode.InvalidSession;
		}

		Node? parent = om.GetObject<Node>(parentId);

		if(parent == null)
		{
			return ResultCode.InvalidParent;
		}

		if(string.IsNullOrEmpty(content) || content.Length < minContentLength)
		{
			return ResultCode.InvalidContent;
		}

		Debug.Assert(session != null);

		Comment comment = om.CreateObject<Comment>();

		comment.Author = session.User;
		comment.Content = content;
		comment.Posted = DateTime.UtcNow;
        comment.Parent = parent;

        return new SubmitCommentResultDTO(ResultCode.Success, comment.Id);
	}

	[DbAPIOperation]
	public ResultCode UpdateComment(ObjectModel om, string username, long id, string content)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public ResultCode DeleteComment(ObjectModel om, string username, long id)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public ResultCode Vote(ObjectModel om, string sid, long nodeId, bool upvote)
	{
		if(string.IsNullOrEmpty(sid) || !om.TryGetSession(sid, out var session))
		{
			return ResultCode.InvalidSession;
		}

		Node? node = om.GetObject<Node>(nodeId);

		if(node == null)
		{
			return ResultCode.InvalidCommentOrPost;
		}

		Debug.Assert(session != null);

		Vote? vote = om.GetVote(session.User.Id, nodeId);

		if(vote == null)
		{
			vote = om.CreateObject<Vote>();
			vote.User = session.User;
			vote.Node = node;
		}

		vote.Upvote = upvote;
		return ResultCode.Success;
	}
}
