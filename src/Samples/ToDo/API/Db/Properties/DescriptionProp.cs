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

    public class FindByEqualTo<TEntity> : ApiSpecificationBase<TEntity> where TEntity : ApiEntityBase, Interface, new()
    {
        #region Properties

        private readonly bool caseSensitive;

        private readonly string value;

        #endregion

        #region Constructors

        public FindByEqualTo(string value, bool caseSensitive = false)
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

    public class FindByContainedTerm<TEntity> : ApiSpecificationBase<TEntity> where TEntity : ApiEntityBase, Interface, new()
    {
        #region Properties

        readonly bool caseSensitive;

        private readonly string term;

        #endregion

        #region Constructors

        public FindByContainedTerm(string term, bool caseSensitive = false)
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