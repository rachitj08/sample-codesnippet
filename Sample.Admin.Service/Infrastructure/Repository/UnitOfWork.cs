using Sample.Admin.Service.Infrastructure.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
       
        private readonly CloudAcceleratorContext DbContext;

        /// <summary>
        /// Unit of work Constructor to inject dependency
        /// </summary>
        /// <param name="dbContext">The db context parameter</param>
        public UnitOfWork(CloudAcceleratorContext dbContext)
        {
            DbContext = dbContext;
        }

        

        /// <summary>
        /// Method to save changes in the database
        /// </summary>
        public void Commit()
        {
            try
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
            catch (ValidationException ve)
            {
                throw ve; 
            }           

        }

    }
}
