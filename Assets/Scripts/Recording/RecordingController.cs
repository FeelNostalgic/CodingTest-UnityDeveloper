using System.Collections;
using System.Threading.Tasks;
using CodingTest.Data;
using CodingTest.Inputs;
using CodingTest.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace CodingTest.Controllers
{
    public class RecordingController
    {
        public RecordingController(SaveController saveController, Button buttonStartRecording, TMP_InputField inputFieldName, Button buttonLoadRecording)
        {
            _saveController = saveController;
            _buttonStartRecording = buttonStartRecording;
            _inputFieldName = inputFieldName;
            _buttonLoadRecording = buttonLoadRecording;
            _playerInputController = ServiceContainer.GetInstance<CustomPlayerInputController>();
            _recordingInitialStateController = ServiceContainer.GetInstance<RecordingInitialStateController>();
            
            ConfigureDelegates();
        }

        #region Public Variables

        public bool IsRecording { get; set; }

        #endregion

        #region Private Variables

        private SaveController _saveController;
        private CustomPlayerInputController _playerInputController;
        private RecordingInitialStateController _recordingInitialStateController;
        
        private Button _buttonStartRecording;
        private TMP_InputField _inputFieldName;
        private Button _buttonLoadRecording;
        
        private InputList _currentInputData;
        
        #endregion

        #region Public Methods

        public IEnumerator RecordingCoroutine(string recordingName)
        {
            _inputFieldName.interactable = false;
            _buttonLoadRecording.interactable = false;
            _currentInputData = new InputList
            {
                InitialState = _recordingInitialStateController.InitialState()
            };
            _currentInputData.InputDataList.Add(new InputData(){InputTime = Time.time});

            while (IsRecording)
            {
                yield return null;
            }
            
            SaveRecording(recordingName);
        }

        public IEnumerator PlayRecordingCoroutine(InputList inputsToPlay)
        {
            //Load starting time
            var currentData = inputsToPlay.InputDataList[0];
            var currentTime = currentData.InputTime;
            inputsToPlay.InputDataList.RemoveAt(0);
            
            //Load Initial State
            _recordingInitialStateController.LoadInitialState(inputsToPlay.InitialState);
            
            //Load first action
            currentData = inputsToPlay.InputDataList[0];
            var pos = 0;

            //interactable false when playing
            _inputFieldName.interactable = false;
            _buttonLoadRecording.interactable = false;
            _buttonStartRecording.interactable = false;
            //InputSystem.DisableDevice(Mouse.current);

            while (pos < inputsToPlay.InputDataList.Count)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= currentData.InputTime)
                {
                    //Debug.Log($"Position{pos} Time: {currentData.InputTime}, Left {currentData.LeftClick}, Right {currentData.RightClick} MousePosition {currentData.MousePosition.x} {currentData.MousePosition.y}");
                    //Load mouse position
                    _playerInputController.OnPointerUpdate.Invoke(new Vector2(currentData.MousePosition.x, currentData.MousePosition.y));
                    Mouse.current.WarpCursorPosition(new Vector2(currentData.MousePosition.x, currentData.MousePosition.y));
                    
                    //Load Left Click
                    if(currentData.LeftClick) _playerInputController.OnLeftClickDownUpdate.Invoke();
                    else _playerInputController.OnLeftClickUpUpdate.Invoke();
                    
                    //Load Right Click
                    if(currentData.RightClick) _playerInputController.OnRightClickDownUpdate.Invoke();
                    else _playerInputController.OnRightClickUpUpdate.Invoke();
                    
                    //Continue to next input
                    pos++;
                    if (pos >= inputsToPlay.InputDataList.Count) continue;
                    currentData = inputsToPlay.InputDataList[pos];
                }

                yield return null;
            }
            
            _buttonLoadRecording.interactable = true;
            _buttonStartRecording.interactable = true;
            _inputFieldName.interactable = true;
            //InputSystem.EnableDevice(Mouse.current);
        }
        
        public void RemoveDelegates()
        {
            _playerInputController.OnRightClickDownUpdate -= OnRightClickDownUpdate;
            _playerInputController.OnRightClickUpUpdate -= OnRightClickUpUpdate;
            _playerInputController.OnPointerUpdate -= OnPointUpdate;
            _playerInputController.OnLeftClickDownUpdate -= OnLeftClickDownUpdate;
            _playerInputController.OnLeftClickUpUpdate -= OnLeftClickUpUpdate;
        }
        
        #endregion

        #region Private Methods

        private void ConfigureDelegates()
        {
            _playerInputController.OnRightClickDownUpdate += OnRightClickDownUpdate;
            _playerInputController.OnRightClickUpUpdate += OnRightClickUpUpdate;
            _playerInputController.OnPointerUpdate += OnPointUpdate;
            _playerInputController.OnLeftClickDownUpdate += OnLeftClickDownUpdate;
            _playerInputController.OnLeftClickUpUpdate += OnLeftClickUpUpdate;
        }

        private void SaveRecording(string name)
        {
            _saveController.SaveData(_currentInputData, name);
            _inputFieldName.interactable = true;
            _buttonLoadRecording.interactable = true;
        }
        
        private void OnRightClickDownUpdate()
        {
            if (!IsRecording) return;
            _currentInputData.InputDataList.Add(new InputData() { InputTime = Time.time, RightClick = true, MousePosition = Mouse.current.position.value });
            WatchHoldRightClick();
        }
        
        private void OnRightClickUpUpdate()
        {
            if (!IsRecording) return;
            _currentInputData.InputDataList.Add(new InputData() { InputTime = Time.time, RightClick = false, MousePosition = Mouse.current.position.value });
        }

        private void OnPointUpdate(Vector2 input)
        {
            if (!IsRecording) return;
            _currentInputData.InputDataList.Add(new InputData() { InputTime = Time.time, MousePosition = input });
        }

        private void OnLeftClickDownUpdate()
        {
            if (!IsRecording) return;
            _currentInputData.InputDataList.Add(new InputData() { InputTime = Time.time, LeftClick = true, MousePosition = Mouse.current.position.value });
            WatchHoldLeftClick();
        }
        
        private void OnLeftClickUpUpdate()
        {
            if (!IsRecording) return;
            _currentInputData.InputDataList.Add(new InputData() { InputTime = Time.time, LeftClick = false, MousePosition = Mouse.current.position.value });
        }

        private async void WatchHoldLeftClick()
        {
            while (Mouse.current.leftButton.isPressed)
            {
                _currentInputData.InputDataList.Add(new InputData() { InputTime = Time.time, LeftClick = true, MousePosition = Mouse.current.position.value });
                await Task.Yield();
            }
        }
        
        private async void WatchHoldRightClick()
        {
            while (Mouse.current.rightButton.isPressed)
            {
                _currentInputData.InputDataList.Add(new InputData() { InputTime = Time.time, RightClick = true, MousePosition = Mouse.current.position.value });
                await Task.Yield();
            }
        }

        #endregion
    }
}