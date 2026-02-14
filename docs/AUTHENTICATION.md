# AUTHENTICATION
*(Le guide sur la sécurité et la gestion du JWT).*

```markdown
# 🔐 Guide d'Authentification JWT

## 🚀 Fonctionnement global

L'API est sécurisée de manière hybride :
- La route `POST /api/Auth/login` est publique (`[AllowAnonymous]`).
- Toutes les routes `/api/Products` et `/api/Orders` sont strictement privées (`[Authorize]`).

### Format du Token
Le système génère un JSON Web Token signé via HMAC-SHA256, d'une durée de validité de 2 heures. 
Il contient les informations suivantes (Claims) :
- `NameIdentifier` : Le nom d'utilisateur (ex: admin).
- `Role` : Le rôle attribué (ex: Admin).
- `Jti` : Un identifiant unique (Guid) pour éviter le rejeu.

## ⚙️ Configuration (Développement vs Production)

### En Local (Environnement de dev)
Les clés sont stockées temporairement dans `AdvancedDevSample.Api/appsettings.json` :
```json
"Jwt": {
  "Key": "CeciEstUneCleSuperSecreteEtTresLonguePourLeProjetAcademique2026!!!",
  "Issuer": "ProductCatalogApi",
  "Audience": "ProductCatalogClients"
}
```

## 🚀 Comment utiliser l'authentification

### 1. Obtenir un token JWT

Envoyez une requête POST à `/api/auth/login` avec des identifiants valides :

```http
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin"
}
```

**Réponse attendue :**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
}
```

### 2. Utiliser le token dans vos requêtes

Ajoutez l'en-tête `Authorization` à toutes vos requêtes :

```http
GET http://localhost:5000/api/products
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

⚠️ **Important** : N'oubliez pas le préfixe `Bearer` suivi d'un espace avant le token ! mais sur swaggerUI il se fait en automatique inserer juste le token

---
# ⚠️ Bonnes Pratiques (Sécurité)

Dans un vrai environnement de production, la `Key` ne doit **jamais** être commitée dans le code source (risque détecté par SonarQube/SonarCloud).

**Solutions préconisées :**

1. **.NET User Secrets** (En local) : `dotnet user-secrets set "Jwt:Key" "MaCleSecrete!"`
2. **Variables d'environnement** (Serveur) : `export Jwt__Key="MaCleSecrete!"`
3. **Gestionnaires de secrets** (Cloud) : Azure Key Vault ou AWS Secrets Manager.

## ❌ Résolution des Erreurs 401 (Unauthorized)

Si le serveur vous renvoie un `401 Unauthorized`, vérifiez ces points :

1. **Oubli du mot clé :** L'en-tête doit être de la forme `Authorization: Bearer <votre_token>`.
2. **Token expiré :** Regénérez un token via la route de login.
3. **Clé asymétrique :** Assurez-vous que l'API n'a pas redémarré avec une `Key` différente dans les configurations.