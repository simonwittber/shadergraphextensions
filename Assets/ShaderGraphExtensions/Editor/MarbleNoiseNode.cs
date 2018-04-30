using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Marble Noise")]
    public class MarbleNoiseNode : NoiseCodeFunctionNode
    {
        public MarbleNoiseNode()
        {
            name = "Marble Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("MarbleNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string MarbleNoise(
            [Slot(0, Binding.ObjectSpacePosition)] Vector3 Pos,
            [Slot(1, Binding.None)] Vector3 Offset,
            [Slot(3, Binding.None, 2, 2, 2, 2)] Vector1 VeinFrequency,

            [Slot(5, Binding.None, 10, 10, 10, 10)] Vector1 VeinFalloff,
            [Slot(6, Binding.None, 0.5f, 0.5f, 0.5f, 0.5f)] Vector1 NoiseFrequency,
            [Slot(7, Binding.None)] out Vector1 Out)
        {
            return @"
{
    float N = 0;
    float3 pos = Offset + Pos;
    
    float x = 0;
    x += dm_SimplexNoise(pos * NoiseFrequency * 1);
    x += dm_SimplexNoise(pos * NoiseFrequency * 2);
    x += dm_SimplexNoise(pos * NoiseFrequency * 3);
    
    N = sin(x * VeinFrequency);
    
    Out = 1-pow((N + 1) * 0.5, VeinFalloff);
}
";
        }

    }

}
