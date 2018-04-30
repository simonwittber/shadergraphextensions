using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Voronoi Noise")]
    public class VoronoiNoiseNode : NoiseCodeFunctionNode
    {
        public VoronoiNoiseNode()
        {
            name = "Voronoi Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("VoronoiNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string VoronoiNoise(
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
    Out = N.y-N.x;
}
";
        }

    }
}
