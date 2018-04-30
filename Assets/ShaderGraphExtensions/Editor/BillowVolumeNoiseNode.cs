using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Billow Volume Noise")]
    public class BillowVolumeNoiseNode : NoiseCodeFunctionNode
    {
        public BillowVolumeNoiseNode()
        {
            name = "Billow Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("BillowVolumeNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public override void GenerateNodeFunction(FunctionRegistry registry, GenerationMode generationMode)
        {
            base.GenerateNodeFunction(registry, generationMode);

        }

        static string BillowVolumeNoise(
            [Slot(0, Binding.ObjectSpacePosition)] Vector3 Pos,
            [Slot(8, Binding.None)] Vector3 Offset,
            [Slot(1, Binding.None, 3, 3, 3, 3)] Vector1 Octaves,
            [Slot(3, Binding.None, 1, 1, 1, 1)] Vector1 Frequency,
            [Slot(4, Binding.None, 1, 1, 1, 1)] Vector1 Gain,
            [Slot(5, Binding.None, 5, 5, 5, 5)] Vector1 Lacunarity,
            [Slot(6, Binding.None, 0.5f, 0.5f, 0.5f, 0.5f)] Vector1 Persistence,
            [Slot(9, Binding.None, 0.5f, 0.5f, 0.5f, 0.5f)] Vector1 Bias,
            [Slot(7, Binding.None)] out Vector1 Out)
        {
            return @"
{
    float F = Frequency;
    float A = Gain;
    float N = 0;
    for (int i = 1; i < Octaves; i++)
    {
        float3 pos = Offset + Pos;
        float s = dm_SimplexNoise(pos*F);
        s *= A;
        s = abs(s);
        N += (s * Lacunarity) + Bias;
        F *= Lacunarity;
        A *= Persistence;
    }
    Out = (N + 1) * 0.5;
}
            ";
        }

    }
}
