using System.Reflection;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{
    [Title("DM", "Ridge Volume Noise")]
    public class RidgeVolumeNoiseNode : NoiseCodeFunctionNode
    {
        public RidgeVolumeNoiseNode()
        {
            name = "Ridge Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("RidgeVolumeNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public override void GenerateNodeFunction(FunctionRegistry registry, GenerationMode generationMode)
        {
            base.GenerateNodeFunction(registry, generationMode);

        }

        static string RidgeVolumeNoise(
            [Slot(0, Binding.ObjectSpacePosition)] Vector3 Pos,
            [Slot(8, Binding.None)] Vector3 Offset,
            [Slot(1, Binding.None, 3, 3, 3, 3)] Vector1 Octaves,
            [Slot(3, Binding.None, 1, 1, 1, 1)] Vector1 Frequency,
            [Slot(4, Binding.None, 1, 1, 1, 1)] Vector1 Gain,
            [Slot(5, Binding.None, 5, 5, 5, 5)] Vector1 Lacunarity,
            [Slot(6, Binding.None, 0.5f, 0.5f, 0.5f, 0.5f)] Vector1 Persistence,
            [Slot(7, Binding.None)] out Vector1 Out)
        {
            return @"
{
    float F = Frequency;
    float A = Gain;
    float3 pos = Offset + Pos;
    float N = 1.0 - abs(dm_SimplexNoise(pos*F));
    N *= N;
    float s = N;
    for (int i = 1; i < Octaves; i++)
    {
        float weight = saturate(s * 2);
        F *= Lacunarity;
        A *= Persistence;
        pos = Offset + Pos;
        s = 1.0 - abs(dm_SimplexNoise(pos*F));
        s *= s;
        s *= weight;
        N += s * A;   
    }
    Out = N;
}
            ";
        }

    }
}
