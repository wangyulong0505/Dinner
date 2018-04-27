using Dinner.WebApi.Unit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Dinner.WebApi
{
    public static class ServiceExtension
    {
        /// <summary>
        /// 用DI批量注入接口程序集中对应的实现类
        /// </summary>
        /// 需要注意的是，这里有如下约定：
        /// IUserService --> UserService, IUserRepository --> UserRepository.
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembly(this IServiceCollection service, string interfaceAssemblyName)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (interfaceAssemblyName == null)
            {
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            }
            var assembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException($"the dll {interfaceAssemblyName} not be found");
            }
            //
            var types = assembly.GetTypes().Where(w => w.IsInterface && !w.IsGenericType);
            foreach (var type in types)
            {
                var implementTypeName = type.Name;
                var implementType = RuntimeHelper.GetImplementType(implementTypeName, type);
                if (implementType != null)
                {
                    service.AddSingleton(type, implementType);
                }
            }
            return service;
        }

        /// <summary>
        /// 用DI批量注入接口程序集中对应的实现类。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <param name="implementAssemblyName">实现程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembly(this IServiceCollection service, string interfaceAssemblyName, string implementAssemblyName)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            if (interfaceAssemblyName == null)
            {
                throw new ArgumentNullException(nameof(interfaceAssemblyName));
            }
            var interfaceAssembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (interfaceAssembly == null)
            {
                throw new DllNotFoundException($"the dll {interfaceAssemblyName} not be found");
            }
            var implementAssembly = RuntimeHelper.GetAssembly(implementAssemblyName);
            //过滤掉非接口和泛型接口
            var types = interfaceAssembly.GetTypes().Where(w => w.GetTypeInfo().IsInterface && !w.GetTypeInfo().IsGenericType);
            foreach (var type in types)
            {
                var implementType = implementAssembly.DefinedTypes.FirstOrDefault(t => t.IsClass && !t.IsGenericType && !t.IsAbstract && t.GetInterfaces().Any(p => p.Name == type.Name));
                if (implementType != null)
                {
                    service.AddSingleton(type, implementType.AsType());
                }
            }
            return service;
        }
    }
}
