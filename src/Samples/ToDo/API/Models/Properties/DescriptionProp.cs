namespace Samples.ToDo.API;

#region << Using >>

using System.Linq.Expressions;

#endregion

public abstract class DescriptionProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public string Description { get; set; }

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
                    return x => x.Description == this.value;

                return x => x.Description.ToLower() == this.value.ToLower();
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
                if (this.caseSensitive)
                    return x => x.Description.Contains(this.term);

                return x => x.Description.ToLower().Contains(this.term.ToLower());
            }
        }

        #endregion
    }

    #endregion
}

public abstract class UserProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public int UserId { get; set; }

        public UserEntity User { get; set; }

        #endregion
    }

    #endregion
}