namespace WEB_153503_Konchik.Extensions;

public static class HttpRequestExtension
{
    public static bool IsAjaxRequest(this HttpRequest httpRequest)
    {
        return httpRequest.Headers["x-requested-with"].ToString().ToLower().Equals("xmlhttprequest");
    }
}
