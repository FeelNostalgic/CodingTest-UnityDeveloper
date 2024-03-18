using System.Collections.Generic;
using System.Runtime.InteropServices;
using CodingTest.Controllers;
using CodingTest.Data;
using CodingTest.Popup;
using CodingTest.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodingTest.Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Inspector Variables

        [Header("Popup")]
        [SerializeField] private PopupFunctionality _popup;

        [Header("Button Colors")] [SerializeField]
        private List<Color> _buttonColorsList;

        [Header("Recording")] 
        [SerializeField] private TextMeshProUGUI _textButtonStartRecording;
        [SerializeField] private Button _buttonStartRecording;
        [SerializeField] private TMP_InputField _inputFieldName;
        [SerializeField] private Button _buttonLoadRecording;
        
        #endregion

        #region Public Variables

        public List<Color> ButtonColorsList => _buttonColorsList;

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(float X, float Y);
        
        #endregion

        #region Private Variables

        private SaveController _saveController;
        private RecordingController _recordingController;
        
        private RectTransform _popupRectTransform;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            ServiceContainer.AddInstance(this);
        }

        private void Start()
        {
            ConfigureConnections();
            
            _textButtonStartRecording.SetText("Start recording");
        }
        
        private void OnDestroy()
        {
            _recordingController.RemoveDelegates();
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Compare color to get name
        /// </summary>
        public string GetColorName(Color c)
        {
            if (c.CompareRGB(_buttonColorsList[0]))
            {
                return "red";
            }

            if (c.CompareRGB(_buttonColorsList[1]))
            {
                return "green";
            }

            if (c.CompareRGB(_buttonColorsList[2]))
            {
                return "blue";
            }

            return "";
        }

        #region Recording

        public void StartStopRecording()
        {
            var recordingName = _inputFieldName.text;
            if (recordingName.Equals(""))
            {
                ShowPopup("Recording name can not be empty");
                return;
            }

            _textButtonStartRecording.SetText(_recordingController.IsRecording ? "Start recording" : "Stop recording");
            _recordingController.IsRecording = !_recordingController.IsRecording;

            //if _isRecording == true => Start recording
            //if _isRecording == false => Save recording
            if (_recordingController.IsRecording) StartCoroutine(_recordingController.RecordingCoroutine(recordingName));
        }

        public void LoadPlayRecording()
        {
            var recordingName = _inputFieldName.text;
            if (recordingName.Equals(""))
            {
                ShowPopup("Recording name can not be empty");
                return;
            }

            if (!_saveController.ExitFileWithName(recordingName))
            {
                ShowPopup("Recording name does not exist");
                return;
            }

            var currentInputList = _saveController.LoadData(recordingName);
            StartCoroutine(_recordingController.PlayRecordingCoroutine(currentInputList));
        }

        public void ShowPopup(string message)
        {
            _popup.ShowPopup(message);
        }
        
        #endregion

        #endregion

        #region Private Methods
        
        private void ConfigureConnections()
        {
            _saveController = ServiceContainer.GetInstance<SaveController>();
            _recordingController = new RecordingController(_saveController, _buttonStartRecording, _inputFieldName, _buttonLoadRecording);
        }
        
        #endregion
    }
}