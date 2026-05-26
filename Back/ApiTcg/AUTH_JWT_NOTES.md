# Mise en place du login avec JWT

## Principe

Un JWT est un token que l'API donne a l'utilisateur apres un login reussi.
Le frontend garde ce token et le renvoie ensuite dans le header HTTP :

```http
Authorization: Bearer <token>
```

L'API verifie le token avec la cle secrete configuree dans `appsettings.json`.
Si le token est valide, l'utilisateur est considere comme connecte.

## Etapes ajoutees dans ce projet

### 1. DTO de login

Fichier : `ApiTcg/DTO/User/UserLogin.cs`

Ce DTO represente les donnees envoyees par le frontend :

```json
{
  "email": "test@test.com",
  "password": "Password123"
}
```

### 2. Recherche du user par email

Fichiers :

- `DAL/Interfaces/IUserRepository.cs`
- `DAL/Repositories/UserRepository.cs`

Le login doit d'abord retrouver l'utilisateur en base avec son email.
La methode ajoutee est :

```csharp
Task<User?> GetByEmailAsync(string email);
```

Elle retourne `null` si aucun utilisateur n'existe avec cet email.

### 3. Verification du mot de passe

Fichier : `BLL/Services/UserService.cs`

Au register, le mot de passe est hashe avec BCrypt.
Au login, on ne re-hashe pas le mot de passe manuellement.
On utilise :

```csharp
BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
```

Cette methode compare le mot de passe en clair recu avec le hash stocke en base.

### 4. Generation du JWT

Fichiers :

- `BLL/Interfaces/IJwtService.cs`
- `BLL/Services/JwtService.cs`

Le service JWT cree un token avec quelques claims :

- `NameIdentifier` : l'id du user
- `Name` : le username
- `Email` : l'email

Ces claims permettent plus tard de retrouver l'identite du user connecte depuis l'API.

### 5. Configuration JWT

Fichier : `ApiTcg/appsettings.json`

```json
"Jwt": {
  "Key": "ChangeThisSecretKeyForDevelopmentOnly123456789",
  "Issuer": "ApiTcg",
  "Audience": "ApiTcgUsers",
  "ExpirationMinutes": 60
}
```

La cle doit etre longue et secrete. Pour un vrai projet, il faut la mettre dans les user secrets ou une variable d'environnement, pas directement dans Git.

### 6. Activation dans Program.cs

Fichier : `ApiTcg/Program.cs`

Deux choses sont importantes :

```csharp
builder.Services.AddAuthentication(...).AddJwtBearer(...);
```

Cela configure la maniere de verifier les tokens.

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

`UseAuthentication` lit le token et identifie l'utilisateur.
`UseAuthorization` applique les regles comme `[Authorize]`.

L'ordre est important : authentication avant authorization.

### 7. Endpoint de login

Fichier : `ApiTcg/Controllers/UserController.cs`

Route ajoutee :

```http
POST /apiTcg/User/login
```

Si le login est valide, l'API repond :

```json
{
  "token": "..."
}
```

Si l'email ou le mot de passe est incorrect, l'API repond `401 Unauthorized`.

## Tester rapidement

1. Creer un utilisateur :

```http
POST /apiTcg/User/register
Content-Type: application/json

{
  "username": "bastien",
  "email": "bastien@test.com",
  "passwordHash": "Password123"
}
```

2. Se connecter :

```http
POST /apiTcg/User/login
Content-Type: application/json

{
  "email": "bastien@test.com",
  "password": "Password123"
}
```

3. Utiliser le token sur une route protegee :

```http
Authorization: Bearer <token>
```

Pour proteger une action de controller, ajoute :

```csharp
[Authorize]
```

Il faudra aussi ajouter `using Microsoft.AspNetCore.Authorization;` dans le controller concerne.
