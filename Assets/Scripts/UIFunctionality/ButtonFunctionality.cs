using System.Collections;
using CodingTest.Inputs;
using CodingTest.Managers;
using CodingTest.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CodingTest.Buttons
{
    public class ButtonFunctionality : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Inspector Variables

        [Header("Tooltip")] [TextArea(2, 5)] [SerializeField]
        private string _tooltipText;

        [SerializeField] private GameObject _tooltipGameObject;
        [SerializeField] private TextMeshProUGUI _tooltipTextMeshProUGUI;

        [TextArea(2, 5)] [SerializeField] private string _popupText;
        
        #endregion

        #region Public Variables

        public Vector3 Position => _currentButton.transform.position;
        public Color Color => _currentImage.color;
        
        #endregion

        #region Private Variables

        private UIManager _uiManager;
        private CustomPlayerInputController _playerInputController;

        private const float TIME_TO_SHOW_TOOLTIP = 0.5f;
        private bool _canShowTooltip;

        private RectTransform _tooltipRectTransform;
        private Image _currentImage;
        private Button _currentButton;
        private Rigidbody2D _currentRigidbody2D;
        private CircleCollider2D _currentCollider;

        private Vector2 _offset;
        private bool _isInsideTheButton;
        private bool _wasClicked;
        private int _colorIndex = 0;
        
        #endregion

        #region Unity Methods

        private void Start()
        {
            ConfigureConnections();
            ConfigureDelegates();
            ConfigureColor();
            ConfigureCollisions();
            ConfigureTooltipGameObject();
            
            _currentButton = GetComponent<Button>(); 
        }

        private void OnDestroy()
        {
            RemoveDelegates();
        }
        
        #endregion

        #region Public Methods

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isInsideTheButton = true;
            _canShowTooltip = true;
            StartCoroutine(ShowTooltipCoroutine());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isInsideTheButton = false;
            _canShowTooltip = false;
            HideTooltip();
        }

        public void SetPosition(Vector3 position)
        {
            _currentButton.transform.position = position;
        }
        
        public void SetImageColor(Color color)
        {
            _currentImage.color = color;
        }
        
        #endregion

        #region Private Methods

        #region Tooltip

        private IEnumerator ShowTooltipCoroutine()
        {
            var timer = TIME_TO_SHOW_TOOLTIP;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                if (!_canShowTooltip) yield break;
                yield return null;
            }

            if (_canShowTooltip) ShowTooltip();
        }

        private void ShowTooltip()
        {
            _tooltipTextMeshProUGUI.text = _tooltipText;
            _tooltipRectTransform.AnimateRectTransformScale(Vector3.zero, Vector3.one, 0.15f, Ease.OutQuint, delegate { _tooltipGameObject.SetActive(true); });
        }

        private void HideTooltip()
        {
            if (_tooltipGameObject.activeInHierarchy)
                _tooltipRectTransform.AnimateRectTransformScale(Vector3.one, Vector3.zero, 0.15f, Ease.OutQuint, onCompleteAction: delegate { _tooltipGameObject.SetActive(false); });
        }
        
        private void ConfigureTooltipGameObject()
        {
            _tooltipGameObject.SetActive(false);
            _tooltipRectTransform = _tooltipGameObject.GetComponent<RectTransform>();
        }

        #endregion

        #region Inputs

        private void OnRightClickDownUpdate()
        {
            if (!_isInsideTheButton) return;
            ChangeImageColor();
        }

        private void OnPointUpdate(Vector2 input)
        {
            if(!(_isInsideTheButton && _wasClicked)) return;
            SetPosition(input + _offset);
        }

        private void OnLeftClickDownUpdate()
        {
            if(!_isInsideTheButton) return;
            OnClickUpdate();
            _wasClicked = true;
            _currentRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            _offset = _currentButton.transform.position - new Vector3(Mouse.current.position.value.x, Mouse.current.position.value.y);
        }

        private void OnLeftClickUpUpdate()
        {
            _wasClicked = false;
            _currentRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnClickUpdate()
        {
            _uiManager.ShowPopup(_popupText.Replace("#color", _uiManager.GetColorName(_currentImage.color)));
        }

        #endregion

        private void ChangeImageColor()
        {
            _colorIndex++;
            _colorIndex %= _uiManager.ButtonColorsList.Count;
            _currentImage.color = _uiManager.ButtonColorsList[_colorIndex];
        }
        
        #region Configuration

        private void ConfigureCollisions()
        {
            _currentRigidbody2D = GetComponent<Rigidbody2D>();
            _currentCollider = GetComponent<CircleCollider2D>();
        }

        private void ConfigureColor()
        {
            _currentImage = GetComponent<Image>();
            _currentImage.color = _uiManager.ButtonColorsList[_colorIndex];
        }
        
        private void ConfigureConnections()
        {
            _uiManager = ServiceContainer.GetInstance<UIManager>();
            _playerInputController = ServiceContainer.GetInstance<CustomPlayerInputController>();
        }

        private void ConfigureDelegates()
        {
            _playerInputController.OnRightClickDownUpdate += OnRightClickDownUpdate;
            _playerInputController.OnPointerUpdate += OnPointUpdate;
            _playerInputController.OnLeftClickDownUpdate += OnLeftClickDownUpdate;
            _playerInputController.OnLeftClickUpUpdate += OnLeftClickUpUpdate;
        }

        private void RemoveDelegates()
        {
            _playerInputController.OnRightClickDownUpdate -= OnRightClickDownUpdate;
            _playerInputController.OnPointerUpdate -= OnPointUpdate;
            _playerInputController.OnLeftClickDownUpdate -= OnLeftClickDownUpdate;
            _playerInputController.OnLeftClickUpUpdate -= OnLeftClickUpUpdate;
        }

        #endregion

        #endregion
        
    }
}