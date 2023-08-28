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
	public async Task<API.ResultCode> CreateCommunity(string communityName)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;
		return await api.CreateCommunity(sid, communityName);
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.ResultCode> DeleteCommunity(string communityName)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;
		return await api.DeleteCommunity(sid, communityName);
	}

	[Microsoft.AspNetCore.Mvc.HttpGet]
	public async Task<GetPostsResultDTO> GetTopPosts(string? communityName = null)
	{
		string? sid = Request.Cookies["sid"];
		return await api.GetTopPosts(sid, communityName);
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.SubmitPostResultDTO> SubmitPost(string communityName, string title, string content)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;

		return await api.SubmitPost(sid, communityName, title, content);
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.ResultCode> DeletePost(long id)
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;

		return await api.DeletePost(sid, id);
	}

	[Microsoft.AspNetCore.Mvc.HttpGet]
	public async Task<API.GetCommentsResultDTO> GetComments(long postId)
	{
		string? sid = Request.Cookies["sid"];
		return await api.GetComments(sid, postId);
	}

	[Microsoft.AspNetCore.Mvc.HttpGet]
	public async Task<API.GetCommentsResultDTO> GetCommentSubtree(long commentId, int maxDepth)
	{
		string? sid = Request.Cookies["sid"];
		return await api.GetCommentSubtree(sid, commentId, maxDepth);
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.SubmitCommentResultDTO> SubmitComment(long parentId, string content)
	{
		string? sid = Request.Cookies["sid"];

		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;

		return await api.SubmitComment(sid, parentId, content);
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.ResultCode> UpdateComment(long id, string content)
	{
		string? sid = Request.Cookies["sid"];

		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;

		return await api.UpdateComment(sid, id, content);
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.ResultCode> DeleteComment(long id)
	{
		string? sid = Request.Cookies["sid"];

		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;

		return await api.DeleteComment(sid, id);
	}

	[Microsoft.AspNetCore.Mvc.HttpPost]
	public async Task<API.ResultCode> Vote(long nodeId, bool upvote)
	{
		string? sid = Request.Cookies["sid"];

		if (string.IsNullOrEmpty(sid))
			return ResultCode.InvalidSession;

		return await api.Vote(sid, nodeId, upvote);
	}

}
