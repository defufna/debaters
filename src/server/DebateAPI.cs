using Debaters.Server.Model;
using VeloxDB.ObjectInterface;
using VeloxDB.Protocol;
using Debaters.Server.Utils;
using System.Diagnostics;

namespace Debaters.Server;


public enum CreateCommunityResultCode
{
	UnknownError,
	InvalidName,
	AlreadyExists,
	Success
}

public enum DeleteCommunityResultCode
{
	UnknownError,
	InvalidName,
	Success,
}

public enum SubmitPostResultCode
{
	UnknownError,
	InvalidCommunity,
	InvalidTitle,
	InvalidContent,
	Success
}

public enum DeletePostResultCode
{
	UnknownError,
	DoesNotExist,
	Success
}

public enum GetCommentsResultCode
{
	UnknownError,
	InvalidPost,
	Success,
}

public enum SubmitCommentResultCode
{
	UnknownError,
	InvalidParent,
	InvalidContent,
	Success,
}

public enum VoteResultCode
{
	UnknownError,
	InvalidCommentOrPost,
	Success
}

public class DeleteCommentResultCode
{

}

public class SubmitCommentResult
{
	public SubmitCommentResultCode Code { get; set; }
	public long Id { get; set; }

	public SubmitCommentResult()
	{

	}

	public SubmitCommentResult(SubmitCommentResultCode code, long id)
	{
		Code = code;
		Id = id;
	}

	public static implicit operator SubmitCommentResult(SubmitCommentResultCode code)
	{
		Debug.Assert(code != SubmitCommentResultCode.Success);
		return new SubmitCommentResult(code, 0);
	}
}

public class GetCommentsResult
{
	public GetCommentsResultCode Code { get; set; }
	public List<CommentDTO>? Comments { get; set; }

	public GetCommentsResult()
	{
	}

	public GetCommentsResult(GetCommentsResultCode code, List<CommentDTO>? comments)
	{
		Code = code;
		Comments = comments;
	}

	public static implicit operator GetCommentsResult(GetCommentsResultCode code)
    {
		Debug.Assert(code != GetCommentsResultCode.Success, "You must provide comments if the result is success");
		return new GetCommentsResult(code, null);
    }
}

public class SubmitPostResult
{
	public SubmitPostResultCode Code { get; set; }
	public long Id { get; set; }

	public SubmitPostResult()
	{

	}

    public SubmitPostResult(SubmitPostResultCode code, long id)
    {
        Code = code;
		Id = id;
	}

	public static implicit operator SubmitPostResult(SubmitPostResultCode code)
    {
		Debug.Assert(code != SubmitPostResultCode.Success, "You must provide Id if the result is success");
		return new SubmitPostResult(code, 0);
    }
}

public enum VoteStatus : short
{
	Downvoted = -1,
	NoVote = 0,
	Upvoted = 1
}

public class PostDTO
{
	public long Id { get; set; }
	public VoteStatus Upvoted { get; set; }

	public long Upvotes { get; set; }
	public long Downvotes { get; set; }

	public string? Title { get; set; }
}

public class CommentDTO
{
	public long Id { get; set; }
	public long Parent { get; set; }

	[AutomapperIgnore]
	public string? AutorUsername { get; set; }

	[AutomapperIgnore]
	public VoteStatus MyVote { get; set; }

	public int Upvotes { get; set; }
	public int Downvotes { get; set; }

	public string? Content { get; set; }
}

[DbAPI(Name = "DebateAPI")]
public class DebateAPI
{
	private const int minTitleLength = 10;
	private const int minContentLength = 10;
	private const int maxTopComments = 10;
	private const int maxComments = 256;
	private const int maxDepth = 3;

