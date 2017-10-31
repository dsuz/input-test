using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEditor;

public class InputDetection : MonoBehaviour {
    List<string> m_inputList = new List<string>();
    System.Text.StringBuilder m_builder = new System.Text.StringBuilder();
    public Text m_messageText;

	// Use this for initialization
	void Start () {
        ReadAxes(m_inputList);
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_builder.Remove(0, m_builder.Length);

        // CrossPlatformInputManager で入力を検出する
        foreach(var input in m_inputList)
        {
            if (CrossPlatformInputManager.GetButton(input))
                m_builder.AppendLine(input + " detected.");
        }

        // Input クラスで入力を検出する
        for (int i = 0; i <= 256; i++)
        {
            KeyCode keyCode = (KeyCode)System.Enum.ToObject(typeof(KeyCode), i);
            if (Input.GetKey(keyCode))
                m_builder.AppendLine(keyCode.ToString() + " detected.");
        }

        if (m_messageText) m_messageText.text = m_builder.ToString();
	}

    void ReadAxes(List<string> axesNameList)
    {
        axesNameList.Clear();
        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
        SerializedObject so = new SerializedObject(inputManager);
        SerializedProperty spArray = so.FindProperty("m_Axes");
        if (spArray.arraySize == 0) return;
        for (int i = 0; i < spArray.arraySize; i++)
        {
            var axis = spArray.GetArrayElementAtIndex(i);
            var name = axis.FindPropertyRelative("m_Name").stringValue;
            var value = axis.FindPropertyRelative("axis").intValue;
            var type = axis.FindPropertyRelative("type").intValue;
            axesNameList.Add(name);
        }
    }
}
