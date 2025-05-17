using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Core.Scripts
{
    public class Cube : MonoBehaviour
    {
        private static int _cubeID = 0;
        private MeshRenderer _meshRenderer;
        [SerializeField] private TMP_Text[] numberText;

        [HideInInspector] public int cubeID;
        [HideInInspector] public Color cubeColor;
        [HideInInspector] public int cubeNumber;
        [HideInInspector] public Rigidbody cubeRigidbody;
        [HideInInspector] public bool isMainCube;
        
        private void Awake()
        {
            cubeID = _cubeID++;
            _meshRenderer = GetComponent<MeshRenderer>();
            cubeRigidbody = GetComponent<Rigidbody>();
        }

        public void SetColor(Color color)
        {
            cubeColor = color;
            _meshRenderer.material.color = cubeColor;
        }

        public void SetNumber(int number)
        {
            cubeNumber = number;
            for (int i = 0; i < numberText.Length; i++)
            {
                numberText[i].text = cubeNumber.ToString();
            }
        }
    }
}