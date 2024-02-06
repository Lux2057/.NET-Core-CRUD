namespace Samples.ToDo.API;

#region << Using >>

using System.Linq.Expressions;

#endregion

public abstract class IsDeletedProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public bool IsDeleted { get; set; }

        #endregion
    }

    public abstract class FindBy
    {
        #region Nested Classes

        public class EqualTo<TEntity> : SpecificationBase<TEntity> where TEntity : EntityBase, Interface, new()
        {
            #region Properties

            private readonly bool value;

            #endregion

            #region Constructors

            public EqualTo(bool value)
            {
                this.value = value;
            }

            #endregion

            public override Expression<Func<TEntity, bool>> ToExpression()
            {
                return x => x.IsDeleted == this.value;
            }
        }

        #endregion
    }

    #endregion
}