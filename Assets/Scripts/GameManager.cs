using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private Button BtnHint;
        public Game Game { get; set; }
        
        [SerializeField] private CardView cardPrefab;
        
        private void Awake() 
        { 
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }

        private void Start()
        {
            Game = new Game(cardPrefab);
            Game.StartNewGame();
            
            BtnHint.onClick.AddListener(Game.SelectHint);
        }
    }
}