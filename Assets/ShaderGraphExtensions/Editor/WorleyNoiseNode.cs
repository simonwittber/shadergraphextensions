using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Worley Noise")]
    public class WorleyNoiseNode : NoiseCodeFunctionNode
    {
        public WorleyNoiseNode()
        {
            name = "Worley Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("WorleyNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string WorleyNoise(
            [Slot(0, Binding.ObjectSpacePosition)] Vector3 Pos,
            [Slot(1, Binding.None)] Vector3 Offset,
            [Slot(2, Binding.None, 10, 10, 10, 10)] Vector1 Frequency,
            [Slot(3, Binding.None)] out Vector1 Out)
        {
            return @"
{
    float2 N = 0;
    float3 pos = Offset + Pos;
    N = dm_Cellular(pos*Frequency);
    Out = min(N.y, N.x);
}
";
        }

    }
}
