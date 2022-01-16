param (
	[string] $configuration
)

# Services
kubectl apply -k ../../../Services/IdeaMachineWeb/Kubernetes/overlays/${configuration}