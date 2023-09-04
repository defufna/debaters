using Debaters.API;

namespace Debaters.WebAPI;

static class Utils
{
    public static void SetStatus(this HttpResponse response , OperationResultDTO result)
    {
    	if(result.Code != ResultCode.Success)
		{
			response.StatusCode = 400;
		}
    }
}
