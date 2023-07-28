using System.Diagnostics;

namespace Debaters.API;

public class GetCommentsResultDTO
{
	public ResultCode Code { get; set; }
	public List<CommentDTO>? Comments { get; set; }

	public GetCommentsResultDTO()
	{
	}

	public GetCommentsResultDTO(ResultCode code, List<CommentDTO>? comments)
	{
		Code = code;
		Comments = comments;
	}

	public static implicit operator GetCommentsResultDTO(ResultCode code)
    {
		Debug.Assert(code != ResultCode.Success, "You must provide comments if the result is success");
		return new GetCommentsResultDTO(code, null);
    }
}
