using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncPoco;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.Infrastructure.PetaPoco.UnitOfWork
{
    public class PetaPocoUnitOfWork : UnitOfWorkBase
    {
        /// <summary>
        /// Gets the <see cref="Database"/>.
        /// </summary>
        public IDatabase Database { get; }

        private readonly IList<object> entitiesToInsert = new List<object>();
        private readonly IList<object> entitiesToUpdate = new List<object>();
        private readonly IDictionary<Guid, Type> entititiesToRemove = new Dictionary<Guid, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PetaPocoUnitOfWork"/> class.
        /// </summary>
        public PetaPocoUnitOfWork(Func<IDatabase> dbFactory)
        {
            this.Database = dbFactory?.Invoke() ?? throw new ArgumentException("Db factory cant be null!");
        }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        protected override async Task CommitCore()
        {
            using (var transaction = await Database.GetTransactionAsync())
            {
                foreach (var insertedEntity in entitiesToInsert)
                {
                    await Database.InsertAsync(insertedEntity);
                }
                foreach (var updatedEntity in entitiesToUpdate)
                {
                    await Database.UpdateAsync(updatedEntity);
                }
                foreach (var pair in entititiesToRemove)
                {
                    if (await Database.InvokeSingleOrDefaultAsync(pair.Value, pair.Key) is IEntity entityToRemove)
                    {
                        await Database.DeleteAsync(entityToRemove.TableName, nameof(entityToRemove.Id), entityToRemove);
                    }
                }
                entitiesToInsert.Clear();
                entitiesToUpdate.Clear();
                entititiesToRemove.Clear();

                transaction.Complete();
            }
        }

        internal void RegisterEntityToInsert(object entity) => entitiesToInsert.Add(entity);
        internal void RegisterEntityToUpdate(object entity) => entitiesToUpdate.Add(entity);
        internal void RegisterEntityToRemove<TEntity>(Guid entityId) where TEntity : IEntity 
           => entititiesToRemove.Add(entityId, typeof(TEntity));

        public override void Dispose()
        {
            Database.Dispose();
        }
    }
}
