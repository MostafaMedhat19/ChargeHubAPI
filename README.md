# ChargeHub Wireless Charging API

This document walks through every available HTTP endpoint so the Flutter / Web front-end can integrate confidently.  
Architecture follows ASP.NET Core Clean Architecture — **API layer** hosts the controllers, **Application layer** handles business rules and DTOs.

> **Headers**  
> - `Content-Type: application/json` for all requests with bodies.  
> - Endpoints marked **(Auth Required)** expect `Authorization: Bearer <JWT_TOKEN>` returned by `POST /api/auth/login`.

---

## 1. Authentication & Verification

### 1.1 Sign Up (Application + API)
`POST /api/auth/signup`

Request:
```json
{
  "username": "mostafa123",
  "name": "Mostafa Medhat",
  "phoneNumber": "+201234567890",
  "email": "mostafamedhat@gmail.com",
  "password": "12345678",
  "confirmPassword": "12345678"
}
```

Response:
```json
{
  "success": true,
  "message": "User created successfully",
  "userId": "USR-551201",
  "identecation": "84219932"
}
```

Notes:
- Backend stores the user, generates `userId` + `identecation`, and emails a 6-digit HTML verification code to `email`.

### 1.2 Verify Signup Code (Application + API)
`POST /api/auth/verify-signup`

Request:
```json
{
  "userId": "USR-551201",
  "verificationCode": "654321"
}
```

Response:
```json
{
  "success": true,
  "message": "Account verified"
}
```

Use this after the user enters the code they received via email.

### 1.3 Sign In (Application + API)
`POST /api/auth/login`

Request:
```json
{
  "username": "mostafa123",
  "password": "12345678"
}
```

Response:
```json
{
  "success": true,
  "token": "JWT_TOKEN",
  "user": {
    "userId": "USR-551201",
    "identecation": "84219932",
    "username": "mostafa123",
    "name": "Mostafa Medhat",
    "phoneNumber": "+201234567890",
    "email": "mostafamedhat@gmail.com",
    "car_charge": 87,
    "esp32": {
      "btName": "ESP32-Charger",
      "btAddress": "AA:BB:CC:DD:EE:FF"
    },
    "status_position": {
      "north": 30,
      "east": 30,
      "south": 30,
      "west": 30
    }
  }
}
```

Save `token` for future authenticated calls.

### 1.4 Forgot Password (Application + API)
`POST /api/auth/forgot-password`

Request:
```json
{
  "phoneNumber": "+201234567890",
  "email": "mostafamedhat@gmail.com"
}
```

Response:
```json
{
  "success": true,
  "message": "Reset code generated and sent"
}
```

### 1.5 Reset Password (Application + API)
`POST /api/auth/reset-password`

Request:
```json
{
  "phoneNumber": "+201234567890",
  "email": "mostafamedhat@gmail.com",
  "resetCode": "123456",
  "newPassword": "12345678",
  "confirmPassword": "12345678"
}
```

Response:
```json
{
  "success": true,
  "message": "Password reset successfully"
}
```

---

## 2. User & ESP32 Endpoints

### 2.1 ESP32 Registration (Application + API)
`POST /api/user/register-esp32`

Request:
```json
{
  "identecation": "84219932",
  "btName": "ESP32-Charger",
  "btAddress": "AA:BB:CC:DD:EE:FF"
}
```
> Backend finds the user by `identecation` (scanned via Flutter) and stores Bluetooth info.

Response:
```json
{
  "success": true,
  "message": "ESP32 registered successfully"
}
```

### 2.2 Get User Info (Application + API) **(Auth Required)**
`GET /api/user/{userId}`

Response:
```json
{
  "userId": "USR-551201",
  "identecation": "84219932",
  "username": "mostafa123",
  "name": "Mostafa Medhat",
  "phoneNumber": "+201234567890",
  "email": "mostafamedhat@gmail.com",
  "car_charge": 87,
  "esp32": {
    "btName": "ESP32-Charger",
    "btAddress": "AA:BB:CC:DD:EE:FF"
  },
  "status_position": {
    "north": 30,
    "east": 30,
    "south": 30,
    "west": 30
  }
}
```

### 2.3 Get Identecation Code (Application + API) **(Auth Required)**
`GET /api/user/{userId}/identecation`

Response:
```json
{
  "success": true,
  "message": "Identecation retrieved",
  "userId": "USR-551201",
  "identecation": "84219932"
}
```

### 2.4 Delete Account (Application + API) **(Auth Required)**
`DELETE /api/user/{userId}`

Response:
```json
{
  "success": true,
  "message": "Account deleted"
}
```

---

## 3. Device → Backend Updates

### 3.1 Update Car Charge (ESP32 → API)
`POST /api/esp32/update-charge`

Request:
```json
{
  "identecation": "84219932",
  "car_charge": 87
}
```

Response:
```json
{
  "success": true,
  "message": "Car charge updated"
}
```

### 3.2 Update Status Position (Hub → API)
`POST /api/hub/update-position`

Request:
```json
{
  "identecation": "84219932",
  "status_position": {
    "north": 30,
    "east": 30,
    "south": 30,
    "west": 30
  }
}
```

Response:
```json
{
  "success": true,
  "message": "Position updated"
}
```

---

## 4. Sequence Summary
1. **Sign Up** → backend emails code.  
2. **Verify Signup** with code.  
3. **Login** → receive JWT.  
4. **Register ESP32** via identecation code when scanned.  
5. **ESP32** pushes car charge every minute; **hub** pushes `status_position`.  
6. **User info / identecation / delete** available via authenticated endpoints.

If you need additional payload fields, sync with backend before rollout so DTOs stay aligned.
