namespace Templates.Blazor.NH.API;

#region << Using >>

using LinqSpecs;

#endregion

public abstract class ApiSpecificationBase<TEntity> : Specification<TEntity> where TEntity : ApiEntityBase, new() { }