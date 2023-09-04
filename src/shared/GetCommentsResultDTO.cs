using System.Diagnostics;

namespace Debaters.API;

public class GetCommentsResultDTO : LoggedInResultDTO
{
	public List<CommentDTO>? Comments { get; set; }

	public PostDTO? Post { get; set; }

	public GetCommentsResultDTO()
	{
	}

	public GetCommentsResultDTO(ResultCode code, UserDTO? user, List<CommentDTO>? comments, PostDTO? post)
	{
		Code = code;
		User = user;
		Comments = comments;
		Post = post;
	}

	public static implicit operator GetCommentsResultDTO(ResultCode code)
    {
		Debug.Assert(code != ResultCode.Success, "You must provide comments if the result is success");
		return new GetCommentsResultDTO(code, null, null, null);
    }
}
