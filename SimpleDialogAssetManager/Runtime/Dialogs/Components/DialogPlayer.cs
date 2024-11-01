using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;
using UnityEngine.Serialization;
#if HAS_NEW_INPUT
    using UnityEngine.InputSystem;
#endif



namespace DialogSystem.Dialogs.Components
{
    public class DialogPlayer : MonoBehaviour
    {
        #if HAS_NEW_INPUT
            [SerializeField] private List<Key> _playKeyboardKey = new List<Key>();
        #else
            [SerializeField] private List<KeyCode> _playKeyboardKey = new List<KeyCode>();
        #endif
        
        private void Update()
        {
            #if HAS_NEW_INPUT
                foreach (var key in _playKeyboardKey) {
                    if (Keyboard.current[key].wasPressedThisFrame) {
                        RequestDialog();
                    }
                    
                }
            #else
                foreach (var keyCode in _playKeyboardKey) {
                    if (Input.GetKeyDown(keyCode)) {
                        RequestDialog();
                    }
                }
            #endif
        }
        public void RequestDialog()
        {
            DialogManager.Instance.Play();
        }
    }
}