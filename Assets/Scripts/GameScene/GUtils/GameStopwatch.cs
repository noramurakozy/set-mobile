using System;
using UnityEngine;

namespace GameScene.GUtils
{
    public class GameStopwatch : MonoBehaviour
    {
        public float Value { get; set; }
        private bool IsRunning { get; set; }

        private void Start()
        {
            Value = 0;
            IsRunning = false;
        }

        void Update()
        {
            if(IsRunning)
            {         
                Value += Time.deltaTime;    
            }
        }
        
        public void Stop()
        {
            IsRunning = false;
        }
        
        public void ResetStopwatch()
        {
            IsRunning = false;
            Value = 0;
        }
        
        public void StartStopwatch()
        {
            IsRunning = true;
        }
    }
}