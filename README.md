# Azure App Service : application .NET de démonstration

Petite application web **ASP.NET Core (.NET 10)** pensée pour être déployée sur **Azure App Service** pendant un tutoriel de la formation AZ-104.

Elle affiche une page d'accueil avec le nom du site, l'instance, la version de .NET et l'heure du serveur. C'est pratique pour vérifier qu'un déploiement a réussi, voir quelle instance répond après un scale-out, ou tester un réglage d'application.

## Routes

| Route | Rôle |
|---|---|
| `/` | Page d'accueil (HTML) qui affiche les informations ci-dessous |
| `/api/info` | Informations d'exécution au format JSON |
| `/health` | Health check : répond `200 Healthy` |

## Lancer en local

Prérequis : le [SDK .NET 10](https://dotnet.microsoft.com/download).

```bash
dotnet run
```

Ouvre ensuite l'adresse affichée dans le terminal (par exemple `http://localhost:5000`).

## Déployer sur Azure App Service

### Option 1 : Déploiement depuis GitHub (Deployment Center)

1. Dans le portail Azure, crée une **Web App** : publication *Code*, pile d'exécution **.NET 10**.
2. Ouvre la Web App, puis **Deployment Center**.
3. Source : **GitHub**. Autorise ton compte, puis choisis ce dépôt et la branche `main`.
4. Enregistre. Azure crée un workflow **GitHub Actions** dans le dépôt et déploie à chaque `push`.
5. Ouvre l'URL de la Web App (`https://<nom>.azurewebsites.net`).

### Option 2 : Azure CLI

```bash
az login
az webapp up --name <nom-unique> --resource-group rg-appservice-demo \
  --location westeurope --sku B1
```

`az webapp up` détecte le projet .NET, crée le plan et la Web App, puis déploie le code.

## Expériences tutoriel

- **App settings** : dans *Environment variables*, ajoute un réglage `Greeting` avec le texte de ton choix, enregistre, puis recharge la page. Le titre change sans redéploiement.
- **Scale-out** : passe le plan à 2 instances, puis actualise la page plusieurs fois. Le champ *Instance* change selon l'instance qui répond.
- **Health check** : dans *Health check*, renseigne le chemin `/health`.
- **Logs** : active les *App Service logs* puis ouvre le *Log stream*.

## Structure

```
AppServiceDemo.csproj   Projet .NET
Program.cs              Application (routes /api/info et /health)
appsettings.json        Configuration, dont le message "Greeting"
wwwroot/index.html      Page d'accueil
```

## Licence

MIT, voir [LICENSE](LICENSE).
