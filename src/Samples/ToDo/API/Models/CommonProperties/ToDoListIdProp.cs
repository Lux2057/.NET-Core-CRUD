namespace Samples.ToDo.API;

#region << Using >>

using System.Linq.Expressions;
using Extensions;

#endregion

public abstract class ToDoListIdProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public int ToDoListId { get; set; }

        #endregion
    }

    public abstract class FindBy
    {
        #region Nested Classes

        public class EqualTo<TEntity> : SpecificationBase<TEntity> where TEntity : EntityBase, Interface, new()
        {
            #region Properties

            private readonly int value;

            #endregion

            #region Constructors

            public EqualTo(int value)
            {
                this.value = value;
            }

            #endregion

            public override Expression<Func<TEntity, bool>> ToExpression()
            {
                return x => x.ToDoListId == this.value;
            }
        }

        public class ContainedIn<TEntity> : SpecificationBase<TEntity> where TEntity : EntityBase, Interface, new()
        {
            #region Properties

            private readonly int[] values;

            #endregion

            #region Constructors

            public ContainedIn(IEnumerable<int> values)
            {
                this.values = values.ToDistinctArrayOrEmpty();
            }

            #endregion

            public override Expression<Func<TEntity, bool>> ToExpression()
            {
                if (!this.values.Any())
                    return x => true;

                return x => this.values.Contains(x.ToDoListId);
            }
        }

        #endregion
    }

    #endregion
}