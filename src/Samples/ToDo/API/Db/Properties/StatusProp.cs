namespace Samples.ToDo.API;

#region << Using >>

using System.Linq.Expressions;
using Extensions;
using Samples.ToDo.Shared;

#endregion

public abstract class StatusProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public ItemStatus Status { get; set; }

        #endregion
    }

    public class FindByValue<TEntity> : ApiSpecificationBase<TEntity> where TEntity : ApiEntityBase, Interface, new()
    {
        #region Properties

        private readonly ItemStatus value;

        #endregion

        #region Constructors

        public FindByValue(ItemStatus value)
        {
            this.value = value;
        }

        #endregion

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => x.Status == this.value;
        }
    }

    public class FindByContainsIn<TEntity> : ApiSpecificationBase<TEntity> where TEntity : ApiEntityBase, Interface, new()
    {
        #region Properties

        private readonly ItemStatus[] values;

        #endregion

        #region Constructors

        public FindByContainsIn(IEnumerable<ItemStatus> values)
        {
            this.values = values.ToDistinctArrayOrEmpty();
        }

        #endregion

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            if (!this.values.Any())
                return x => true;

            return x => this.values.Contains(x.Status);
        }
    }

    #endregion
}