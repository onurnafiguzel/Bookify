﻿using Bookify.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Bookify.Infrastructure.Authorization;

internal class CustomClaimsTransformation : IClaimsTransformation
{
	private readonly IServiceProvider serviceProvider;

	public CustomClaimsTransformation(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		if (principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
			principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
		{
			return principal;
		}

		using var scope = serviceProvider.CreateScope();

		var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

		var indentityId = principal.GetIdentityId();

		var userRoles = await authorizationService.GetRolesForUserAysnc(indentityId);

		var claimsIdentity = new ClaimsIdentity();

		claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.Id.ToString()));

		foreach (var role in userRoles.Roles)
		{
			claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
		}

		principal.AddIdentity(claimsIdentity);

		return principal;
	}
}
