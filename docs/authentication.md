# Authentication API

Uygulamayı HTTPS profiliyle çalıştırın:

```powershell
dotnet run --project API --launch-profile https
```

Yerel adres: `https://localhost:7205`.

| Endpoint | Açıklama |
| --- | --- |
| `GET /api/auth/csrf` | CSRF cookie'sini ve JSON içinde `requestToken` değerini verir. |
| `POST /api/auth/register` | Müşteri oluşturur; başarılıysa kullanıcı bilgileriyle `201` döner. Oturum açmaz. |
| `POST /api/auth/login` | E-posta/parolayı doğrular, authentication cookie'si ve kullanıcı bilgileriyle `200` döner. |
| `GET /api/auth/me` | Oturumdaki kullanıcının kimlik, ad, soyad, e-posta ve rol bilgilerini verir. |
| `POST /api/auth/logout` | Cookie'yi siler; `204` döner. |

## İstek akışı

1. Önce `GET /api/auth/csrf` çağırın; cookie'yi koruyup `requestToken` değerini alın.
2. POST isteklerinde bu değeri `X-CSRF-TOKEN` başlığına koyun. JSON gönderirken `Content-Type: application/json` kullanın.
3. Login başarılı olduktan sonra `GET /api/auth/csrf` çağrısını tekrarlayın. Token, mevcut kullanıcının kimliğine bağlıdır.
4. Logout sonrasında anonim istekler için yeniden token alın.

Tarayıcıda aynı origin'den yapılan `fetch` istekleri cookie'leri varsayılan olarak taşır. HTTP istemcisi kullanıyorsanız cookie jar/session özelliğini açık tutun. Token tek başına yeterli değildir; CSRF cookie'si de gönderilmelidir.

## Kayıt gövdesi

```json
{
  "firstName": "Ada",
  "lastName": "Yilmaz",
  "email": "ada@example.com",
  "password": "ExamplePass1!",
  "confirmPassword": "ExamplePass1!"
}
```

Ad/soyad zorunlu ve en fazla 100 karakterdir. E-posta zorunlu, geçerli biçimde ve en fazla 255 karakter olmalıdır. Parola 8–128 karakter olmalı; mevcut Identity politikası büyük/küçük harf, rakam ve özel karakter de gerektirir. Parola tekrarı eşleşmelidir.

Rol sunucuda daima `customer` olarak atanır. İstekten `role` veya `userId` alınmaz. Adres yönetimi ayrı aşamada eklenecektir.

## Giriş gövdesi

```json
{
  "email": "ada@example.com",
  "password": "ExamplePass1!",
  "rememberMe": false
}
```

`rememberMe: true` kalıcı cookie ister. Cookie HTTPS, HttpOnly ve SameSite=Lax ayarlarıyla oluşturulur; bilet süresi 8 saattir ve kayan süre yenilemesi açıktır. Başarısız parola denemeleri Identity'nin hesap kilitleme sayacına dahil edilir.

## Hatalar

- `400`: Alan doğrulaması, parola politikası veya eksik/geçersiz CSRF token'ı.
- `401`: Oturum yok/geçersiz, giriş bilgileri yanlış veya hesap girişe uygun değil.
- `403`: Kullanıcının erişim için gereken rolü yok.
- `409`: E-posta zaten kayıtlı.

`me` isteğinde kullanıcı kimliği doğrulanmış `HttpContext.User` içindeki `NameIdentifier` claim'inden alınır. Parola hash'i ve Identity'nin dahili güvenlik alanları yanıta dahil edilmez.
