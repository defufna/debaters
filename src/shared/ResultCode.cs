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
	AlreadyExists,
	DoesNotExist,
}
