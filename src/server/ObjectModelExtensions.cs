
using System.Diagnostics.CodeAnalysis;
using Debaters.Server.Model;
using Debaters.API;
using VeloxDB.ObjectInterface;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace Debaters.Server;

public static class ObjectModelExtensions
{
	public static bool UserExists(this ObjectModel om, string username)
	{
		return TryGetUser(om, username, out _);
	}

	public static bool TryGetUser(this ObjectModel om, string username, [MaybeNullWhen(false)] out User? user)
	{
		HashIndexReader<User, string> index = User.GetUsernameHashIndex(om);
		user = index.GetObject(username.ToLower());
		return user != null;
	}

	public static bool TryGetSession(this ObjectModel om, string sessionId, [MaybeNullWhen(false)] out Session? session)
	{
		HashIndexReader<Session, long, long> index = Session.GetSidIndex(om);
        session = null;
        Span<byte> decoded = stackalloc byte[16];

		if(!Convert.TryFromBase64String(sessionId, decoded, out var bytesWritten) || bytesWritten != 16)
			return false;

        long sidHigh = MemoryMarshal.Read<long>(decoded);
        long sidLow = MemoryMarshal.Read<long>(decoded.Slice(8));

        session = index.GetObject(sidHigh, sidLow);
		return session != null;
	}

	public static bool CommunityExists(this ObjectModel om, string name)
	{
		return TryGetCommunity(om, name, out _);
	}

	public static bool TryGetCommunity(this ObjectModel om, string name, [MaybeNullWhen(false)] out Community? community)
	{
		HashIndexReader<Community, string> index = Community.GetCommunityNameIndex(om);
		community = index.GetObject(name.ToLower());
		return community != null;
	}

	public static VoteStatus GetVoteStatus(this ObjectModel om, long userId, long nodeId)
	{
		HashIndexReader<Vote, long, long> index = Vote.GetUsernameHashIndex(om);
		Vote? vote = index.GetObject(userId, nodeId);
		if (vote == null)
			return VoteStatus.NoVote;
		else
			return vote.Upvote ? VoteStatus.Upvoted : VoteStatus.Downvoted;
	}

	public static Vote? GetVote(this ObjectModel om, long userId, long nodeId)
	{
		HashIndexReader<Vote, long, long> index = Vote.GetUsernameHashIndex(om);
		return index.GetObject(userId, nodeId);
	}




}
