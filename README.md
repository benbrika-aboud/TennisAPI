🎾 TennisAPI
TennisAPI est une API REST développée en .NET 8 pour la gestion des joueurs de tennis et le suivi de leurs statistiques. 
Elle permet de consulter la liste des joueurs, d’ajouter de nouveaux joueurs et de récupérer des statistiques globales, le tout via des endpoints simples et structurés. 
basée sur une clean architecture avec les couches API, Application, Domain, Infrastructure, et des tests unitaires.

🏗️ Structure du projet
TennisAPI/
├── API/                # Point d'entrée de l'application, Controllers et configuration
├── Application/        # Services métiers, interfaces, gestion des cas d'utilisation
├── Domain/             # Entités, règles métiers et agrégats
├── Infrastructure/     # Accès aux données, implémentations concrètes et services externes
├── Tests/              # Tests unitaires et d'intégration
└── README.md           # Documentation du projet

🚀 Endpoints principaux

| Méthode | Endpoint             | Description                                             | Corps de la requête | Réponse                      |
| ------- | -------------------- | ------------------------------------------------------- | ------------------- | ---------------------------- |
| GET     | `/api/players`       | Récupère la liste complète des joueurs (triée par rang) | –                   | Liste des joueurs            |
| GET     | `/api/players/{id}`  | Récupère un joueur par son ID                           | –                   | Joueur ou 404 si introuvable |
| POST    | `/api/players`       | Ajoute un nouveau joueur                                | `Player` JSON       | Joueur créé (201)            |
| GET     | `/api/players/stats` | Récupère les statistiques globales sur les joueurs      | –                   | Statistiques des joueurs     |



🏛️ Prérequis
.NET 8 SDK
Visual Studio 2022 ou VS Code

⚙️ Installation
Cloner le projet :
git clone https://github.com/benbrika-aboud/TennisAPI.git
cd TennisAPI

Restaurer les dépendances :
dotnet restore

Lancer l’API :
dotnet run --project API/TennisAPI.csproj

✅ Tests
dotnet test Tests/TennisAPI.Tests.csproj

