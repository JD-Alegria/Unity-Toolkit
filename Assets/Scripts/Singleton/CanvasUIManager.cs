using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages text, images, panels on the main Screen Space - Overlay Canvas
/// Ideally each text or element has their update method
/// Values for text fields can come from other managers, and therefore referenced by type
/// </summary>
public class CanvasUIManager : MonoBehaviour
{
    public static CanvasUIManager Instance { get; private set; }

    [Header("Canvas Text Field References")]
    [SerializeField] TMP_Text textField1;
    [SerializeField] TMP_Text textField2;
    [SerializeField] TMP_Text textField3;
    
    void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(gameObject);
        else Instance = this;
    }

    void Update()
    {
        UpdateText1();
    }

    void UpdateText1()
    {
        textField1.text = $"Sample text";
    }
    
    void UpdateText2()
    {
        textField2.text = $"Sample text";
    }
    
    void UpdateText3()
    {
        textField3.text = $"Sample text";
    }
}
