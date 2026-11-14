# Kayıtlı adres API

Tüm endpoint'ler giriş yapılmasını ve `customer` rolünü gerektirir. Kullanıcı kimliği oturumun `NameIdentifier` claim'inden alınır. İstek gövdesi veya query üzerinden kullanıcı seçilemez.

| Endpoint | Başarılı cevap |
| --- | --- |
| `GET /api/addresses` | Kullanıcının adresleri; adresi yoksa `200` ve `[]`. |
| `GET /api/addresses/{id}` | Kullanıcının tek adresi; `200`. |
| `POST /api/addresses` | Oluşturulan adres ve Location başlığı; `201`. |
| `PUT /api/addresses/{id}` | Adresin tüm alanlarını günceller; `204`. |
| `DELETE /api/addresses/{id}` | Adresi siler; `204`. |

POST ve PUT gövdesi:

```json
{
  "title": "Ev",
  "city": "Istanbul",
  "district": "Kadikoy",
  "addressLine": "Ornek Mahallesi, Ornek Sokak No: 10 Daire: 2"
}
```

Tüm alanlar zorunludur; boş veya yalnızca boşluk içeren değerler kabul edilmez. Title, city ve district en fazla 100, addressLine en fazla 1000 karakter olabilir. Baş/son boşluklar kaydedilirken temizlenir. Aynı kullanıcı birden fazla adres kaydedebilir.

POST, PUT ve DELETE isteklerinde geçerli CSRF cookie'si ve `X-CSRF-TOKEN` başlığı gönderilmelidir. Login sonrasında `/api/auth/csrf` ile token yenilenir. Ayrıntılar: [Authentication API](authentication.md).

## Kayıt sırasında adres ekleme

Register gövdesine aşağıdaki isteğe bağlı alan eklenebilir:

```json
{
  "firstName": "Ada",
  "lastName": "Yilmaz",
  "email": "ada@example.com",
  "password": "ExamplePass1!",
  "confirmPassword": "ExamplePass1!",
  "address": {
    "title": "Ev",
    "city": "Istanbul",
    "district": "Kadikoy",
    "addressLine": "Ornek Mahallesi, Ornek Sokak No: 10 Daire: 2"
  }
}
```

Adres alanı gönderilmezse veya `null` ise kayıt normal devam eder. Gönderilen adres eksik/geçersizse kullanıcı kaydı da yapılmaz. Kayıt sonrasında login olup `/api/addresses` üzerinden adresler yönetilebilir.

## Hatalar ve sahiplik

- `400`: Geçersiz alanlar veya eksik/geçersiz CSRF token'ı.
- `401`: Giriş yapılmamış/geçersiz oturum.
- `403`: Kullanıcının customer rolü yok.
- `404`: Adres bulunamadı veya başka kullanıcıya ait.

Başkasının adresini okumak, güncellemek ve silmek aynı şekilde `404` döner. DTO'larda `userId` bulunmaz; sahiplik sorguda hem adres kimliği hem oturumdaki kullanıcı kimliği ile kontrol edilir.

Adres kayıtları, `Order.DeliveryAddress` alanından bağımsızdır. Sipariş oluşturma aşamasında bu alana adres metni kopyalanacak; kayıtlı adresin daha sonra değiştirilmesi veya silinmesi geçmiş sipariş adreslerini değiştirmeyecek.
