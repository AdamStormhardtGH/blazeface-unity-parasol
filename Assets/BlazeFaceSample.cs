/* 
*   BlazeFace
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using NatML.VideoKit;
    using NatML.Vision;
    using NatML.Visualizers;

    public sealed class BlazeFaceSample : MonoBehaviour {

        [Header(@"Camera")]
        public VideoKitCameraManager cameraManager;  

        [Header(@"UI")]
        public BlazeFaceVisualizer visualizer;

        private BlazeFacePredictor predictor;

        private bool isListeningToCameraFrames = false;

        private async void Start () {

            // Disable screen dimming for mobile devices
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // Create the RVM predictor
            predictor = await BlazeFacePredictor.Create();
            // Listen for camera frames
            listenToCameraFrames();
        }

        

        private void OnCameraFrame (CameraFrame frame) {
            // Predict
            var faces = predictor.Predict(frame);
            // Visualize
            visualizer.Render(faces);
        }

        private void OnDisable () {
            // Stop listening for camera frames
            cameraManager.OnCameraFrame.RemoveListener(OnCameraFrame);
            // Dispose the predictor
            predictor?.Dispose();
        }

        public void OnApplicationFocus(bool hasFocus) {
            // Listen for camera frames when the app is in focus or moves to background
            if (hasFocus) {
                listenToCameraFrames();
            } else {
                stopListeningToCameraFrames();
            }
        }

        // helper methods
        private void listenToCameraFrames () {
            // Listen for camera frames. Since this is a more common use case, we have a helper method for it.
            if (isListeningToCameraFrames) return;

            cameraManager.OnCameraFrame.AddListener(OnCameraFrame);
            isListeningToCameraFrames = true;
        }

        private void stopListeningToCameraFrames () {
            // Stop listening for camera frames

            if (isListeningToCameraFrames) {
                cameraManager.OnCameraFrame.RemoveListener(OnCameraFrame);
                isListeningToCameraFrames = false;
            }
        }
    }
}