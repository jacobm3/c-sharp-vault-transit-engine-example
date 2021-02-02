# C# Vault Transit Example

## Auth Methods

This example supports two different methods for development and "production" scenarios.

### Token

Uitlizing a Vault token requires you to set the method to token and configure the VAULT_AUTH_TOKEN environment variable.

```
VAULT_AUTH_METHOD="token"
VAULT_AUTH_TOKEN="..."
```

### Kubernetes Auth

Integrating this example with a Kubernetes environment requires the K8s cluster to be configured with your Vault cluster. Once that has been accomplished, you'll want to set the auth method to kubernetes and configure the Vault role that this application will use.

```
VAULT_AUTH_METHOD="kubernetes"
VAULT_ROLE="dotnet-app"
```

```
vault write -namespace=admin auth/kubernetes/role/dotnet-app \
        bound_service_account_names=dotnet-app \
        bound_service_account_namespaces=default \
        policies=dotnet-app \
        ttl=24h
```

## Policies

```
vault policy write dotnet-app dotnet-app.hcl
```

## Transit Configuration

```
vault secrets enable transit
```

```
vault write -f transit/keys/test_key
```

## Kube Port Forward

```
kubectl port-forward pod/dotnet-app-deployment-5dc7c95c6f-28wls 5000:80
```


## Test it

```
curl --data '{"PinNumber":"1234", "CardNumber": "1234-1234-1234-1234", "CardExpirationDate":"01/01/2021" }' --header "Content-Type: application/json" --header "encryptionKeyId: test_key" --header "encryptionKeyVersion: 1" http://localhost:5000/pinreset
```