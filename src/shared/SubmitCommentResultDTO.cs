using System.Diagnostics;

namespace Debaters.API;

public class SubmitCommentResultDTO
{
	public ResultCode Code { get; set; }
	public long Id { get; set; }

	public SubmitCommentResultDTO()
	{

	}

	public SubmitCommentResultDTO(ResultCode code, long id)
	{
		Code = code;
		Id = id;
	}

	public static implicit operator SubmitCommentResultDTO(ResultCode code)
	{
		Debug.Assert(code != ResultCode.Success);
		return new SubmitCommentResultDTO(code, 0);
	}
}
