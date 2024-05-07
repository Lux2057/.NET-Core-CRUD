namespace Samples.ToDo.API;

#region << Using >>

using System.Linq.Expressions;
using Extensions;

#endregion

public abstract class UserIdProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public int UserId { get; set; }

        #endregion
    }

    public abstract class FindBy
    {
        #region Nested Classes

        public class ContainedIn<TEntity> : SpecificationBase<TEntity> where TEntity : EntityBase, Interface, new()
        {
            #region Properties

            private readonly int[] ids;

            #endregion

            #region Constructors

            public ContainedIn(IEnumerable<int> ids)
            {
                this.ids = ids.ToDistinctArrayOrEmpty();
            }

            #endregion

            public override Expression<Func<TEntity, bool>> ToExpression()
            {
                if (this.ids.Length == 0)
                    return x => true;

                return x => this.ids.Contains(x.UserId);
            }
        }

        public class EqualTo<TEntity> : SpecificationBase<TEntity> where TEntity : EntityBase, Interface, new()
        {
            #region Properties

            private readonly int id;

            #endregion

            #region Constructors

            public EqualTo(int id)
            {
                this.id = id;
            }

            #endregion

            public override Expression<Func<TEntity, bool>> ToExpression()
            {
                return x => x.UserId == this.id;
            }
        }

        #endregion
    }

    #endregion
}