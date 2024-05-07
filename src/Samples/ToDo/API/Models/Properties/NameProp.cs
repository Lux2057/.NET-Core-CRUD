namespace Samples.ToDo.API;

#region << Using >>

using System.Linq.Expressions;
using Extensions;

#endregion

public abstract class NameProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public string Name { get; set; }

        #endregion
    }

    public abstract class FindBy
    {
        #region Nested Classes

        public class EqualTo<TEntity> : SpecificationBase<TEntity> where TEntity : EntityBase, Interface, new()
        {
            #region Properties

            private readonly bool caseSensitive;

            private readonly string value;

            #endregion

            #region Constructors

            public EqualTo(string value, bool caseSensitive = false)
            {
                this.value = value;
                this.caseSensitive = caseSensitive;
            }

            #endregion

            public override Expression<Func<TEntity, bool>> ToExpression()
            {
                if (this.caseSensitive)
                    return x => x.Name == this.value;

                return x => x.Name.ToLower() == this.value.ToLower();
            }
        }

        #endregion
    }

    #endregion
}