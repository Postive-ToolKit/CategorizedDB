﻿using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;
using UnityEngine.Audio;

namespace Postive.SimpleSoundAssetManager.Runtime.Data
{
    public class SoundData : CategoryElement
    {
        public AudioClip Clip => _clips[UnityEngine.Random.Range(0, _clips.Length)];
        public int ClipCount {
            get {
                if (_clips == null) return 0;
                return _clips.Length;
            }
        }
        public float Volume {
            get => _useRandomVolume ? UnityEngine.Random.Range(_volumeRange.x, _volumeRange.y) : _volume;
            #if UNITY_EDITOR
            set => _volume = value;
            #endif
        }

        public float Pitch {
            get => _useRandomPitch ? UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y) : _pitch;
            #if UNITY_EDITOR
            set => _pitch = value;
            #endif
        }

        public Vector2 VolumeRange {
            get => _volumeRange;
            #if UNITY_EDITOR
            set => _volumeRange = value;
            #endif
        }
        
        public Vector2 PitchRange {
            get => _pitchRange;
            #if UNITY_EDITOR
            set => _pitchRange = value;
            #endif
        }
        public bool UseRandomVolume {
            get => _useRandomVolume;
        }
        public bool UseRandomPitch {
            get => _useRandomPitch;
        }
        public AudioMixerGroup Mixer => _mixer;
        [SerializeField] private bool _useRandomVolume = false;
        [Range(0f, 2f)]
        [SerializeField] private float _volume = 1f;
        [SerializeField] private Vector2 _volumeRange = new Vector2(0.9f, 1.1f);
        [SerializeField] private bool _useRandomPitch = false;
        [Range(0f, 2f)]
        [SerializeField] private float _pitch = 1f;
        [SerializeField] private Vector2 _pitchRange = new Vector2(0.9f, 1.1f);
        [SerializeField] private AudioMixerGroup _mixer;
        [SerializeField] private AudioClip[] _clips;
    }
}