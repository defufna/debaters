using System.Diagnostics;

namespace Debaters.API;

public class GetPostsResultDTO : LoggedInResultDTO
{
	public List<PostDTO>? Posts { get; set; }
	public string? Community { get; set; }

    public GetPostsResultDTO()
    {

    }

    public GetPostsResultDTO(ResultCode code, UserDTO? user, List<PostDTO>? posts, string? community)
    {
        Code = code;
        User = user;
        Posts = posts;
        Community = community;
    }

	public static implicit operator GetPostsResultDTO(ResultCode code)
    {
		Debug.Assert(code != ResultCode.Success, "You must provide posts if the result is success");
		return new GetPostsResultDTO(code, null, null, null);
    }
}
