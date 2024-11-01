using Postive.SimpleSoundAssetManager.Runtime.Attributes;
using UnityEngine;

namespace Postive.SimpleSoundAssetManager.Runtime
{
    public class SoundPlayerComponent : MonoBehaviour
    {
        public string SoundName {
            get => _soundName;
            set => _soundName = value;
        }
        [SoundSelector]
        [SerializeField] private string _soundName = "NONE";
        public void PlaySound() {
            SoundManager.PlaySoundAt3D(_soundName, transform.position);
        }
        public void PlaySoundAtTransform() {
            SoundManager.PlaySoundAtTransform(_soundName, this.transform);
        }
        public void PlaySoundAt2D() {
            SoundManager.PlaySoundAt2D(_soundName);
        }
        public void PlaySoundByName(string soundName) {
            SoundManager.PlaySoundAt3D(soundName, transform.position);
        }
        public void PlaySoundByNameAnim(AnimationEvent animationEvent) {
            if (animationEvent.animatorClipInfo.weight < 0.1f) return;
            SoundManager.PlaySoundAt3D(animationEvent.stringParameter, transform.position);
        }
    }
}