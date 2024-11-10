﻿using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
	private readonly ApplicationDbContext dbContext;

	public AuthorizationService(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<UserRoleResponse> GetRolesForUserAysnc(string identityId)
	{
		var roles = await dbContext.Set<User>()
			.Where(user => user.IdentityId == identityId)
			.Select(user => new UserRoleResponse
			{
				Id = user.Id,
				Roles = user.Roles.ToList()
			})
			.FirstAsync();

		return roles;
	}

	public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
	{
		var permissions = await dbContext.Set<User>()
			.Where(user => user.IdentityId == identityId)
			.SelectMany(user => user.Roles.Select(role => role.Permissions))
			.FirstAsync();

		var permissionsSet = permissions.Select(p => p.Name).ToHashSet();

		return permissionsSet;
	}
}
