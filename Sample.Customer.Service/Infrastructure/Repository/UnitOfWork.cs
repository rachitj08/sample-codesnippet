using System.ComponentModel.DataAnnotations;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CloudAcceleratorContext DbContext;
        public UnitOfWork(CloudAcceleratorContext dbContext)
        {
            DbContext = dbContext;
        }

        

        /// <summary>
        /// Method to save changes in the database
        /// </summary>
        public void Commit()
        {

            #region ONLY FOR DEBUG
            //var entities = from e in DbContext.ChangeTracker.Entries()
            //               where e.State == EntityState.Added
            //                   || e.State == EntityState.Modified
            //               select e.Entity;
            //foreach (var entity in entities)
            //{
            //    try
            //    {
            //        var validationContext = new ValidationContext(entity);
            //        Validator.ValidateObject(entity, validationContext);
            //    }
            //    catch(ValidationException Ve)
            //    {

            //    }
            //}
            #endregion
            DbContext.SaveChanges();
        }

        /// <summary>
        /// Method to save changes in the database
        /// </summary>
        public int CommitWithStatus()
        { 
           return DbContext.SaveChanges();
        }
    }

}

