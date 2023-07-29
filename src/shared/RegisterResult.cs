namespace Debaters.API;

public enum RegisterResult
{
	Success,
	AlreadyExists,
	InvalidPassword,
	InvalidUsername,
	InvalidEmail,
}
