﻿using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;

namespace Bookify.Domain.Users;

public sealed class User : Entity
{
	private readonly List<Role> roles = new();

	private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
	{
		FirstName = firstName;
		LastName = lastName;
		Email = email;
	}

	private User()
	{
	}

	public FirstName FirstName { get; private set; }
	public LastName LastName { get; private set; }
	public Email Email { get; private set; }
	public string IdentityId { get; private set; } = String.Empty;

	public IReadOnlyCollection<Role> Roles => roles.ToList();

	public static User Create(FirstName firstName, LastName lastName, Email email)
	{
		var user = new User(Guid.NewGuid(), firstName, lastName, email);

		user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

		user.roles.Add(Role.Registered);

		return user;
	}

	public void SetIdentityId(string identityId)
	{

		IdentityId = identityId;
	}
}
