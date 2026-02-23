const faceAttendance = {
      video: null,
      canvas: null,
      ctx: null,
      faceMesh: null,
      faceApiLoaded: false,
      mediaPipeLoaded: false,
      isPaused: false,

      // Liveness States: 'NONE', 'FACE_DETECTED', 'NEUTRAL', 'ACTION_DONE'
      livenessState: 'NONE',
      stateTimestamp: 0,

      getDetectorOptions: function (inputSize = 160) {
            return new faceapi.TinyFaceDetectorOptions({ inputSize, scoreThreshold: 0.5 });
      },

      init: async function (videoElementId, canvasElementId) {
            this.video = document.getElementById(videoElementId);
            this.canvas = document.getElementById(canvasElementId);
            this.ctx = this.canvas.getContext('2d');
            this.resetLiveness();

            console.log("Loading AI Models (Face Mesh + Face API)...");

            await Promise.all([
                  faceapi.nets.tinyFaceDetector.loadFromUri('/models/face-api'),
                  faceapi.nets.faceLandmark68Net.loadFromUri('/models/face-api'),
                  faceapi.nets.faceRecognitionNet.loadFromUri('/models/face-api')
            ]);
            this.faceApiLoaded = true;

            this.faceMesh = new FaceMesh({
                  locateFile: (file) => {
                        return `https://cdn.jsdelivr.net/npm/@mediapipe/face_mesh/${file}`;
                  }
            });

            this.faceMesh.setOptions({
                  maxNumFaces: 1,
                  refineLandmarks: true,
                  minDetectionConfidence: 0.5,
                  minTrackingConfidence: 0.5
            });

            this.mediaPipeLoaded = true;
      },

      resetLiveness: function () {
            this.livenessState = 'NONE';
            this.stateTimestamp = Date.now();
      },

      startVideo: function () {
            navigator.mediaDevices.getUserMedia({ video: { width: 640, height: 480 } })
                  .then(stream => {
                        this.video.srcObject = stream;
                  })
                  .catch(err => console.error("Error accessing camera:", err));
      },

      pauseProcessing: function (state) {
            this.isPaused = state;
            if (state) {
                  this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
            }
      },

      // Strict 3D Check: Compare nose tip depth vs eye-line depth
      calculateDepthScore: function (multiFaceLandmarks) {
            if (!multiFaceLandmarks || multiFaceLandmarks.length === 0) return 0;

            const landmarks = multiFaceLandmarks[0];
            const noseTip = landmarks[1];
            const leftEye = landmarks[33];
            const rightEye = landmarks[263];

            // In MediaPipe, Z is negative for points closer to camera
            // A real face: Nose tip Z should be significantly more negative than eyes
            const eyeAvgZ = (leftEye.z + rightEye.z) / 2;
            const depthDiff = eyeAvgZ - noseTip.z;

            return depthDiff; // Higher is more "3D-ish"
      },

      // Dynamic Liveness State Machine
      updateLiveness: function (landmarks, multiFaceLandmarks) {
            const now = Date.now();
            const depthScore = this.calculateDepthScore(multiFaceLandmarks);
            const is3D = depthScore > 0.04; // Adjust based on calibration

            // 1. Initial detection
            if (this.livenessState === 'NONE') {
                  this.livenessState = 'FACE_DETECTED';
                  this.stateTimestamp = now;
                  return { state: 'FACE_DETECTED', msg: 'Đã thấy gương mặt.' };
            }

            // 2. WAITING FOR NEUTRAL (Reset if we stay in smile too long at start)
            if (this.livenessState === 'FACE_DETECTED') {
                  if (this.isSmiling(landmarks)) {
                        return { state: 'WAITING', msg: 'Vui lòng giữ mặt bình thường.' };
                  }
                  this.livenessState = 'NEUTRAL';
                  this.stateTimestamp = now;
                  return { state: 'NEUTRAL', msg: 'Tốt! Bây giờ hãy MỈM CƯỜI.' };
            }

            // 3. WAITING FOR SMILE (Dynamic action)
            if (this.livenessState === 'NEUTRAL') {
                  if (now - this.stateTimestamp > 5000) {
                        this.resetLiveness();
                        return { state: 'TIMEOUT', msg: 'Hết thời gian. Thử lại.' };
                  }

                  if (this.isSmiling(landmarks)) {
                        if (is3D) {
                              this.livenessState = 'VERIFIED';
                              return { state: 'VERIFIED', msg: 'Xác thực người thật thành công!' };
                        } else {
                              return { state: 'SPOOF', msg: 'Phát hiện ảnh chụp 2D!' };
                        }
                  }
                  return { state: 'NEUTRAL', msg: 'Đang đợi nụ cười...' };
            }

            return { state: this.livenessState, msg: '' };
      },

      isSmiling: function (landmarks) {
            const leftCorner = landmarks[61];
            const rightCorner = landmarks[291];
            const topLip = landmarks[13];
            const bottomLip = landmarks[14];

            const mouthWidth = Math.sqrt(Math.pow(leftCorner.x - rightCorner.x, 2) + Math.pow(leftCorner.y - rightCorner.y, 2));
            const mouthHeight = Math.abs(topLip.y - bottomLip.y) || 0.001;

            return (mouthWidth / mouthHeight) > 4.5;
      },

      captureDescriptor: async function () {
            const options = this.getDetectorOptions(160);
            const detections = await faceapi.detectSingleFace(this.video, options)
                  .withFaceLandmarks()
                  .withFaceDescriptor();

            if (detections) {
                  return JSON.stringify(Array.from(detections.descriptor));
            }
            return null;
      },

      getFrameBase64: function () {
            const tempCanvas = document.createElement('canvas');
            tempCanvas.width = this.video.videoWidth;
            tempCanvas.height = this.video.videoHeight;
            const ctx = tempCanvas.getContext('2d');
            ctx.drawImage(this.video, 0, 0);
            return tempCanvas.toDataURL('image/jpeg', 0.8);
      }
};
