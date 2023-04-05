namespace CRUD.CQRS
{
    public abstract class CommandBase : ICommand
    {
        #region Properties

        public object Result { get; set; }

        #endregion
    }
}