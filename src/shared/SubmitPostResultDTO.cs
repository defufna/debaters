using System.Diagnostics;

namespace Debaters.API;

public class SubmitPostResultDTO
{
	public ResultCode Code { get; set; }
	public long Id { get; set; }

	public SubmitPostResultDTO()
	{

	}

    public SubmitPostResultDTO(ResultCode code, long id)
    {
        Code = code;
		Id = id;
	}

	public static implicit operator SubmitPostResultDTO(ResultCode code)
    {
		Debug.Assert(code != ResultCode.Success, "You must provide Id if the result is success");
		return new SubmitPostResultDTO(code, 0);
    }
}
