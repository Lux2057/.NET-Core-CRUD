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

        public class ContainedTerm<TEntity> : SpecificationBase<TEntity> where TEntity : EntityBase, Interface, new()
        {
            #region Properties

            readonly bool caseSensitive;

            private readonly string term;

            #endregion

            #region Constructors

            public ContainedTerm(string term, bool caseSensitive = false)
            {
                this.term = term;
                this.caseSensitive = caseSensitive;
            }

            #endregion

            public override Expression<Func<TEntity, bool>> ToExpression()
            {
                if (this.term.IsNullOrWhitespace())
                    return x => true;

                if (this.caseSensitive)
                    return x => x.Name.Contains(this.term);

                return x => x.Name.ToLower().Contains(this.term.ToLower());
            }
        }

        #endregion
    }

    #endregion
}