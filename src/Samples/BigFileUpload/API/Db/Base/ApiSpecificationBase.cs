namespace Samples.UploadBigFile.API;

#region << Using >>

using LinqSpecs;

#endregion

public abstract class ApiSpecificationBase<TEntity> : Specification<TEntity> where TEntity : ApiEntityBase, new() { }