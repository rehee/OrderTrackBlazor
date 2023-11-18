param(
  [Parameter(Mandatory = $false)]
  [bool]$rebuild = $true
)
$image = "otb:0.1"
Write-Output "Cleaning Docker system"
docker system prune --force
Write-Output "Running docker build"

if(($rebuild )){
  docker build `
  -t $image `
  --file ./Dockerfile `
  --rm `
  .
}



Write-Output "Running docker compose up"
docker-compose -f docker-compose.yml up