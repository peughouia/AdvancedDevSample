# DOCUMENTATION_TECHNIQUE
*Le coeur technique du projet, qui détaille les choix d'architecture*

```
🛠️ Documentation Technique

🏗️ Architecture en Couches (Clean Architecture)

Le projet est divisé en 4 projets distincts respectant la règle de dépendance (l'extérieur dépend de l'intérieur, l'intérieur ne dépend de rien).

┌─────────────────────────────────────────────────────────┐
│                    API (Présentation)                   │
│         Controllers, Middlewares, Configuration         │
└────────────────────────┬────────────────────────────────┘
                         │ dépend de
┌────────────────────────▼────────────────────────────────┐
│                    APPLICATION                          │
│         Services, DTOs, Exceptions Applicatives         │
└────────────────────────┬────────────────────────────────┘
                         │ dépend de
┌────────────────────────▼────────────────────────────────┐
│                      DOMAINE (CORE)                     │
│    Entities, Value Objects, Interfaces Repository       │
└─────────────────────────────────────────────────────────┘
                         ▲
┌────────────────────────┴────────────────────────────────┐
│                  INFRASTRUCTURE                         │
│       Implémentation des Repositories (In-Memory)       │
└─────────────────────────────────────────────────────────┘
```

----
# 🎯 Modélisation Métier (DDD)
Nous avons identifié deux grands contextes (Aggregates) :

### 1. Model : Product
- Entité Racine : ``Products``
- Value Object : ``Prices`` (Encapsule la valeur décimale du prix et garantit qu'il est toujours strictement positif).
- Règles : Le nom ne peut être vide, le stock ne peut être négatif, une promotion ne peut s'appliquer que sur un produit actif.

### 2. Model : Order (Commande)
- Entité Racine : ``Order``
- Entité Enfant : ``OrderItem`` (Ligne de commande).
- Règles : - À l'ajout d'un produit, on fige son prix unitaire dans ``OrderItem`` pour que les changements futurs du catalogue n'affectent pas la commande validée.

    - Le calcul du Total est dynamique.
    - Gestion stricte des statuts (``Cart`` ➔ Validated ou Cancelled).

----
# ⚠️ Gestion Centralisée des Erreurs
Les try-catch sont bannis des **contrôleurs**. Un ExceptionHandlingMiddleware intercepte toutes les exceptions et normalise les réponses HTTP :


| Exception Levée | Code HTTP Retourné | Cas d'usage |
| --- | --- | --- |
| `DomainException` | **400 Bad Request** | Violation d'une règle métier pure (ex: prix < 0). |
| `NotFoundException` | **404 Not Found** | Produit ou commande inexistante dans la base. |
| `ApplicationException` | **400 Bad Request** | Erreur de logique applicative (ex: Stock insuffisant). |
| `Exception` (non gérée) | **500 Internal Error** | Bug technique inattendu. |

## 🧪 Stratégie de Tests

* **Tests Unitaires (xUnit) :** Isolent le Domaine pour vérifier que `Product` et `Prices` réagissent correctement aux données invalides.
* **Mocking (Moq) :** Utilisation de simulacres pour l'`IProductRepository` afin de tester la logique du `ProductService` (Couche Application) sans toucher à l'Infrastructure.

## Exécuter tous les tests de la solution
``dotnet test``

## Exécuter les tests avec un niveau de détail complet (utile pour le débogage)
``dotnet test --verbosity detailed``

