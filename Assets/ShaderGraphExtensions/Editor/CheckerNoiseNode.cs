using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Checker Noise")]
    public class CheckerNoiseNode : NoiseCodeFunctionNode
    {
        public CheckerNoiseNode()
        {
            name = "Checker Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("CheckerNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string CheckerNoise(
            [Slot(0, Binding.ObjectSpacePosition)] Vector3 Pos,
            [Slot(1, Binding.None)] Vector3 Offset,
            [Slot(2, Binding.None, 1, 1, 1, 1)] Vector3 Scale,

            [Slot(99, Binding.None)] out Vector1 Out)
        {
            return @"
{
    float N = 0;
    float3 pos = floor((Offset + Pos) * (1/Scale))*Scale;
    N = frac(sin(dot(pos, float3(12.9898,78.233,43.141567))) * 43758.5453123);
    Out = N;
}
";
        }

    }

}
