using System;

namespace Arr.DDA.Script
{
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EvaluatorAttribute : Attribute
    {
        public EvaluatorAttribute()
        {
            
        }
    }
}