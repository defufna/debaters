using VeloxDB.Client;
using VeloxDB.Protocol;

namespace Debaters.API;

[DbAPI(Name = "DebateAPI")]
public interface IDebateAPI
{
    [DbAPIOperation]
    DatabaseTask<ResultCode> CreateCommunity(string username, string communityName);

    [DbAPIOperation]
    DatabaseTask<ResultCode> DeleteCommunity(string username, string communityName);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<List<PostDTO>> GetTopPosts();

    [DbAPIOperation]
    DatabaseTask<SubmitPostResultDTO> SubmitPost(string username, string communityName, string title, string content);

    [DbAPIOperation]
    DatabaseTask<ResultCode> DeletePost(string username, long id);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<GetCommentsResultDTO> GetComments(string? username, long postId);

    [DbAPIOperation(OperationType = DbAPIOperationType.Read)]
    DatabaseTask<GetCommentsResultDTO> GetCommentSubtree(string? username, long commentId, int maxDepth = -1);

    [DbAPIOperation]
    DatabaseTask<SubmitCommentResultDTO> SubmitComment(string username, long parentId, string content);

    [DbAPIOperation]
    DatabaseTask UpdateComment(string username, long id, string content);

    [DbAPIOperation]
    DatabaseTask<ResultCode> DeleteComment(string username, long id);

    [DbAPIOperation]
    DatabaseTask<ResultCode> Vote(string username, long nodeId, bool upvote);
}
