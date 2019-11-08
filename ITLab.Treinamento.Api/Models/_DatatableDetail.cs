using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ITLab.Treinamento.Api.Models
{
    public class _DatatableDetail
    {
        public int start { get; set; }
        public int length { get; set; }
        public string orderByColumn { get; set; }
        public bool orderByAsc { get; set; }
    }

    public static class CustomOrderBy
    {
        public static IQueryable<TEntity> OrderByPropertyName<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool? asc)
        {
            if (!asc.HasValue) asc = true;

            var command = asc.Value ? "OrderBy" : "OrderByDescending";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}