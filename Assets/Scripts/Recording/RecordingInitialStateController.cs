using System.Collections.Generic;
using System.Linq;
using CodingTest.Buttons;
using CodingTest.Data;
using CodingTest.Popup;
using CodingTest.Utility;
using UnityEngine;

namespace CodingTest.Controllers
{
    public class RecordingInitialStateController : MonoBehaviour
    {
        #region Inspector Variables

        [Header("UI Items to save initial position")] 
        [SerializeField] private List<RectTransform> _uiRectTransforms;
        
        [Header("Items to save gameObject active In Hierarchy ")] 
        [SerializeField] private List<GameObject> _gameObjectsActiveInHierarchy;

        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            ServiceContainer.AddInstance(this);
        }

        #endregion

        #region Public Methods

        public RecordingState InitialState()
        {
            return new RecordingState
            {
                ShapePositions = _uiRectTransforms.Select(x =>
                {
                    var button = x.GetComponent<ButtonFunctionality>();
                    
                    return new ShapeState
                    {
                        Position = button.Position,
                        Color = button.Color
                    };
                }).ToList(),
                
                PopupActiveStates = _gameObjectsActiveInHierarchy.Select(x =>
                    {
                        var popup = x.GetComponent<PopupFunctionality>();
                        return new PopupState { IsActive = popup.IsActive, Text = popup.CurrentText };
                    })
                    .ToList()
            };
        }

        public void LoadInitialState(RecordingState initialState)
        {
            for (var i = 0; i < initialState.ShapePositions.Count; i++)
            {
                var button = _uiRectTransforms[i].GetComponent<ButtonFunctionality>();
                button.SetPosition(initialState.ShapePositions[i].Position);
                button.SetImageColor(initialState.ShapePositions[i].Color);
            }

            for (var i = 0; i < initialState.PopupActiveStates.Count; i++)
            {
                var popup = _gameObjectsActiveInHierarchy[i].GetComponent<PopupFunctionality>();
                if(initialState.PopupActiveStates[i].IsActive) popup.ShowPopup(initialState.PopupActiveStates[i].Text);
                else popup.HidePopup();
            }
        }
        
        #endregion

        #region Private Methods

        #endregion
    }
}