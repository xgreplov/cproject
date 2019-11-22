using System;
using System.Reflection;
using System.Threading.Tasks;
using AsyncPoco;

namespace DemoEshop.Infrastructure.PetaPoco
{
    public static class DatabaseExtensions
    {
        public static async Task<object> InvokeSingleOrDefaultAsync(this IDatabase database, Type typeParameter, Guid id)
        {
            var genericMethod = typeof(Database)
                .GetMethod(nameof(Database.SingleOrDefaultAsync), BindingFlags.Public | BindingFlags.Instance, null,
                    new[] { typeof(object) }, null)
                ?.MakeGenericMethod(typeParameter);
            dynamic awaitable = genericMethod?.Invoke(database, new[] { (object)id }) ??
                                throw new InvalidOperationException("No such method exists!");
            await awaitable;
            object value = awaitable.GetAwaiter().GetResult();
            return value;
        }
    }
}
