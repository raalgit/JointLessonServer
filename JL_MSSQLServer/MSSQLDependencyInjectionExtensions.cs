using JL_MSSQLServer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace JL_MSSQLServer
{
    public static class MSSQLDependencyInjectionExtensions
    {
        public static void AddIRepository(this IServiceCollection serviceCollection)
        {
            // Получение базового интерфейса 
            var baseInterfaceType = typeof(IRepository<>);

            // Получение всех интерфейсов и классов
            var interfaceAssemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();

            // Получение всех интерфейсов унаследованных от базового за исключением самого базового
            var utilityInterfaces =
                interfaceAssemblies.Where(x =>
                    x.IsInterface &&
                    x.GetInterfaces()
                        .Any(y =>
                            y.IsGenericType &&
                            y != baseInterfaceType &&
                            y.GetGenericTypeDefinition() == baseInterfaceType
                            )
                        )
                .ToList();

            if (utilityInterfaces == null || utilityInterfaces.Count == 0) return;

            foreach (var iUtility in utilityInterfaces)
            {
                // Получение класса утилиты для текущего интерфейса
                var utilityClass =
                    interfaceAssemblies
                    .Where(x => !x.IsInterface && iUtility.IsAssignableFrom(x))
                    .ToList();

                if (utilityClass != null && utilityClass.Count > 0) serviceCollection.AddScoped(iUtility, utilityClass.First());
            }
        }
    }
}
