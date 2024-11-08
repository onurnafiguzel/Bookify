using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
	private const string PasswordCredentialType = "password";
	private readonly HttpClient httpClient;

	public AuthenticationService(HttpClient httpClient)
	{
		this.httpClient = httpClient;
	}

	public async Task<string> RegisterAsync(
		User user,
		string password,
		CancellationToken cancellationToken = default)
	{
		var userRepresentationModel = UserRepresentationModel.FromUser(user);

		userRepresentationModel.Credentials = new CredentialRepresentationModel[]
		{
			new()
			{
				Value = password,
				Temporary = false,
				Type = PasswordCredentialType
			}
		};

		var response = await httpClient.PostAsJsonAsync(
			"users",
			userRepresentationModel,
			cancellationToken);

		return ExtractIdentityFromLocationHeader(response);
	}

	private static string ExtractIdentityFromLocationHeader(
		HttpResponseMessage httpResponseMessage)
	{
		const string usersSegmentName = "users/";

		var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

		if (locationHeader is null)
		{
			throw new InvalidOperationException("Location header can't be null");
		}

		var usersSegmentValueIndex = locationHeader.IndexOf(
			usersSegmentName,
			StringComparison.InvariantCultureIgnoreCase);

		var usersIdentityId = locationHeader.Substring(
			usersSegmentValueIndex + usersSegmentName.Length);

		return usersIdentityId;
	}
}
