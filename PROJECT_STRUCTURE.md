# Projekt Szerkezet és Architektúra

## 📁 Könyvtár Szerkezet

```
EFCodeFirst/
│
├── EFCodeFirst.API/                    # REST API projekt
│   ├── Controllers/
│   │   ├── BooksController.cs          # Könyv CRUD végpontok
│   │   └── PublishersController.cs     # Kiadó CRUD végpontok
│   │
│   ├── Models/
│   │   ├── Book.cs                     # Book entitás
│   │   ├── Publisher.cs                # Publisher entitás
│   │   └── LibraryContext.cs           # DbContext
│   │
│   ├── appsettings.json                # Konfigurációs fájl
│   ├── Program.cs                      # Startup konfigurációja
│   └── EFCodeFirst.API.csproj
│
├── EFCodeFirst.WPF/                    # WPF Desktop alkalmazás
│   ├── MainWindow.xaml                 # Felhasználói felület
│   ├── MainWindow.xaml.cs              # Code-behind logika
│   │
│   ├── Services/
│   │   └── ApiClient.cs                # HTTP kommunikáció az API-val
│   │
│   ├── App.xaml
│   ├── App.xaml.cs
│   └── EFCodeFirst.WPF.csproj
│
└── README.md                           # Dokumentáció
```

---

## 🏗️ Architektúra Diagram

```
┌─────────────────────────────────────────────────────────┐
│                  WPF Alkalmazás                         │
│         (Windows Presentation Foundation)               │
│                                                         │
│  ┌───────────────────────────────────────────────────┐ │
│  │            MainWindow (UI)                        │ │
│  │  ┌─────────────────────────────────────────────┐ │ │
│  │  │  Könyvek ListView  │  Kiadók ListView       │ │ │
│  │  │  Szerkesztő Form   │  Szerkesztő Form      │ │ │
│  │  └─────────────────────────────────────────────┘ │ │
│  └───────────────────────────────────────────────────┘ │
│                      ▼                                  │
│  ┌───────────────────────────────────────────────────┐ │
│  │        ApiClient (Kommunikáció)                  │ │
│  │  HTTP GET, POST, PUT, DELETE kérések            │ │
│  └───────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
                        │
                    HTTP/HTTPS
                        │
                        ▼
┌─────────────────────────────────────────────────────────┐
│           ASP.NET Core REST API                         │
│                                                         │
│  ┌───────────────────────────────────────────────────┐ │
│  │  Controllers                                      │ │
│  │  ┌──────────────────┬──────────────────────────┐ │ │
│  │  │ BooksController  │ PublishersController    │ │ │
│  │  │                  │                          │ │ │
│  │  │ GET /books       │ GET /publishers         │ │ │
│  │  │ POST /books      │ POST /publishers        │ │ │
│  │  │ PUT /books/{id}  │ PUT /publishers/{id}    │ │ │
│  │  │ DELETE /books    │ DELETE /publishers/{id} │ │ │
│  │  └──────────────────┴──────────────────────────┘ │ │
│  └───────────────────────────────────────────────────┘ │
│                      ▼                                  │
│  ┌───────────────────────────────────────────────────┐ │
│  │  Models (Entity Classes)                          │ │
│  │  ┌──────────┐              ┌──────────────┐      │ │
│  │  │ Book     │──┐       ┌──│ Publisher    │      │ │
│  │  │          │  │       │  │              │      │ │
│  │  │ ISBN     │  └───►◄──┘  │ ID           │      │ │
│  │  │ Title    │         (1:N)│ Name         │      │ │
│  │  │ Author   │             │ Books: List  │      │ │
│  │  │ Publisher│◄─────────────┘              │      │ │
│  │  └──────────┘             └──────────────┘      │ │
│  └───────────────────────────────────────────────────┘ │
│                      ▼                                  │
│  ┌───────────────────────────────────────────────────┐ │
│  │  LibraryContext (DbContext)                       │ │
│  │  Entity Framework Core                            │ │
│  └───────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
                        │
                    MySQL Driver
                        │
                        ▼
┌─────────────────────────────────────────────────────────┐
│              MySQL Adatbázis                            │
│                                                         │
│  ┌───────────────────────────────────────────────────┐ │
│  │  library database                                 │ │
│  │                                                   │ │
│  │  ┌──────────────────┐   ┌───────────────────┐  │ │
│  │  │ books tábla      │   │ publishers tábla  │  │ │
│  │  │                  │   │                   │  │ │
│  │  │ ISBN (PK)        │   │ ID (PK)           │  │ │
│  │  │ Title            │   │ Name              │  │ │
│  │  │ Author           │   │ PublisherID (FK)──┼──┼─┤
│  │  │ Language         │   │                   │  │ │
│  │  │ Pages            │   └───────────────────┘  │ │
│  │  │ Description      │                           │ │
│  │  │ PublisherID (FK)─┼──┐                       │ │
│  │  └──────────────────┘   │                       │ │
│  │                          ▼                       │ │
│  │                   (Foreign Key)                 │ │
│  └───────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
```

---

## 🔄 Adatfolyam Diagram

### Könyv Hozzáadása

