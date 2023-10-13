using Debaters.API;
using Microsoft.AspNetCore.Mvc;
using VeloxDB.AspNet;

namespace Debaters.WebAPI;
[Route("/api/Debate/[action]")]
[ApiController]
public class DebateAPIController : ControllerBase
{
	private readonly IDebateAPI api;

	public DebateAPIController(IVeloxDBConnectionProvider vlxCP)
	{
		api = vlxCP.Get<IDebateAPI>();
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.OperationResultDTO> CreateCommunity(string communityName)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}
		var result = await api.CreateCommunity(sid, communityName);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.OperationResultDTO> DeleteCommunity(string communityName)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}
		var result = await api.DeleteCommunity(sid, communityName);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpGet]
	public async Task<GetPostsResultDTO> GetTopPosts(string? communityName = null)
	{
		string? sid = Request.Cookies["sid"];
		var result = await api.GetTopPosts(sid, communityName);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.SubmitPostResultDTO> SubmitPost(string communityName, string title, string content)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}

		var result = await api.SubmitPost(sid, communityName, title, content);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.OperationResultDTO> DeletePost(long id)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}

		var result = await api.DeletePost(sid, id);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpGet]
	public async Task<API.GetCommentsResultDTO> GetComments(long postId)
	{
		string? sid = Request.Cookies["sid"];
		var result = await api.GetComments(sid, postId);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpGet]
	public async Task<API.GetCommentsResultDTO> GetCommentSubtree(long commentId, int maxDepth)
	{
		string? sid = Request.Cookies["sid"];
		var result = await api.GetCommentSubtree(sid, commentId, maxDepth);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.SubmitCommentResultDTO> SubmitComment(long parentId, string content)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}

		var result = await api.SubmitComment(sid, parentId, content);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.OperationResultDTO> UpdateComment(long id, string content)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}

		var result = await api.UpdateComment(sid, id, content);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.OperationResultDTO> DeleteComment(long id)
	{
		string? sid = Request.Cookies["sid"];

		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}

		var result = await api.DeleteComment(sid, id);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.OperationResultDTO> Vote(long nodeId, bool upvote)
	{
		string? sid = Request.Cookies["sid"];

		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}

		var result = await api.Vote(sid, nodeId, upvote);
		Response.SetStatus(result);
		return result;
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.OperationResultDTO> RemoveVote(long nodeId)
	{
		string? sid = Request.Cookies["sid"];

		if (string.IsNullOrEmpty(sid))
		{
			Response.SetStatus(ResultCode.InvalidSession);
			return ResultCode.InvalidSession;
		}

		var result = await api.RemoveVote(sid, nodeId);
		Response.SetStatus(result);
		return result;
	}
}
