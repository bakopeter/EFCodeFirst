# Könyvtár Kezelő - API & WPF Alkalmazás Telepítési Útmutató

## 📋 Projekt Áttekintése

Ez a projekt egy teljes CRUD alkalmazás:
- **API**: ASP.NET Core REST API
- **Frontend**: WPF (Windows Presentation Foundation) Desktop Alkalmazás
- **Adatbázis**: MySQL (Entity Framework Code-First)

---

## 🚀 Telepítési Lépések

### 1. API Projekt Létrehozása

#### A. Új projekt létrehozása
```bash
dotnet new webapi -n EFCodeFirst.API
cd EFCodeFirst.API
```

#### B. NuGet csomagok telepítése
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Swashbuckle.AspNetCore
```

#### C. Fájlok másolása
Az alábbi fájlokat másolja az API projektbe:
- `BooksController.cs` → `Controllers/BooksController.cs`
- `PublishersController.cs` → `Controllers/PublishersController.cs`
- `Book.cs` → `Models/Book.cs` (az eredeti Entity Framework modellek)
- `Publisher.cs` → `Models/Publisher.cs`
- `LibraryContext.cs` → `Models/LibraryContext.cs`
- `Program.cs` → helyettesítse az eredeti `Program.cs` fájlt

#### D. SSL Tanúsítvány
```bash
dotnet dev-certs https --trust
```

#### E. Futtatás
```bash
dotnet run
# Az API a https://localhost:5001 címen fut
# Swagger UI: https://localhost:5001/swagger
```

---

### 2. WPF Projekt Létrehozása

#### A. Új WPF projekt létrehozása
```bash
dotnet new wpf -n EFCodeFirst.WPF
cd EFCodeFirst.WPF
```

#### B. XAML és Code-behind másolása
Másolja az alábbi fájlokat:
- `MainWindow.xaml` → `MainWindow.xaml`
- `MainWindow.xaml.cs` → `MainWindow.xaml.cs`
- `ApiClient.cs` → `Services/ApiClient.cs`

#### C. Futtatás
```bash
dotnet run
```

---

## 🔧 Konfigurációs Beállítások

### API - Adatbázis Kapcsolat
A `LibraryContext.cs` fájlban szükséges lehet módosítani az adatbázis connection stringet:

```csharp
optionsBuilder.UseMySQL("server=localhost;user=root;password=YOUR_PASSWORD;database=library");
```

### WPF - API Cím
Az `ApiClient.cs` fájlban szerkessze az API base URL-t szükség szerint:

```csharp
private const string BaseUrl = "https://localhost:5001/api";
```

---

## 📊 Adatbázis Inicializálás

### 1. Adatbázis létrehozása MySQL-ben
```sql
CREATE DATABASE library;
USE library;
```

### 2. Entity Framework Migrations (opcionális)
```bash
cd EFCodeFirst.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 🎯 Alapvető Funkciók

### Könyvek Management
- ✅ **Lista megtekintése**: Kattintson "Frissítés" gombra
- ✅ **Új könyv hozzáadása**: Kattintson "Új könyv" gombra
- ✅ **Könyv szerkesztése**: Dupla kattintás a táblázaton
- ✅ **Könyv törlése**: Nyissa meg szerkesztésre, majd kattintson "Törlés"

### Kiadók Management
- ✅ **Lista megtekintése**: Kattintson "Frissítés" gombra
- ✅ **Új kiadó hozzáadása**: Kattintson "Új kiadó" gombra
- ✅ **Kiadó szerkesztése**: Dupla kattintás a táblázaton
- ✅ **Kiadó törlése**: Nyissa meg szerkesztésre, majd kattintson "Törlés"

---

## 📝 API Végpontok

### Könyvek

| Metódus | Végpont | Leírás |
|---------|---------|--------|
| GET | `/api/books` | Összes könyv |
| GET | `/api/books/{isbn}` | Egy könyv |
| POST | `/api/books` | Új könyv létrehozása |
| PUT | `/api/books/{isbn}` | Könyv frissítése |
| DELETE | `/api/books/{isbn}` | Könyv törlése |

### Kiadók

| Metódus | Végpont | Leírás |
|---------|---------|--------|
| GET | `/api/publishers` | Összes kiadó |
| GET | `/api/publishers/{id}` | Egy kiadó |
| POST | `/api/publishers` | Új kiadó létrehozása |
| PUT | `/api/publishers/{id}` | Kiadó frissítése |
| DELETE | `/api/publishers/{id}` | Kiadó törlése |

---

## 🧪 Tesztelés Postman-nel

1. Importálja a Postman-be ezt az URL-t (vagy manuálisan adja hozzá):
```
https://localhost:5001/api/books
```

2. Példa POST kérés:
```json
{
  "isbn": "978-0-13-110362-7",
  "title": "The C# Programming Language",
  "author": "Anders Hejlsberg",
  "language": "English",
  "pages": 928,
  "description": "A comprehensive guide to C#",
  "publisherID": 1
}
```

---

## 🐛 Hibakezelés

### Gyakori hibák

**1. SSL tanúsítvány hiba**
- Megoldás: `dotnet dev-certs https --trust`

**2. Adatbázis kapcsolati hiba**
- Ellenőrizze a connection stringet
- Valódi MySQL szerver fut-e?

**3. "Kiadó nem található" hiba**
- Hozzon létre előbb kiadókat az alkalmazásban

**4. CORS hiba**
- Az API már konfigurálva van CORS-ra, ellenőrizze az URL-t

---

## 📚 Támogatott Technológiák

- **.NET 7+** vagy **.NET 6+**
- **Entity Framework Core**
- **ASP.NET Core**
- **WPF**
- **MySQL**
- **C# 11+**

---

## 🔐 Biztonsági Megjegyzések

- Ez egy fejlesztési demo
- Éles használathoz:
  - Implementáljon autentikációt (JWT)
  - Validáljon összes inputot
  - Használjon HTTPS-t
  - Titkosítson bizalmas adatokat
  - Implementáljon rate limiting-et

---

## 💡 Továbbfejlesztési Ötletek

- [ ] Felhasználói autentikáció
- [ ] Könyv keresés szűrésével
- [ ] Export PDF/Excel-be
- [ ] Önálló könyv jelölés
- [ ] Felhasználói felületen üzenetek javítása
- [ ] Szinkron/aszinkron operációk

---

## 📞 Támogatás

Ha problémája van:
1. Ellenőrizze a status üzenetet az alkalmazásban (piros szöveg = hiba)
2. Nézze meg az API Swagger UI-t a hibakódokért
3. Ellenőrizze a MySQL adatbázist

---

**Készült: C# Entity Framework Code-First alapján**
