namespace Debaters.API;

public enum ResultCode
{
	Success,
	UnknownError,
	InvalidSession,
	InvalidName,
	InvalidCommunity,
	InvalidTitle,
	InvalidContent,
	InvalidPost,
	InvalidParent,
	InvalidCommentOrPost,
    InvalidPassword,
    InvalidEmail,
    InvalidUsername,
	AlreadyExists,
	DoesNotExist
}


public class OperationResultDTO
{
	public ResultCode Code { get; set; }

	public static implicit operator OperationResultDTO(ResultCode code)
    {
		return new OperationResultDTO() { Code = code };
    }
}

public class UserDTO
{
	public string Username { get; set; }

	public UserDTO()
	{
		Username = string.Empty;
	}

	public UserDTO(string username)
	{
		this.Username = username;
	}
}

public class LoggedInResultDTO : OperationResultDTO
{
	public UserDTO? User { get; set; }
}