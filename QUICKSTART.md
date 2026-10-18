# ⚡ Gyors Indítás - 5 perc alatt

## 🎯 Cél
API és WPF alkalmazás, amely működik az Entity Framework modelleiddel.

---

## 🚀 1. Lépés: API Projekt

### Parancsok futtatása (Visual Studio Code vagy Command Prompt)

```bash
# API projekt létrehozása
dotnet new webapi -n EFCodeFirst.API
cd EFCodeFirst.API

# Szükséges csomagok
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Swashbuckle.AspNetCore
```

### Fájlok másolása

1. **Controllers mappa létrehozása**
   - `Controllers/BooksController.cs` (az én BooksController.cs fájlom)
   - `Controllers/PublishersController.cs` (az én PublishersController.cs fájlom)

2. **Models mappa előkészítése**
   - Másolja az eredeti `Book.cs`-t
   - Másolja az eredeti `Publisher.cs`-t
   - Másolja az eredeti `LibraryContext.cs`-t

3. **Program.cs lecserélése**
   - Helyettesítse az eredeti Program.cs-t az én Program.cs fájlommal

### Futtatás

```bash
dotnet run
```

✅ Az API fut a `https://localhost:5001` alatt
🌐 Swagger UI: `https://localhost:5001/swagger`

---

## 🖥️ 2. Lépés: WPF Alkalmazás

### Projekt létrehozása

```bash
# Vissza a meghajtó gyökerébe
cd ..

# WPF projekt
dotnet new wpf -n EFCodeFirst.WPF
cd EFCodeFirst.WPF
```

### Fájlok másolása

1. **MainWindow.xaml**
   - Cserélje le az eredeti MainWindow.xaml fájlt az én fájlomra

2. **MainWindow.xaml.cs**
   - Cserélje le az eredeti MainWindow.xaml.cs fájlt az én fájlomra

3. **Services mappa létrehozása**
   - Könyvtár hozzáadása: `Services/`
   - `Services/ApiClient.cs` másolása (az én ApiClient.cs fájlom)

### Futtatás

```bash
dotnet run
```

✅ Az alkalmazás megnyílik!

---

## ⚙️ 3. Lépés: Konfigurálás

### MySQL Adatbázis

```bash
# MySQL-ben
mysql -u root
```

```sql
CREATE DATABASE library;
USE library;
```

### LibraryContext.cs módosítása

Nyissa meg az API projekt `Models/LibraryContext.cs` fájlját és szerkessze ezt:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    // Módosítsa a szервер, felhasználó, jelszó adatokat
    optionsBuilder.UseMySQL("server=localhost;user=root;password=YOUR_PASSWORD;database=library");
}
```

---

## 🎮 4. Lépés: Tesztelés

### WPF Alkalmazásban

1. **Frissítés gomb** - Kiadók listája betöltődik
2. **Új kiadó hozzáadása** - "Új kiadó" gomb
3. **Könyv hozzáadása** - Válasszon ki egy kiadót, majd "Új könyv"
4. **Szerkesztés** - Dupla kattintás egy sorra
5. **Törlés** - Nyissa meg, majd "Törlés" gomb

---

## 📋 Checklist

- [ ] API projekt létrehozva
- [ ] Controllers fájlok másolva
- [ ] Models fájlok másolva
- [ ] Program.cs cserélt
- [ ] API tesztelve: `https://localhost:5001/swagger`
- [ ] WPF projekt létrehozva
- [ ] MainWindow fájlok másolva
- [ ] ApiClient.cs másolva
- [ ] MySQL adatbázis létrehozva
- [ ] Jelszó beállítva LibraryContext.cs-ben
- [ ] WPF alkalmazás indult
- [ ] Tesztelve: könyv hozzáadása, szerkesztése, törlése

---

## 🆘 Gyors Hibajavítás

### "Nem tudok csatlakozni a MySQL-hez"
```csharp
// Ellenőrizze LibraryContext.cs-ben:
optionsBuilder.UseMySQL("server=localhost;user=root;password=your_password;database=library");
```

### "Az API nem induló meg"
```bash
# SSL tanúsítvány problémája
dotnet dev-certs https --trust
dotnet run
```

### "WPF meghiúsult a csatlakozáskor"
- Ellenőrizze, hogy az API fut-e (https://localhost:5001)
- Ellenőrizze az ApiClient.cs BaseUrl értékét

### "Nincs 'Kiadók' az adatbázisban"
1. WPF alkalmazásban kattintson "Új kiadó" gombra
2. Adjon hozzá egy kiadót (pl. "Penguin Books")
3. Mentse el

---

## 📚 Útmutatások

- **Teljes telepítés**: SETUP_GUIDE.md
- **Architektúra**: PROJECT_STRUCTURE.md
- **API végpontok**: PROJECT_STRUCTURE.md > API Végpontok

---

## 🎉 Kész!

Az alkalmazása teljesen működőképes! Mostantól:
- ✅ Könyveket adhat hozzá
- ✅ Könyveket szerkeszthet
- ✅ Könyveket törölhet
- ✅ Kiadókat kezelhet

**Gratulálunk! 🎊**

---

## 💡 Jelenlegi Funkciók

| Funkció | Státusz |
|---------|---------|
| Könyvek listázása | ✅ |
| Könyv hozzáadása | ✅ |
| Könyv szerkesztése | ✅ |
| Könyv törlése | ✅ |
| Kiadók listázása | ✅ |
| Kiadó hozzáadása | ✅ |
| Kiadó szerkesztése | ✅ |
| Kiadó törlése | ✅ |
| REST API | ✅ |
| Swagger dokumentáció | ✅ |

---

## 🔧 További Testreszabások

Ha szeretne javításokat vagy új funkciókat:

1. **Ui javítás** - Módosítsa MainWindow.xaml-t
2. **Új mező hozzáadása** - Szerkessze a Model és a DTO-kat
3. **Validáció** - Adjon hozzá logikát MainWindow.xaml.cs-ben
4. **Stílus** - Frissítse a WPF témát

---

**Kezdjen az 1. lépéstől és haljon felfelé!**
