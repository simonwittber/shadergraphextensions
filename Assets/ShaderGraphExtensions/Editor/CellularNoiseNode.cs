using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Cellular Noise")]
    public class CellularNoiseNode : NoiseCodeFunctionNode
    {
        public CellularNoiseNode()
        {
            name = "Cellular Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("CellularNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string CellularNoise(
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
    Out = (N.y + N.x) * 0.5;
}
";
        }

    }
}
