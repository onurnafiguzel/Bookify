using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;

internal abstract class Repository<T> where T : Entity
{
	protected readonly ApplicationDbContext dbContext;

	protected Repository(ApplicationDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<T?> GetByIdAsync(
		Guid id,
		CancellationToken cancellationToken = default)
	{
		return await dbContext
			.Set<T>()
			.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
	}

	public void Add(T entity)
	{
		dbContext.Add(entity);
	}
}
