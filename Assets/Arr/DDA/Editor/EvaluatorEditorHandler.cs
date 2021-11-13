using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arr.DDA.Editor
{
    public class EvaluatorEditorHandler
    {
        private Type[] evaluators;
        
        public Type[] FindEvaluator()
        {
            if (evaluators != null) return evaluators;
            
            IEnumerable<Type> totalTypes = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<Type> typeToAdd;
                try
                {
                    typeToAdd = assembly.GetTypes().Where(t => typeof(IEvaluator).IsAssignableFrom(t) && !t.IsInterface);
                } catch (ReflectionTypeLoadException e) {
                    typeToAdd =  e.Types.Where(t => t != null && typeof(IEvaluator).IsAssignableFrom(t) && !t.IsInterface);
                }
        
                totalTypes = totalTypes.Concat(typeToAdd).ToArray();
            }

            evaluators = totalTypes.ToArray();
        
            return evaluators;
        }

        public Type GetEvaluatorType(int index) => FindEvaluator()[index];
    }
}