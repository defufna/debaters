using Debaters.API;
using Microsoft.AspNetCore.Mvc;
using VeloxDB.AspNet;

namespace Debaters.WebAPI;
[Route("/api/User/[action]")]
[ApiController]
public class UserAPIController : ControllerBase
{
	private readonly TimeSpan sessionExpire = TimeSpan.FromDays(14);
	private readonly IUserAPI api;

	public UserAPIController(IVeloxDBConnectionProvider vlxCP)
	{
		api = vlxCP.Get<IUserAPI>();
	}

	[HttpPost]
	public async Task<bool> Login([FromBody]LoginCredentials credentials)
	{
		string? sid = await api.Login(credentials.Username, credentials.Password);
		if(sid != null)
		{
			Response.Cookies.Append("sid", sid, new CookieOptions() 
			{ 
				HttpOnly = true,
				Expires = DateTime.UtcNow.Add(sessionExpire),
				SameSite = SameSiteMode.Strict
			});
		}
		return sid != null;
	}

	[HttpPost]
	public async Task<API.RegisterResult> Register([FromBody] RegisterData registerData)
	{
		return await api.Register(registerData.Username, registerData.Password, registerData.EMail);
	}

	[HttpPost]
	public async Task<API.ResultCode> LogOut()
	{
		string? sid = Request.Cookies["sid"];
		if (string.IsNullOrEmpty(sid))
			return API.ResultCode.UnknownError;
		return await api.LogOut(sid);
	}

}

public class LoginCredentials
{
	public string? Username { get; set; }
	public string? Password { get; set; }
}

public class RegisterData
{
	public string? Username { get; set; }
	public string? Password { get; set; }
	public string? EMail { get; set; }
}