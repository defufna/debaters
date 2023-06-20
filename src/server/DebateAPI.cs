using Debaters.Server.Model;
using VeloxDB.ObjectInterface;
using VeloxDB.Protocol;
using Debaters.Server.Utils;
using System.Diagnostics;

namespace Debaters.Server;


public enum CreateCommunityResult
{
	UnknownError,
	InvalidName,
	AlreadyExists,
	Success
}

public enum DeleteCommunityResult
{
	UnknownError,
	InvalidName,
	Success
}

[DbAPI(Name = "DebateAPI")]
public class DebateAPI
{
	[DbAPIOperation]
	public CreateCommunityResult CreateCommunity(ObjectModel om, string username, string communityName)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return CreateCommunityResult.UnknownError;
		}

		if(string.IsNullOrEmpty(communityName) || !communityName.IsAlphanumeric())
		{
			return CreateCommunityResult.InvalidName;
		}

		if(om.CommunityExists(communityName.ToLower()))
		{
			return CreateCommunityResult.AlreadyExists;
		}

		Community community = om.CreateObject<Community>();
		return CreateCommunityResult.Success;
	}

	public DeleteCommunityResult DeleteCommunity(ObjectModel om, string username, string communityName)
	{
		if(string.IsNullOrEmpty(username) || !om.UserExists(username))
		{
			return DeleteCommunityResult.UnknownError;
		}

		if(string.IsNullOrEmpty(communityName) || !om.TryGetCommunity(communityName, out var community))
		{
			return DeleteCommunityResult.InvalidName;
		}

		Debug.Assert(community != null);
		community.Delete();
		return DeleteCommunityResult.Success;
	}

	public void SubmitPost(ObjectModel om, string username, string community, string title, string content)
	{

	}

	public void DeletePost(ObjectModel om, string username, long id)
	{

	}

	public void SubmitComment(ObjectModel om, string username, long parentId, string content)
	{

	}

	public void UpdateComment(ObjectModel om, string username, long id, string content)
	{

	}

	public void DeleteComment(ObjectModel om, string username, long id)
	{

	}
}
