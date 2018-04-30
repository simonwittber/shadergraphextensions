using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Bars")]
    public class BarsNode : NoiseCodeFunctionNode
    {
        public BarsNode()
        {
            name = "Bars  3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("BarsNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string BarsNoise(
            [Slot(0, Binding.ObjectSpacePosition)] Vector3 Pos,
            [Slot(1, Binding.None)] Vector3 Offset,
            [Slot(2, Binding.None, 1, 1, 1, 1)] Vector3 Scale,

            [Slot(99, Binding.None)] out Vector1 Out)
        {
            return @"
{
    float3 S = 1/Scale;
    float3 N = (floor(Pos * S)*Scale);
    Out = length(Pos-N);
}
";
        }

    }

}
