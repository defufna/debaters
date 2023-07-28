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
	public ResultCode CreateCommunity(ObjectModel om, string username, string communityName)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return ResultCode.UnknownError;
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
	public ResultCode DeleteCommunity(ObjectModel om, string username, string communityName)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return ResultCode.UnknownError;
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
	public List<PostDTO> GetTopPosts(ObjectModel om)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public SubmitPostResultDTO SubmitPost(ObjectModel om, string username, string communityName, string title, string content)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return ResultCode.UnknownError;
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

		Debug.Assert(community != null);

		Post post = om.CreateObject<Post>();
		post.Community = community;
		post.Title = title;
		post.Content = content;
		return new SubmitPostResultDTO(ResultCode.Success, post.Id);
	}

	[DbAPIOperation]
	public ResultCode DeletePost(ObjectModel om, string username, long id)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return ResultCode.UnknownError;
		}

		Post? post = om.GetObject<Post>(id);
		if(post == null)
		{
			return ResultCode.DoesNotExist;
		}

		post.Delete();
		return ResultCode.Success;

	}

	[DbAPIOperation(OperationType = DbAPIOperationType.Read)]
	public GetCommentsResultDTO GetComments(ObjectModel om, string? username, long postId)
	{
		bool loggedIn = username != null;

		if(loggedIn && !om.UserExists(username!))
		{
			return ResultCode.UnknownError;
		}

		Post? post = om.GetObject<Post>(postId);

		if(post == null)
		{
			return ResultCode.InvalidPost;
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

		return new GetCommentsResultDTO(ResultCode.Success, result);
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
	public GetCommentsResultDTO GetCommentSubtree(ObjectModel om, string? username, long commentId, int maxDepth = -1)
	{
		throw new NotImplementedException();
	}

	private int CompareTop(Comment first, Comment second)
	{
		return (first.Upvotes + first.Downvotes) - (second.Upvotes + second.Downvotes);
	}

	[DbAPIOperation]
	public SubmitCommentResultDTO SubmitComment(ObjectModel om, string username, long parentId, string content)
	{
		if(string.IsNullOrEmpty(username) || !om.TryGetUser(username, out var user))
		{
			return ResultCode.UnknownError;
		}

		Comment? parent = om.GetObject<Comment>(parentId);

		if(parent == null)
		{
			return ResultCode.InvalidParent;
		}

		if(string.IsNullOrEmpty(content) || content.Length < minContentLength)
		{
			return ResultCode.InvalidContent;
		}

		Debug.Assert(user != null);

		Comment comment = om.CreateObject<Comment>();

		comment.Author = user;
		comment.Content = content;
		comment.Posted = DateTime.UtcNow;

		return new SubmitCommentResultDTO(ResultCode.Success, comment.Id);
	}

	[DbAPIOperation]
	public void UpdateComment(ObjectModel om, string username, long id, string content)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public ResultCode DeleteComment(ObjectModel om, string username, long id)
	{
		throw new NotImplementedException();
	}

	[DbAPIOperation]
	public ResultCode Vote(ObjectModel om, string username, long nodeId, bool upvote)
	{
		if(string.IsNullOrEmpty(username) || !om.TryGetUser(username, out var user))
		{
			return ResultCode.UnknownError;
		}

		Node? node = om.GetObject<Node>(nodeId);

		if(node == null)
		{
			return ResultCode.InvalidCommentOrPost;
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
		return ResultCode.Success;
	}
}
