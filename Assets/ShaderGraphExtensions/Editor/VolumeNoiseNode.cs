using System.Reflection;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Drawing.Controls;
using UnityEngine;

namespace DifferentMethods.ShaderGraph
{

    [Title("DM", "Volume Noise")]
    public class VolumeNoiseNode : NoiseCodeFunctionNode
    {

        public VolumeNoiseNode()
        {
            name = "Volume Noise 3D";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("UnfilteredVolumeNoise", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static string UnfilteredVolumeNoise(
            [Slot(0, Binding.ObjectSpacePosition)] Vector3 Pos,
            [Slot(1, Binding.None)] Vector3 Offset,
            [Slot(2, Binding.None, 10, 10, 10, 10)] Vector1 Frequency,
            [Slot(3, Binding.None)] out Vector1 Out)
        {
            return @"{ Out = dm_SimplexNoise((Offset+Pos)*Frequency) + 1 * 0.5; }";
        }
    }

}


