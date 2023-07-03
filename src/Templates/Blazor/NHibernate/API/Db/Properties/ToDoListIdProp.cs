namespace Templates.Blazor.NH.API;

#region << Using >>

using System.Linq.Expressions;
using CRUD.Extensions;

#endregion

public abstract class ToDoListProp
{
    #region Nested Classes

    public interface Interface
    {
        #region Properties

        public ToDoListEntity ToDoList { get; set; }

        #endregion
    }

    public class FindById<TEntity> : ApiSpecificationBase<TEntity> where TEntity : ApiEntityBase, Interface, new()
    {
        #region Properties

        private readonly int value;

        #endregion

        #region Constructors

        public FindById(int value)
        {
            this.value = value;
        }

        #endregion

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => x.ToDoList.Id == this.value;
        }
    }

    #endregion
}