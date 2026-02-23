$models = @(
    "tiny_face_detector_model-weights_manifest.json",
    "tiny_face_detector_model-shard1",
    "face_landmark_68_model-weights_manifest.json",
    "face_landmark_68_model-shard1",
    "face_recognition_model-weights_manifest.json",
    "face_recognition_model-shard1",
    "face_recognition_model-shard2",
    "face_expression_model-weights_manifest.json",
    "face_expression_model-shard1"
)

$baseUrl = "https://raw.githubusercontent.com/justadudewhohacks/face-api.js/master/weights/"
$destPath = "d:\HACKATHON\HRM\wwwroot\models\face-api\"

if (!(Test-Path $destPath)) {
    New-Item -ItemType Directory -Force -Path $destPath
}

foreach ($model in $models) {
    $url = $baseUrl + $model
    $dest = Join-Path $destPath $model
    Write-Host "Downloading $model..."
    Invoke-WebRequest -Uri $url -OutFile $dest
}

Write-Host "All models downloaded successfully!"
