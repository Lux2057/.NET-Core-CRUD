﻿namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;
using Tests.Models;

#endregion

public class ReadDispatcherTest : DbTest
{
    #region Properties

    protected TestDbContext context;

    protected IReadDispatcher dispatcher;

    #endregion

    #region Constructors

    public ReadDispatcherTest(TestDbContext context, IReadDispatcher dispatcher)
    {
        this.context = context;
        this.dispatcher = dispatcher;
    }

    #endregion
}