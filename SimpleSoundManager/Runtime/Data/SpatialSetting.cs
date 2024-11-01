using System;
using UnityEngine;

namespace Postive.SimpleSoundAssetManager.Runtime.Data
{
    [Serializable]
    public class SpatialSetting
    {
        public float MinDistance = 1;
        public float MaxDistance = 30;
        [Range(0f,5f)] public float DopplerLevel = 1;
        public AnimationCurve VolumeRolloff = AnimationCurve.EaseInOut(0, 1, 1, 0);
        public AnimationCurve SpatialBlend = AnimationCurve.EaseInOut(0, 1, 1, 0);
        public AnimationCurve Spread = AnimationCurve.EaseInOut(0, 0, 0, 1);
        public AnimationCurve ReverbZoneMix = AnimationCurve.EaseInOut(0, 1, 1, 1);
    }
}