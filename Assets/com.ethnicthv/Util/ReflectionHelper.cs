using System;
using System.Linq;
using System.Reflection;

namespace com.ethnicthv.Util
{
    public static class ReflectionHelper
    {
        public static Type[] GetClassesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            // Get all assemblies in the current AppDomain
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Get all classes in the assemblies that are marked with the specific attribute
            var classes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && t.GetCustomAttributes(typeof(TAttribute), false).Length > 0)
                .ToArray();

            return classes;
        }
        
        public static MethodInfo[] GetMethodsWithAttribute<TAttribute>(Type classType) where TAttribute : Attribute
        {
            // Get all methods in the class that are marked with the specific attribute
            var methods = classType.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(TAttribute), false).Length > 0)
                .ToArray();

            return methods;
        }
        
        public static Type GetDelegateType(Type returnType, params Type[] parameterTypes)
        {
            var delegateGenericTypeDefinition = typeof(Func<,>).GetGenericTypeDefinition();
            var delegateType = delegateGenericTypeDefinition.MakeGenericType(parameterTypes.Concat(new[] { returnType }).ToArray());
            return delegateType;
        }

        public static Delegate CreateDelegate(Type delegateType, object target, MethodInfo method)
        {
            var delegateInstance = Delegate.CreateDelegate(delegateType, target, method);
            return delegateInstance;
        }
    }
}