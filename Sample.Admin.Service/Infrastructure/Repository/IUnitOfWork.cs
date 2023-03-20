namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        #region Declaration
        /// <summary>
        /// Commit method to save changes in the database
        /// </summary>
        void Commit();

        #endregion
    }
}
