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

### Users (Kullanıcılar)

| Method | Endpoint | Açıklama | Auth | Role |
|--------|----------|----------|------|------|
| GET | `/api/users` | Tüm kullanıcıları listele | ✅ | Admin |
| GET | `/api/users/{id}` | Kullanıcı detayı | ✅ | - |
| PUT | `/api/users/{id}` | Kullanıcı güncelle | ✅ | - |
| DELETE | `/api/users/{id}` | Kullanıcı sil | ✅ | Admin |

### Sessions (Seanslar)

| Method | Endpoint | Açıklama | Auth | Role |
|--------|----------|----------|------|------|
| GET | `/api/sessions` | Tüm seansları listele | ✅ | - |
| GET | `/api/sessions/{id}` | Seans detayı | ✅ | - |
| POST | `/api/sessions` | Yeni seans ekle | ✅ | Admin |
| PUT | `/api/sessions/{id}` | Seans güncelle | ✅ | Admin |
| DELETE | `/api/sessions/{id}` | Seans sil | ✅ | Admin |

### Reservations (Rezervasyonlar)

| Method | Endpoint | Açıklama | Auth | Role |
|--------|----------|----------|------|------|
| GET | `/api/reservations` | Tüm rezervasyonları listele | ✅ | Admin |
| GET | `/api/reservations/{id}` | Rezervasyon detayı | ✅ | - |
| POST | `/api/reservations` | Yeni rezervasyon oluştur | ✅ | - |
| PUT | `/api/reservations/{id}` | Rezervasyon güncelle | ✅ | - |
| DELETE | `/api/reservations/{id}` | Rezervasyon sil | ✅ | - |

## 🔐 Authentication

API, JWT (JSON Web Token) tabanlı kimlik doğrulama kullanır.

### Login İşlemi

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Giriş başarılı.",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "fullName": "Admin User",
      "email": "admin@example.com",
      "role": "Admin"
    },
    "expiresAt": "2024-01-02T12:00:00Z"
  }
}
```

### Token Kullanımı

Tüm korumalı endpoint'lere istek gönderirken `Authorization` header'ında token'ı gönderin:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📦 API Response Formatı

Tüm API yanıtları standart formatta döner:

```json
{
  "success": true,
  "message": "İşlem başarılı",
  "data": {
    // Response verisi burada
  }
}
```

### Hata Durumları

**400 Bad Request:**
```json
{
  "success": false,
  "message": "Geçersiz istek parametreleri",
  "data": null
}
```

**404 Not Found:**
```json
{
  "success": false,
  "message": "ID'si 1 olan şube bulunamadı.",
  "data": null
}
```

**401 Unauthorized:**
```json
{
  "success": false,
  "message": "Email veya şifre hatalı.",
  "data": null
}
```

**409 Conflict:**
```json
{
  "success": false,
  "message": "Bu email adresi zaten kullanılıyor.",
  "data": null
}
```

## 💡 Örnek Kullanımlar

### 1. Kullanıcı Kaydı

```http
POST /api/auth/register
Content-Type: application/json

{
  "fullName": "Yeni Kullanıcı",
  "email": "yeni@example.com",
  "password": "Password123!",
  "role": "User"
}
```

### 2. Şube Ekleme (Admin)

```http
POST /api/branches
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Yeni Şube",
  "description": "Açıklama"
}
```

### 3. Seans Oluşturma (Admin)

```http
POST /api/sessions
Authorization: Bearer {token}
Content-Type: application/json

{
  "startTime": "2024-01-15T10:00:00Z",
  "durationMinutes": 60,
  "quota": 20,
  "price": 100,
  "branchId": 1
}
```

### 4. Rezervasyon Oluşturma

```http
POST /api/reservations
Authorization: Bearer {token}
Content-Type: application/json

{
  "userId": 1,
  "sessionId": 1
}
```

## 🗄 Veritabanı Yapısı

### Entities

- **User**: Kullanıcı bilgileri
- **Branch**: Spor tesisi şubeleri
- **Session**: Spor seansları
- **Reservation**: Rezervasyonlar

### İlişkiler

- User ↔ Reservation (1-N)
- Session ↔ Reservation (1-N)
- Branch ↔ Session (1-N)

### Soft Delete

Tüm entity'ler `IsDeleted` alanı ile soft delete destekler. Silinen kayıtlar fiziksel olarak silinmez, sadece `IsDeleted = true` olarak işaretlenir.

## 🌱 Seed Data

Uygulama ilk çalıştırıldığında otomatik olarak seed data eklenir:

- **Admin User**: `admin@example.com` / `Admin123!`
- **Test User**: `user@example.com` / `User123!`
- **3 Şube** (Merkez, Kuzey, Güney)
- **9 Seans** (Her şube için 3 seans)
- **3 Rezervasyon**

## 📝 Logging

Proje .NET'in built-in logging sistemini kullanır. Loglar şu kategorilerde tutulur:

- **Information**: Genel bilgilendirme
- **Warning**: Uyarılar
- **Error**: Hatalar
- **Exception**: Exception detayları

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
- ✅ Logging
- ✅ Swagger/OpenAPI Dokümantasyonu
- ✅ Entity Framework Core Migrations

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## 👨‍💻 Geliştirici

.NET 9 REST API Ödevi - Yazılım Mimarisi Dersi

---

**Not**: Bu proje .NET 9 REST API ödevi gereksinimlerine göre geliştirilmiştir.

