namespace NhTests.Shared;

#region << Using >>

using CRUD.Extensions;
using NHibernate;

#endregion

public static class SessionFactoryExt
{
    public static async Task AddEntitiesAsync<TEntity>(this ISessionFactory sessionFactory, IEnumerable<TEntity> enumerable)
    {
        var entities = enumerable.ToArrayOrEmpty();
        using (var session = sessionFactory.OpenSession())
        {
            foreach (var entity in entities)
                await session.SaveAsync(entity);

            await session.FlushAsync();
        }
    }

    public static async Task UpdateEntitiesAsync<TEntity>(this ISessionFactory sessionFactory, IEnumerable<TEntity> enumerable)
    {
        var entities = enumerable.ToArrayOrEmpty();
        using (var session = sessionFactory.OpenSession())
        {
            foreach (var entity in entities)
                await session.UpdateAsync(entity);

            await session.FlushAsync();
        }
    }

    public static async Task<TEntity[]> GetEntitiesAsync<TEntity>(this ISessionFactory sessionFactory)
    {
        TEntity[] entitiesInDb;
        using (var session = sessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TEntity>().ToArray();
        }

        return await Task.FromResult(entitiesInDb);
    }
}