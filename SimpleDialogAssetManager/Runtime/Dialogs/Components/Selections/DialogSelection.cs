using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Runtime.Structure.ScriptableObjects.Components.Selections
{
    public class DialogSelection : MonoBehaviour
    {
        public event Action<int> OnSelectionSelected;
        int SelectionIndex {
            get => _selectionIndex;
            set => _selectionIndex = value;
        }
        [Header("Init Events")]
        [SerializeField] private UnityEvent<string> _onInitSelectionContent;
        private int _selectionIndex = 0;
        private void Awake()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
                OnSelectionSelected?.Invoke(SelectionIndex);
            });
        }
        /// <summary>
        /// Init the selection
        /// </summary>
        /// <param name="index">Index of the selection</param>
        /// <param name="content">Content of the selection, It will pass to the OnInitSelectionContentEvent</param>
        public void Init(int index ,DialogContent content)
        {
            _selectionIndex = index;
            _onInitSelectionContent?.Invoke(content.Content);
            OnInit(index, content);
            Show();
        }
        public virtual void OnInit(int index, DialogContent content) { }

        /// <summary>
        /// Show the selection
        /// If there is no show event, then activate the gameobject
        /// But if there is a show event, invoke the event
        /// </summary>
        public virtual void Show() {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// Hide the selection
        /// If there is no hide event, then deactivate the gameobject
        /// But if there is a hide event, invoke the event
        /// </summary>
        public virtual void Hide() {
            gameObject.SetActive(false);
        }
    }
}