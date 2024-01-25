namespace Samples.ToDo.API;

#region << Using >>

using LinqSpecs;

#endregion

public abstract class SpecificationBase<TEntity> : Specification<TEntity> where TEntity : EntityBase, new() { }