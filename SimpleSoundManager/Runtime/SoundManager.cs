using System.Collections;
using System.Collections.Generic;
using Postive.SimpleSoundAssetManager.Runtime;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Postive.SimpleSoundAssetManager.Runtime
{
    public sealed class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance {
            get {
                if (_instance) return _instance;
                _instance = FindFirstObjectByType<SoundManager>();
                if (_instance) return _instance;
                var go = new GameObject("SoundManager");
                _instance = go.AddComponent<SoundManager>();
                return _instance;
            }
        }
        private static SoundManager _instance = null;
        private AudioListener CurrentAudioListener
        {
            get {
                if (_audioListener == null) {
                    _audioListener = FindFirstObjectByType<AudioListener>();
                }
                return _audioListener;
            }
        }
        [SerializeField] private bool _useDebug = false;
        private Queue<GameObject> _pooledAudioSources = new Queue<GameObject>();
        private AudioListener _audioListener = null;
        //private List<Timer> _timers = new List<Timer>();
        public static void PlaySoundAt3D(string key, Vector3 position)
        {
            if (key.Equals("NONE")) return;
            var sound = SoundDB.Instance.Get(key);
            if (sound == null) {
                if (Instance._useDebug) {
                    Debug.LogError($"Sound with name {key} not found");
                }
                return;
            }
            //attach audio source to position and play sound
            var audioSource = Instance.GetAudioSource();
            audioSource.transform.parent = null;
            audioSource.transform.position = position;
            audioSource.name = $"Audio Clip : {key}";
            audioSource.clip = sound.Clip;
            audioSource.volume = sound.Volume;
            audioSource.pitch = sound.Pitch;
            audioSource.outputAudioMixerGroup = sound.Mixer;
            audioSource.Play();
            if (Instance._useDebug) {
                Debug.Log($"Playing sound {key} at {position}");
            }
            //remove after sound is played
            Instance.DisableAudioSource(audioSource.gameObject, sound.Clip.length);
        }
        public static void PlaySoundAt2D(string key)
        {
            if (key.Equals("NONE")) return;
            var sound = SoundDB.Instance.Get(key);
            if (sound == null) {
                if (Instance._useDebug) {
                    Debug.LogError($"Sound with name {key} not found");
                }
                return;
            }
            //attach audio source to position and play sound
            var audioSource = Instance.GetAudioSource();
            //get audio listener position
            audioSource.transform.parent = Instance.CurrentAudioListener.transform;
            audioSource.transform.position = Vector3.zero;
            audioSource.name = $"Audio Clip : {sound.Name}";
            audioSource.clip = sound.Clip;
            audioSource.volume = sound.Volume;
            audioSource.pitch = sound.Pitch;
            audioSource.maxDistance = 3f;
            audioSource.minDistance = 1f;
            audioSource.outputAudioMixerGroup = sound.Mixer;
            audioSource.Play();
            if (Instance._useDebug) {
                Debug.Log($"Playing sound {key} at 2D");
            }
            //remove after sound is played
            Instance.DisableAudioSource(audioSource.gameObject, sound.Clip.length);
        }
        public static void PlaySoundAtTransform(string key, Transform transform)
        {
            if (key.Equals("NONE")) return;
            var sound = SoundDB.Instance.Get(key);
            if (sound == null) {
                if (Instance._useDebug) {
                    Debug.LogError($"Sound with name {key} not found");
                }
                return;
            }

            //attach audio source to transform and play sound
            var audioSource = Instance.GetAudioSource();
            audioSource.transform.parent = transform;
            audioSource.transform.localPosition = Vector3.zero;
            audioSource.name = $"Audio Clip : {sound.Name}";
            audioSource.clip = sound.Clip;
            audioSource.volume = sound.Volume;
            audioSource.pitch = sound.Pitch;
            audioSource.outputAudioMixerGroup = sound.Mixer;
            audioSource.Play();
            if (Instance._useDebug) {
                Debug.Log($"Playing sound {key} at {transform.name}");
            }
            //remove after sound is played
            Instance.DisableAudioSource(audioSource.gameObject, sound.Clip.length);
        }
        
        private AudioSource GetAudioSource() {
            if (_pooledAudioSources.Count == 0) {
                return CreateAudioSource();
            }
            GameObject go = _pooledAudioSources.Dequeue();
            go.SetActive(true);
            return go.GetComponent<AudioSource>();
        }
        private AudioSource CreateAudioSource() {
            var go = new GameObject();
            AudioSource audioSource = go.AddComponent<AudioSource>();
            return audioSource;
        }
        
        private void DisableAudioSource(GameObject audioSource, float delay) {
            StartCoroutine(DisableAudioSourceCoroutine(audioSource, delay));
        }
        private IEnumerator DisableAudioSourceCoroutine(GameObject audioSource, float delay) {
            yield return new WaitForSeconds(delay);
            audioSource.SetActive(false);
            _pooledAudioSources.Enqueue(audioSource);
        }
        private void OnDestroy() {
            StopAllCoroutines();
            foreach (var audioSource in _pooledAudioSources) {
                Destroy(audioSource);
            }
        }
    }
}