	[DbAPIOperation]
	public CreateCommunityResultCode CreateCommunity(ObjectModel om, string username, string communityName)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return CreateCommunityResultCode.UnknownError;
		}

		if(string.IsNullOrEmpty(communityName) || !communityName.IsAlphanumeric())
		{
			return CreateCommunityResultCode.InvalidName;
		}

		if(om.CommunityExists(communityName.ToLower()))
		{
			return CreateCommunityResultCode.AlreadyExists;
		}

		Community community = om.CreateObject<Community>();
		community.Name = communityName;
		community.NameLower = communityName.ToLower();
		return CreateCommunityResultCode.Success;
	}

	[DbAPIOperation]
	public DeleteCommunityResultCode DeleteCommunity(ObjectModel om, string username, string communityName)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return DeleteCommunityResultCode.UnknownError;
		}

		if(string.IsNullOrEmpty(communityName) || !om.TryGetCommunity(communityName, out var community))
		{
			return DeleteCommunityResultCode.InvalidName;
		}

		Debug.Assert(community != null);
		community.Delete();
		return DeleteCommunityResultCode.Success;
	}

	[DbAPIOperation(OperationType = DbAPIOperationType.Read)]
	public List<PostDTO> GetTopPosts(ObjectModel om)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public SubmitPostResult SubmitPost(ObjectModel om, string username, string communityName, string title, string content)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return SubmitPostResultCode.UnknownError;
		}

		if(string.IsNullOrEmpty(communityName) || !om.TryGetCommunity(communityName, out var community))
		{
			return SubmitPostResultCode.InvalidCommunity;
		}

		if(string.IsNullOrEmpty(title) || title.Length < minTitleLength)
		{
			return SubmitPostResultCode.InvalidTitle;
		}

		if(string.IsNullOrEmpty(content) || content.Length < minContentLength)
		{
			return SubmitPostResultCode.InvalidContent;
		}

		Debug.Assert(community != null);

		Post post = om.CreateObject<Post>();
		post.Community = community;
		post.Title = title;
		post.Content = content;
		return new SubmitPostResult(SubmitPostResultCode.Success, post.Id);
	}

	[DbAPIOperation]
	public DeletePostResultCode DeletePost(ObjectModel om, string username, long id)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return DeletePostResultCode.UnknownError;
		}

		Post? post = om.GetObject<Post>(id);
		if(post == null)
		{
			return DeletePostResultCode.DoesNotExist;
		}

		post.Delete();
		return DeletePostResultCode.Success;

	}

	[DbAPIOperation(OperationType = DbAPIOperationType.Read)]
	public GetCommentsResult GetComments(ObjectModel om, string? username, long postId)
	{
		bool loggedIn = username != null;

		if(loggedIn && !om.UserExists(username!))
		{
			return GetCommentsResultCode.UnknownError;
		}

		Post? post = om.GetObject<Post>(postId);

		if(post == null)
		{
			return GetCommentsResultCode.InvalidPost;
		}

		LimitedHeap<Comment> topComments = new LimitedHeap<Comment>(maxComments, CompareTop);

		Queue<(IEnumerable<Comment> comments, int depth)> queue = new();

		queue.Enqueue((post.Comments, 0));

		while (queue.Count > 0)
		{
			(IEnumerable<Comment> comments, int depth) = queue.Dequeue();

			foreach(Comment comment in comments)
			{
				topComments.Add(comment);
				if(depth + 1 < maxDepth)
				{
					queue.Enqueue((comment.Comments, depth + 1));
				}
			}
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

		return new GetCommentsResult(GetCommentsResultCode.Success, result);
	}

	private void AddWithParents(ObjectModel om, Comment comment, HashSet<long> selected, List<CommentDTO> result, bool loggedIn)
	{
		Comment current = comment;

		while(true)
		{
			CommentDTO dto = current.ToDTO();
			dto.AutorUsername = current.Author.Username;

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
	public GetCommentsResult GetCommentSubtree(ObjectModel om, string? username, long commentId, int maxDepth = -1)
	{
		throw new NotImplementedException();
	}

	private int CompareTop(Comment first, Comment second)
	{
		return (first.Upvotes + first.Downvotes) - (second.Upvotes + second.Downvotes);
	}

	[DbAPIOperation]
	public SubmitCommentResult SubmitComment(ObjectModel om, string username, long parentId, string content)
	{
		if(string.IsNullOrEmpty(username) || !om.TryGetUser(username, out var user))
		{
			return SubmitCommentResultCode.UnknownError;
		}

		Comment? parent = om.GetObject<Comment>(parentId);

		if(parent == null)
		{
			return SubmitCommentResultCode.InvalidParent;
		}

		if(string.IsNullOrEmpty(content) || content.Length < minContentLength)
		{
			return SubmitCommentResultCode.InvalidContent;
		}

		Debug.Assert(user != null);

		Comment comment = om.CreateObject<Comment>();

		comment.Author = user;
		comment.Content = content;
		comment.Posted = DateTime.UtcNow;

		return new SubmitCommentResult(SubmitCommentResultCode.Success, comment.Id);
	}

	[DbAPIOperation]
	public void UpdateComment(ObjectModel om, string username, long id, string content)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public DeleteCommentResultCode DeleteComment(ObjectModel om, string username, long id)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public VoteResultCode Vote(ObjectModel om, string username, long nodeId, bool upvote)
	{
		if(string.IsNullOrEmpty(username) || !om.TryGetUser(username, out var user))
		{
			return VoteResultCode.UnknownError;
		}

		Node? node = om.GetObject<Node>(nodeId);

		if(node == null)
		{
			return VoteResultCode.InvalidCommentOrPost;
		}

		Debug.Assert(user != null);

		Vote? vote = om.GetVote(user.Id, nodeId);

		if(vote == null)
		{
			vote = om.CreateObject<Vote>();
			vote.User = user;
			vote.Node = node;
		}

		vote.Upvote = upvote;
		return VoteResultCode.Success;
	}
}
