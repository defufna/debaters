using System.Diagnostics;

namespace Debaters.API;

public class GetPostsResultDTO
{

    public ResultCode Code { get; set; }
	public List<PostDTO>? Posts { get; set; }
	public string? Community { get; set; }

    public GetPostsResultDTO()
    {

    }

    public GetPostsResultDTO(ResultCode code, List<PostDTO>? posts, string? community)
    {
        Code = code;
        Posts = posts;
        Community = community;
    }

	public static implicit operator GetPostsResultDTO(ResultCode code)
    {
		Debug.Assert(code != ResultCode.Success, "You must provide posts if the result is success");
		return new GetPostsResultDTO(code, null, null);
    }
}
