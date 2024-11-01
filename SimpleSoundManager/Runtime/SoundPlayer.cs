using System;
using Postive.SimpleSoundAssetManager.Runtime.Attributes;
using UnityEngine;

namespace Postive.SimpleSoundAssetManager.Runtime
{
    [Serializable]
    public class SoundPlayer
    {
        public string SoundName {
            get => _soundName;
            set => _soundName = value;
        }
        [SoundSelector]
        [SerializeField] private string _soundName = "NONE";
        public void PlaySoundAtPosition(Vector3 position) {
            SoundManager.PlaySound(_soundName, position);
        }
        public void PlaySoundAtTransform(Transform transform){
            SoundManager.PlaySoundAtTransform(_soundName, transform);
        }
    }
}