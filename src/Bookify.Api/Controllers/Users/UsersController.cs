using Bookify.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{
	private readonly ISender sender;

	public UsersController(ISender sender)
	{
		this.sender = sender;
	}

	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<IActionResult> Register(
		RegisterUserRequest request,
		CancellationToken cancellationToken)
	{
		var command = new RegisterUserCommand(
			request.Email,
			request.FirstName,
			request.LastName,
			request.Password);

		var result = await sender.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		return Ok(result.Value);
	}
}
