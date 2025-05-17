using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Core.Scripts
{
    public class CubeSpawner : MonoBehaviour
    {
        public static CubeSpawner Instance { get; private set; }
        
        [SerializeField] private int cubesQueueCapacity = 1;
        [SerializeField] private bool autoQueueGrow = true;
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private Color[] cubeColors;

        [HideInInspector] public int maxCubeNumber = 4096;
        
        private Queue<Cube> _cubesQueue = new();
        private int _maxPower = 12;
        private Vector3 _defaultSpawnPosition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            _defaultSpawnPosition = transform.position;
            maxCubeNumber = (int)Mathf.Pow(2, _maxPower);

            InitializeCubesQueue();
        }

        private void InitializeCubesQueue()
        {
            for (int i = 0; i < cubesQueueCapacity; i++)
                AddCubeToQueue();
        }

        private void AddCubeToQueue()
        {
            Cube cube = Instantiate(cubePrefab, _defaultSpawnPosition, Quaternion.identity, transform)
                .GetComponent<Cube>();
            cube.gameObject.SetActive(false);
            cube.isMainCube = false;
            _cubesQueue.Enqueue(cube);
        }

        public Cube SpawnCube(int number, Vector3 pos)
        {
            if (_cubesQueue.Count <= 0)
            {
                if (autoQueueGrow)
                {
                    cubesQueueCapacity++;
                    AddCubeToQueue();
                }
                else
                {
                    Debug.LogError("[Cube Queue] - No more cubes available in the pool!");
                    return null;
                }
            }

            Cube cube = _cubesQueue.Dequeue();
            cube.transform.position = pos;
            cube.SetNumber(number);
            cube.SetColor(GetColor(number));
            cube.gameObject.SetActive(true);
            return cube;
        }

        public Cube SpawnRandomCube()
        {
            return SpawnCube(GenerateRandomNumber(), _defaultSpawnPosition);
        }

        public void DestroyCube(Cube cube)
        {
            cube.cubeRigidbody.velocity = Vector3.zero;
            cube.cubeRigidbody.angularVelocity = Vector3.zero;
            cube.transform.rotation = Quaternion.identity;
            cube.isMainCube = false;
            cube.gameObject.SetActive(false);
            _cubesQueue.Enqueue(cube);
        }

        public int GenerateRandomNumber()
        {
            return (int)Mathf.Pow(2, UnityEngine.Random.Range(1, 6));
        }

        private Color GetColor(int number)
        {
            return cubeColors[(int)(Mathf.Log(number) / Mathf.Log(2)) - 1];
        }
    }
}