```
1. Felhasználó kitölt egy formot (ISBN, Cím, Szerző stb.)
   ↓
2. WPF alkalmazás CreateBook() metódus hívása
   ↓
3. ApiClient HTTP POST kérést küld az API-nak
   POST /api/books
   {
     "isbn": "978-...",
     "title": "Cím",
     ...
   }
   ↓
4. API BooksController.CreateBook() végpont feldolgozza
   ↓
5. DbContext lekérdezi a kiadót az adatbázisból
   ↓
6. Új Book entitás létrehozása
   ↓
7. SaveChangesAsync() - adatbázisba mentés
   ↓
8. API 201 Created válasz küldése
   ↓
9. WPF megjeleníti a sikert és frissíti a listát
```

### Könyv Szerkesztése

```
1. Felhasználó dupla kattint egy könyvre az listában
   ↓
2. WPF betölti a könyv adatait a formba
   ↓
3. Felhasználó módosít az adatok között
   ↓
4. Kattint a "Mentés" gombra
   ↓
5. ApiClient HTTP PUT kérést küld
   PUT /api/books/{isbn}
   {
     "title": "Új cím",
     ...
   }
   ↓
6. API BooksController.UpdateBook() végpont dolgozza fel
   ↓
7. Meglévő Book entitás frissítése az adatbázisban
   ↓
8. API 200 OK válasz küldése
   ↓
9. WPF frissíti a listát
```

### Könyv Törlése

```
1. Felhasználó megnyit egy könyvet szerkesztésre
   ↓
2. Kattint a "Törlés" gombra
   ↓
3. Pottvitásítási ablak megjelenik
   ↓
4. Felhasználó megerősíti a törlést
   ↓
5. ApiClient HTTP DELETE kérést küld
   DELETE /api/books/{isbn}
   ↓
6. API BooksController.DeleteBook() végpont dolgozza fel
   ↓
7. Book entitás törlése az adatbázisból
   ↓
8. API 200 OK válasz küldése
   ↓
9. WPF frissíti a listát
```

---

## 🔗 Relációk

### Egy-több kapcsolat (One-to-Many)
- **Publisher (1) ←→ (N) Books**
- Egy kiadónak több könyve lehet
- Egy könyvnek pontosan egy kiadója van

```
Publisher
├── ID (Primary Key)
├── Name
└── Books (ICollection<Book>)
     ↓
    Book
    ├── ISBN (Primary Key)
    ├── Title
    ├── Author
    ├── Language
    ├── Pages
    ├── Description
    └── Publisher (Foreign Key)
```

---

## 📡 API Végpontok Teljes Listája

### Könyvek Management

| Metódus | Végpont | Request Body | Response | Status |
|---------|---------|--------------|----------|--------|
| GET | `/api/books` | - | List[BookDto] | 200 |
| GET | `/api/books/{isbn}` | - | BookDto | 200/404 |
| POST | `/api/books` | CreateBookDto | BookDto | 201/400 |
| PUT | `/api/books/{isbn}` | UpdateBookDto | Success | 200/400/404 |
| DELETE | `/api/books/{isbn}` | - | Success | 200/404 |

### Kiadók Management

| Metódus | Végpont | Request Body | Response | Status |
|---------|---------|--------------|----------|--------|
| GET | `/api/publishers` | - | List[PublisherDto] | 200 |
| GET | `/api/publishers/{id}` | - | PublisherDto | 200/404 |
| POST | `/api/publishers` | CreatePublisherDto | PublisherDto | 201 |
| PUT | `/api/publishers/{id}` | UpdatePublisherDto | Success | 200/400/404 |
| DELETE | `/api/publishers/{id}` | - | Success | 200/400/404 |

---

## 📦 NuGet Csomagok

### API Projekthez
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0+" />
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="7.0+" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.0+" />
</ItemGroup>
```

### WPF Projekthez
```xml
<ItemGroup>
    <!-- Nincs szükség további csomagra, a .NET beépített biztosít mindent -->
</ItemGroup>
```

---

## 🔐 Adatbázis Entitások

### Book Tábla
```sql
CREATE TABLE books (
    ISBN VARCHAR(50) PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Author VARCHAR(255),
    Language VARCHAR(50),
    Pages INT,
    Description LONGTEXT,
    PublisherID INT NOT NULL,
    FOREIGN KEY (PublisherID) REFERENCES publishers(ID)
);
```

### Publisher Tábla
```sql
CREATE TABLE publishers (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL
);
```

---

## 🎯 Rendszer Jellemzői

✅ **Teljes CRUD funkciók**
- Könyvek kezelése (Create, Read, Update, Delete)
- Kiadók kezelése (Create, Read, Update, Delete)

✅ **RESTful API**
- Standard HTTP metódusok
- JSON adatcsere
- Swagger dokumentáció

✅ **WPF Felhasználói Felület**
- DataGrid táblázatok
- Szerkesztő formok
- Hibakezelés és üzenetkezés

✅ **Entity Framework Code-First**
- Modell-alapú adatbázis
- Type-safe lekérdezések
- Automata migrációk

✅ **Aszinkron Operációk**
- Non-blocking HTTP kérések
- Responsive felhasználói felület

---

## 🚀 Deployment Lehetőségek

1. **Helyi fejlesztés**: `dotnet run`
2. **IIS**: `dotnet publish -c Release`
3. **Docker**: Containerizálás
4. **Azure**: Cloud deployment
5. **Linux**: systemd service

---

## 📊 Technológiai Verem

| Réteg | Technológia |
|-------|-------------|
| Frontend | WPF (XAML) |
| Backend | ASP.NET Core |
| ORM | Entity Framework Core |
| Adatbázis | MySQL |
| API Dokumentáció | Swagger/OpenAPI |
| Kommunikáció | HTTP/HTTPS, JSON |

