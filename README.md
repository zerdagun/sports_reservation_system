# Spor Rezervasyon Sistemi API

.NET 9 ile geliştirilmiş, katmanlı mimari kullanılarak oluşturulmuş REST API projesi. Spor tesisleri için rezervasyon yönetim sistemi.

## 📋 İçindekiler

- [Teknolojiler](#teknolojiler)
- [Mimari](#mimari)
- [Kurulum](#kurulum)
- [API Endpoint'leri](#api-endpointleri)
- [Authentication](#authentication)
- [API Response Formatı](#api-response-formatı)
- [Örnek Kullanımlar](#örnek-kullanımlar)

## 🛠 Teknolojiler

- **.NET 9** (zorunlu)
- **Entity Framework Core 9.0** (ORM)
- **SQLite** (Veritabanı)
- **AutoMapper** (DTO Mapping)
- **JWT Bearer Authentication** (Kimlik Doğrulama)
- **Swagger/OpenAPI** (API Dokümantasyonu)
- **Minimal API** (Endpoint'ler)

## 🏗 Mimari

Proje **3 katmanlı mimari** (Layered Architecture) kullanılarak geliştirilmiştir:

```
sports_reservation_system/
├── sports_reservation_system.API/          # Presentation Layer
│   ├── Controllers/                      # Controller-based endpoints
│   ├── Middleware/                        # Exception Handling
│   └── Program.cs                         # Minimal API endpoints
│
├── sports_reservation_system.Business/     # Business Layer
│   ├── Services/                          # Business logic
│   ├── DTOs/                              # Data Transfer Objects
│   ├── Mappings/                          # AutoMapper profiles
│   └── Common/                             # Shared utilities
│
└── sports_reservation_system.Data/          # Data Layer
    ├── Entities/                           # Domain models
    ├── Repositories/                       # Data access
    ├── UnitOfWork/                         # Transaction management
    ├── Seed/                               # Seed data
    └── Migrations/                          # Database migrations
```

### Mimari Diyagramı

```
┌─────────────────────────────────────────────────────────┐
│                    API Layer (Controllers)               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │   Branches   │  │    Users     │  │   Sessions   │  │
│  │  Controller  │  │  Controller  │  │  Controller  │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
│                                                          │
│              Minimal API Endpoints                       │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│              Business Layer (Services)                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │   Branch     │  │    User      │  │   Session    │  │
│  │   Service    │  │   Service    │  │   Service    │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
│                                                          │
│              DTOs + AutoMapper                           │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│              Data Layer (Repositories)                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │  Generic     │  │   Unit of    │  │   App        │  │
│  │  Repository  │  │    Work      │  │   DbContext  │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│                    SQLite Database                      │
└─────────────────────────────────────────────────────────┘
```

## 🚀 Kurulum

### Gereksinimler

- .NET 9 SDK
- Visual Studio 2022 veya VS Code
- Git

### Adımlar

1. **Projeyi klonlayın:**
```bash
git clone <repository-url>
cd sports_reservation_system
```

2. **Veritabanı migration'larını uygulayın:**
```bash
cd sports_reservation_system.API
dotnet ef database update --project ../sports_reservation_system.Data
```

3. **Projeyi çalıştırın:**
```bash
dotnet run
```

4. **Swagger UI'ya erişin:**
```
https://localhost:5001/swagger
```

## 📡 API Endpoint'leri

### Authentication Endpoints

| Method | Endpoint | Açıklama | Auth |
|--------|----------|----------|------|
| POST | `/api/auth/login` | Kullanıcı girişi | ❌ |
| POST | `/api/auth/register` | Kullanıcı kaydı | ❌ |

### Branches (Şubeler)

#### Controller Endpoints
| Method | Endpoint | Açıklama | Auth | Role |
|--------|----------|----------|------|------|
| GET | `/api/branches` | Tüm şubeleri listele | ✅ | - |
| GET | `/api/branches/{id}` | Şube detayı | ✅ | - |
| POST | `/api/branches` | Yeni şube ekle | ✅ | Admin |
| PUT | `/api/branches/{id}` | Şube güncelle | ✅ | Admin |
| DELETE | `/api/branches/{id}` | Şube sil (soft delete) | ✅ | Admin |

#### Minimal API Endpoints
| Method | Endpoint | Açıklama | Auth | Role |
|--------|----------|----------|------|------|
| GET | `/api/minimal/branches` | Tüm şubeleri listele | ✅ | - |
| GET | `/api/minimal/branches/{id}` | Şube detayı | ✅ | - |
| POST | `/api/minimal/branches` | Yeni şube ekle | ✅ | Admin |
| PUT | `/api/minimal/branches/{id}` | Şube güncelle | ✅ | Admin |
| DELETE | `/api/minimal/branches/{id}` | Şube sil | ✅ | Admin |

### Sports (Sporlar)

| Method | Endpoint | Açıklama | Auth | Role |
|--------|----------|----------|------|------|
| GET | `/api/sports` | Tüm sporları listele | ✅ | - |
| GET | `/api/sports/{id}` | Spor detayı | ✅ | - |
| POST | `/api/sports` | Yeni spor ekle | ✅ | Admin |
| PUT | `/api/sports/{id}` | Spor güncelle | ✅ | Admin |
| DELETE | `/api/sports/{id}` | Spor sil (soft delete) | ✅ | Admin |

#### Minimal API Endpoints (Sports)
| Method | Endpoint | Açıklama | Auth | Role |
|--------|----------|----------|------|------|
| GET | `/api/minimal/sports` | Tüm sporları listele | ✅ | - |
| GET | `/api/minimal/sports/{id}` | Spor detayı | ✅ | - |
| POST | `/api/minimal/sports` | Yeni spor ekle | ✅ | Admin |
| PUT | `/api/minimal/sports/{id}` | Spor güncelle | ✅ | Admin |
| DELETE | `/api/minimal/sports/{id}` | Spor sil | ✅ | Admin |

### Users (Kullanıcılar)
...

## 🗄 Veritabanı Yapısı

### Entities

- **User**: Kullanıcı bilgileri
- **Branch**: Spor tesisi şubeleri
- **Sport**: Spor branşları (Futbol, Basketbol, Buz Pateni vb.)
- **Session**: Spor seansları (Artık SportId ile ilişkilidir)
- **Reservation**: Rezervasyonlar

### İlişkiler

- User ↔ Reservation (1-N)
- Session ↔ Reservation (1-N)
- Branch ↔ Session (1-N)
- Sport ↔ Session (1-N)

### Soft Delete

Tüm entity'ler `IsDeleted` alanı ile soft delete destekler. Silinen kayıtlar fiziksel olarak silinmez, sadece `IsDeleted = true` olarak işaretlenir.

## 🌱 Seed Data

Uygulama ilk çalıştırıldığında otomatik olarak seed data eklenir:

- **Admin User**: `admin@example.com` / `Admin123!`
- **Test User**: `user@example.com` / `User123!`
- **5 Spor**: (Buz Pateni, Futbol, Basketbol, Tenis, Yüzme)
- **3 Şube** (Merkez, Kuzey, Güney)
- **9 Seans** (Her şube için 3 seans - Rastgele sporlarla)
- **3 Rezervasyon**

## 📝 Logging (Serilog)

Proje **Serilog** ile gelişmiş loglama altyapısına sahiptir. Loglar hem **konsola** hem de **dosyaya** yazılır.

- **Dosya Yolu**: `/logs/log-{tarih}.txt`
- **Format**: JSON structured logging
- **Log Seviyeleri**: Information, Warning, Error, Fatal

## ✅ Özellikler

- ✅ .NET 9
- ✅ Katmanlı Mimari (Layered Architecture)
- ✅ CRUD İşlemleri (Controller + Minimal API)
- ✅ DTO Kullanımı (Create, Update, Response)
- ✅ Standart API Response Formatı
- ✅ Global Exception Handling
- ✅ JWT Authentication & Authorization
- ✅ Role-Based Access Control (Admin/User)
- ✅ Soft Delete
- ✅ Seed Data
- ✅ Advanced Logging (Serilog)
- ✅ Swagger/OpenAPI Dokümantasyonu
- ✅ Entity Framework Core Migrations

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## 👨‍💻 Geliştirici

.NET 9 REST API Ödevi - Yazılım Mimarisi Dersi

---

**Not**: Bu proje .NET 9 REST API ödevi gereksinimlerine göre geliştirilmiştir.

