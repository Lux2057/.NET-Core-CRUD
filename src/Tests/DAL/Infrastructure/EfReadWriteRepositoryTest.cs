﻿namespace EfTests.DAL;

#region << Using >>

using CRUD.DAL;
using Tests.Models;

#endregion

public abstract class EfReadWriteRepositoryTest : DbTest
{
    #region Properties

    protected readonly TestDbContext context;

    protected readonly IReadWriteRepository<TestEntity> repository;

    #endregion

    #region Constructors

    protected EfReadWriteRepositoryTest(TestDbContext context, IReadWriteRepository<TestEntity> repository)
    {
        this.context = context;
        this.repository = repository;
    }

    #endregion
}