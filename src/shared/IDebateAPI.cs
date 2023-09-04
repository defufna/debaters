using VeloxDB.Client;
using VeloxDB.Protocol;

namespace Debaters.API;

[DbAPI(Name = "DebateAPI")]
public interface IDebateAPI
{
    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> CreateCommunity(string sid, string communityName);

    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> DeleteCommunity(string sid, string communityName);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<GetPostsResultDTO> GetTopPosts(string? sid, string? communityName = null);

    [DbAPIOperation]
    DatabaseTask<SubmitPostResultDTO> SubmitPost(string sid, string communityName, string title, string content);

    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> DeletePost(string sid, long id);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<GetCommentsResultDTO> GetComments(string? sid, long postId);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<GetCommentsResultDTO> GetCommentSubtree(string? sid, long commentId, int maxDepth = -1);

    [DbAPIOperation]
    DatabaseTask<SubmitCommentResultDTO> SubmitComment(string sid, long parentId, string content);

    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> UpdateComment(string sid, long id, string content);

    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> DeleteComment(string sid, long id);

    [DbAPIOperation]
    DatabaseTask<OperationResultDTO> Vote(string sid, long nodeId, bool upvote);
}
