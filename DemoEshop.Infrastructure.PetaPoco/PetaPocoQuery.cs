using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncPoco;
using DemoEshop.Infrastructure.PetaPoco.UnitOfWork;
using DemoEshop.Infrastructure.Query;
using DemoEshop.Infrastructure.Query.Predicates;
using DemoEshop.Infrastructure.Query.Predicates.Operators;
using DemoEshop.Infrastructure.UnitOfWork;

namespace DemoEshop.Infrastructure.PetaPoco
{
    public class PetaPocoQuery<TEntity> : QueryBase<TEntity> where TEntity : class, IEntity, new()
    {
        #region SQLStatements
        private const string SelectFromClause = "SELECT * FROM ";
        private const string WhereClause = "WHERE ";
        private const string OrderByClause = "ORDER BY ";
        private const string Ascending = " ASC";
        private const string Descending = " DESC";
        private const string Or = " OR ";
        private const string And = " AND ";
        private const string OpenParenthesis = "(";
        private const string CloseParenthesis = ")";
        #endregion       

        /// <summary>
        /// Gets the <see cref="IDatabase"/>.
        /// </summary>
        protected IDatabase Database => ((PetaPocoUnitOfWork)Provider.GetUnitOfWorkInstance()).Database;

        public PetaPocoQuery(IUnitOfWorkProvider provider) : base(provider) { }

        public override async Task<QueryResult<TEntity>> ExecuteAsync()
        {
            QueryResult<TEntity> result;           
            var sql = Sql.Builder
                .Append($"{SelectFromClause}{new TEntity().TableName}");

            if (Predicate != null)
            {
                sql.Append($"{WhereClause}{(Predicate is CompositePredicate composite ? BuildCompositePredicate(composite) : BuildSimplePredicate(Predicate as SimplePredicate))}");
            }
            if (!string.IsNullOrWhiteSpace(SortAccordingTo))
            {
                sql.Append(OrderByClause + SortAccordingTo + (UseAscendingOrder ? Ascending : Descending));
            }
            Debug.WriteLine(sql.SQL);
            if (DesiredPage.HasValue)
            {
                var page = await Database.PageAsync<TEntity>(DesiredPage.Value, PageSize, sql);
                result = new QueryResult<TEntity>(page.Items, page.TotalItems, (int)page.ItemsPerPage, (int)page.CurrentPage);
            }
            else
            {
                var items = await Database.FetchAsync<TEntity>(sql);
                result = new QueryResult<TEntity>(items, items.Count);
            }
            return result;
        }

        private string BuildCompositePredicate(CompositePredicate compositePredicate)
        {
            if (compositePredicate.Predicates.Count == 0)
            {
                throw new InvalidOperationException("At least one simple predicate must be given");
            }
            var sql = OpenParenthesis;
            sql += ChoosePredicate(compositePredicate, 0);
            for (var i = 1; i < compositePredicate.Predicates.Count; i++)
            {
                sql += compositePredicate.Operator == LogicalOperator.OR ? Or : And;
                sql += ChoosePredicate(compositePredicate, i);
            }
            return sql + CloseParenthesis;
        }
        
        private static string BuildSimplePredicate(IPredicate predicate)
        {
            var simplePredicate = predicate as SimplePredicate;
            if (simplePredicate == null)
            {
                throw new ArgumentException("Expected simple predicate!");
            }
            return simplePredicate.GetWhereCondition();
        }

        private string ChoosePredicate(CompositePredicate compositePredicate, int index)
        {
            return compositePredicate.Predicates[index] is CompositePredicate predicate
                ? BuildCompositePredicate(predicate)
                : BuildSimplePredicate(compositePredicate.Predicates[index]);
        }
    }
}
