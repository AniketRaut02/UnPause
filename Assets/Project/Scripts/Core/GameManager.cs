using DarkPixelGD.Input;
using UnityEngine;

namespace DarkPixelGD.Core
{


    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance { get; private set; }
        [SerializeField] private InputReader inputReader;
        public InputReader InputReader => inputReader;


        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            inputReader.Initialize();
        }
       

        private void OnApplicationQuit() => inputReader.Terminate();
    }
}