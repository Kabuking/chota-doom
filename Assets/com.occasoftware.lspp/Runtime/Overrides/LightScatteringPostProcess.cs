using System;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.LSPP.Runtime
{
    [Serializable, VolumeComponentMenu("OccaSoftware/Light Scattering")]
    public sealed class LightScatteringPostProcess : VolumeComponent, IPostProcessComponent
    {
        [Header("Performance Settings")]
        [Tooltip("Control the quality of the effect. Higher values are more computationally expensive but can provide smoother results.")]
        public ClampedIntParameter numberOfSamples = new ClampedIntParameter(16, 4, 64);

        [Space()]
        [Header("Basic Appearance Settings")]
        [Tooltip("Set the fog density. Higher values result in a more intense light scattering result.")]
        public MinFloatParameter fogDensity = new MinFloatParameter(0, 0);

        [Tooltip("Determine how far to sample the occluder texture (in screen space distance).")]
        public ClampedFloatParameter maxRayDistance = new ClampedFloatParameter(0.5f, 0.01f, 1.0f);

        [Tooltip("Set the fog color.")]
        public ColorParameter tint = new ColorParameter(Color.white, true, false, true);

        [Space()]
        [Header("Additional Appearance Settings")]
        [Tooltip("When enabled, we assume the area outside of the screen bounds is unoccluded.")]
        public BoolParameter softenScreenEdges = new BoolParameter(false);

        [Tooltip(
            "When disabled, each pixel has a fixed random offset to replace banding with noise. When enabled, each pixel has a random per-frame offset to replace static noise with variable noise."
        )]
        public BoolParameter animateSamplingOffset = new BoolParameter(false);

        [Tooltip("When enabled, the effect will immediately disappear when the main light source moves offscreen.")]
        public BoolParameter lightMustBeOnScreen = new BoolParameter(false);

        [Tooltip(
            "Set whether the light falloff is based on the individual pixel ray direction or the camera forward direction (which is the same for every pixel)."
        )]
        public FalloffBasisParameter falloffBasis = new FalloffBasisParameter(FalloffBasis.RayDirection);

        [Tooltip("Set the occlusion value for off-screen pixels. 1 = Not Occluded. 0 = Occluded.")]
        public ClampedFloatParameter occlusionAssumption = new ClampedFloatParameter(0f, 0f, 1f);

        [Tooltip(
            "LSPP assumes that an occluder blocks less light as the ray gets farther away. This option changes that assumption. 1 = Distant occluders block no light. 0 = Distant occluders block full light."
        )]
        public ClampedFloatParameter occlusionOverDistanceAmount = new ClampedFloatParameter(1f, 0f, 1f);

        [Tooltip("Controls the strength of the falloff effect. Higher values cause the light scattering effect to weaken more quickly.")]
        public ClampedFloatParameter falloffIntensity = new ClampedFloatParameter(3f, 1f, 10f, false);

        public bool IsActive()
        {
            return fogDensity.value > 0;
        }

        public bool IsTileCompatible() => false;
    }

    public enum FalloffBasis
    {
        RayDirection,
        CameraForward
    }

    [Serializable]
    public sealed class FalloffBasisParameter : VolumeParameter<FalloffBasis>
    {
        public FalloffBasisParameter(FalloffBasis value, bool overrideState = false)
            : base(value, overrideState) { }
    }
}