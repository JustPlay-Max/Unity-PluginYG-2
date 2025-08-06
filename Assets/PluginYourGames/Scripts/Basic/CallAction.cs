using System;
using System.Collections.Generic;
using System.Reflection;

namespace YG.Insides
{
    public static class CallAction
    {
        public static void CallIByAttribute(Type attribute, Type type, object instance = null)
        {
            List<Action> methodsToCall = new List<Action>();

            MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (MethodInfo method in methods)
            {
                var attributes = method.GetCustomAttributes(attribute, true);
                if (attributes.Length > 0)
                {
                    MethodInfo currentMethod = method;

                    object inst = attribute;
                    if (instance != null)
                        inst = instance;

                    methodsToCall.Add(() => currentMethod.Invoke(inst, null));
                }
            }
            foreach (var action in methodsToCall)
            {
                action.Invoke();
            }
        }
    }
}