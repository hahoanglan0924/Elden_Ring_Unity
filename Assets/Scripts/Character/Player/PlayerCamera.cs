using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace PH {
    public class PlayerCamera : MonoBehaviour {
        public static PlayerCamera instance;
        public PlayerManager player;
       // public Camera cameraObject;


        private CinemachineVirtualCamera cinemachineVirtualCamera;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject); // Destroy duplicate instances
            }

            cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        public void HandleAllCameraActions() {
            if (player != null) {
                FollowTarget();
            }
            // Other camera actions
        }

        public void FollowTarget() {
           
                cinemachineVirtualCamera.Follow = player.cameraRoot; // Use the Follow property as a setter
                
            
        }
    }
}
