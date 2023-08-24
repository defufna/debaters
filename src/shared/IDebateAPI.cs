using VeloxDB.Client;
using VeloxDB.Protocol;

namespace Debaters.API;

[DbAPI(Name = "DebateAPI")]
public interface IDebateAPI
{
    [DbAPIOperation]
    DatabaseTask<ResultCode> CreateCommunity(string sid, string communityName);

    [DbAPIOperation]
    DatabaseTask<ResultCode> DeleteCommunity(string sid, string communityName);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<List<PostDTO>> GetTopPosts(string? sid);

    [DbAPIOperation]
    DatabaseTask<SubmitPostResultDTO> SubmitPost(string sid, string communityName, string title, string content);

    [DbAPIOperation]
    DatabaseTask<ResultCode> DeletePost(string sid, long id);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<GetCommentsResultDTO> GetComments(string? sid, long postId);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<GetCommentsResultDTO> GetCommentSubtree(string? sid, long commentId, int maxDepth = -1);

    [DbAPIOperation]
    DatabaseTask<SubmitCommentResultDTO> SubmitComment(string sid, long parentId, string content);

    [DbAPIOperation]
    DatabaseTask<ResultCode> UpdateComment(string sid, long id, string content);

    [DbAPIOperation]
    DatabaseTask<ResultCode> DeleteComment(string sid, long id);

    [DbAPIOperation]
    DatabaseTask<ResultCode> Vote(string sid, long nodeId, bool upvote);
}
