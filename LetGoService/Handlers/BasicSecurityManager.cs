using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace LetGoService.Handlers
{
	public class BasicSecurityManager : DelegatingHandler
    {
		private const string HttpBasicAuthScheme = "Basic";

		private static readonly Encoding HttpBasicEncoding = Encoding.UTF8;

		private static readonly char[] ColonSeparator = new char[] { ':' };

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
			// 2. check if the request has public access
			AuthenticationHeaderValue authorizationHeader = request.Headers.Authorization;
			if (authorizationHeader != null && HttpBasicAuthScheme.Equals(authorizationHeader.Scheme, StringComparison.Ordinal))
			{
				string base64 = authorizationHeader.Parameter;
				byte[] bits = Convert.FromBase64String(base64);
				string usernameColonPassword = HttpBasicEncoding.GetString(bits);
				string[] usernameAndPassword = usernameColonPassword.Split(ColonSeparator, StringSplitOptions.RemoveEmptyEntries);
				if (usernameAndPassword.Length == 2)
				{

					if ("iwhackathon".Equals(usernameAndPassword[0]) && "Letshack25".Equals(usernameAndPassword[1]))
					{
						try
						{
							return base.SendAsync(request, cancellationToken);
						}
						catch (Exception ex)
						{
							HttpResponseMessage response = request.CreateResponse(HttpStatusCode.InternalServerError, ex);
							var ddd = new TaskCompletionSource<HttpResponseMessage>();
							ddd.SetResult(response);
							return ddd.Task;
						}
					}
				}
			}

			HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Forbidden, "Service api request is denied.");
			var tsc = new TaskCompletionSource<HttpResponseMessage>();
			tsc.SetResult(reply);
			return tsc.Task;
        }
    }
}