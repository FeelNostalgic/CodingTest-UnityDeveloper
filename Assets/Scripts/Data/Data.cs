using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodingTest.Data
{
    [Serializable]
    public class InputData
    {
        public float InputTime;
        public bool LeftClick;
        public bool RightClick;
        public Vector2 MousePosition;
    }
    
    [Serializable]
    public class InputList
    {
        public List<InputData> InputDataList = new();
        public RecordingState InitialState;
    }

    [Serializable]
    public struct RecordingState
    {
        public List<ShapeState> ShapePositions;
        public List<PopupState> PopupActiveStates;
    }

    [Serializable]
    public struct ShapeState
    {
        public Vector3 Position;
        public Color Color;
    }

    [Serializable]
    public struct PopupState
    {
        public bool IsActive;
        public string Text;
    }
}