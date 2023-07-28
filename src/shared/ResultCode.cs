namespace Debaters.API;

public enum ResultCode
{
	Success,
	UnknownError,
	InvalidName,
	InvalidCommunity,
	InvalidTitle,
	InvalidContent,
	InvalidPost,
	InvalidParent,
	InvalidCommentOrPost,
	AlreadyExists,
	DoesNotExist,
}
