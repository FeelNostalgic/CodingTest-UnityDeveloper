using CodingTest.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodingTest.Popup
{
    public class PopupFunctionality : MonoBehaviour
    {
        #region Inspector Variables
        
        [SerializeField] private TextMeshProUGUI _popupTextMeshProUGUI;
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;
        
        #endregion

        #region Public Variables

        public bool IsActive => gameObject.activeInHierarchy;
        public string CurrentText => _popupTextMeshProUGUI.text;
        
        #endregion

        #region Private Variables

        private RectTransform _popupRectTransform;
        
        #endregion

        #region Unity Methods

        private void Start()
        {
            gameObject.SetActive(false);
            _popupRectTransform = GetComponent<RectTransform>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show popup gameobject with text
        /// </summary>
        public void ShowPopup(string text)
        {
            _popupTextMeshProUGUI.text = text;
            if (gameObject.activeInHierarchy) return;
            _popupRectTransform.AnimateRectTransformPosition(_startPosition, _endPosition, 0.15f, Ease.OutQuint, onPlayAction: delegate { gameObject.SetActive(true); });
        }

        /// <summary>
        /// Hide current popup gameobject
        /// </summary>
        public void HidePopup()
        {
            _popupRectTransform.AnimateRectTransformPosition(_endPosition, _startPosition, 0.15f, Ease.OutQuint, onCompleteAction: delegate { gameObject.SetActive(false); });
        }
        
        #endregion

        #region Private Methods

        #endregion
    }
}