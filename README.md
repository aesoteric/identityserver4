# IdentityServer4

```bash
az login

az group create --name rg-identityserver4-dev --location southafricanorth

az group create --name rg-identityserver4-prod --location southafricanorth

az acr create --name acridentityserver4dev --resource-group rg-identityserver4-dev --location southafricanorth --sku Standard

az acr create --name acridentityserver4prod --resource-group rg-identityserver4-prod --location southafricanorth --sku Standard
```
