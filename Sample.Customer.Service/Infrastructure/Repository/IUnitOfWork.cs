namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        
        /// <summary>
        /// Commit method to save changes in the database
        /// </summary>
        void Commit();

        int CommitWithStatus();
    }
}
