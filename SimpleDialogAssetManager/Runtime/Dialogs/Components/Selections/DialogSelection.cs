using System;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DialogSystem.Runtime.Structure.ScriptableObjects.Components.Selections
{
    //Add Button component when creating a new DialogSelection
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class DialogSelection : MonoBehaviour
    {
        int SelectionIndex {
            get => _selectionIndex;
            set => _selectionIndex = value;
        }
        [Header("Init Events")]
        [SerializeField] private UnityEvent<int> _onInitSelectionIndex;
        [SerializeField] private UnityEvent<string> _onInitSelectionContent;
        [Header("Show/Hide Events")]
        [SerializeField] private UnityEvent _onShowSelectionEvent;
        [SerializeField] private UnityEvent _onHideSelectionEvent;
        private DialogSelector _parentSelector;
        private int _selectionIndex = 0;
        private DialogSelector _parentSelector1;
        private void Start()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                _parentSelector.Select(_selectionIndex);
            });
        }
        /// <summary>
        /// Init the selection
        /// </summary>
        /// <param name="index">Index of the selection</param>
        /// <param name="content">Content of the selection, It will pass to the OnInitSelectionContentEvent</param>
        /// <param name="parentSelector">Parent of the selection</param>
        public void Init(int index ,DialogContent content, DialogSelector parentSelector)
        {
            _selectionIndex = index;
            _parentSelector = parentSelector;
            _onInitSelectionIndex?.Invoke(index);
            _onInitSelectionContent?.Invoke(content.Content);
        }
        /// <summary>
        /// Show the selection
        /// If there is no show event, then activate the gameobject
        /// But if there is a show event, invoke the event
        /// </summary>
        public void Show() {
            //If there is no show event, then activate the gameobject
            if (_onShowSelectionEvent.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(true);
                return;
            }
            _onShowSelectionEvent?.Invoke();
        }
        /// <summary>
        /// Hide the selection
        /// If there is no hide event, then deactivate the gameobject
        /// But if there is a hide event, invoke the event
        /// </summary>
        public void Hide()
        {
            //If there is no hide event, then deactivate the gameobject
            if (_onHideSelectionEvent.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(false);
                return;
            }
            _onHideSelectionEvent?.Invoke();
        }
    }
}