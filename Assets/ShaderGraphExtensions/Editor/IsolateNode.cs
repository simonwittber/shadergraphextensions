using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Isolate")]
    public class IsolateNode : CodeFunctionNode
    {

        public ColorField color;

        public IsolateNode()
        {
            name = "Isolate Node";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("GenerateShader", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string GenerateShader(
            [Slot(0, Binding.None, 0.5f, 0.5f, 0.5f, 0.5f)] DynamicDimensionVector Center,
            [Slot(1, Binding.None, 1f, 1f, 1f, 1f)] DynamicDimensionVector Width,
            [Slot(2, Binding.None)] DynamicDimensionVector In,
            [Slot(3, Binding.None)] out DynamicDimensionVector Out)
        {
            return @"
{ 
    In = abs(In - Center);
    if(In>Width) 
        Out = 0;
    else {
        In /= Width;
        Out = 1.0-In*In*(3.0-2.0*In);
    }
}";
        }

    }
}
