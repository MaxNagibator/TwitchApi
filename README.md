# TwitchApi

**Описание:**
Инструмент для работы с Twitch API, позволяющий менять название трансляции и её категорию одним кликом.

**Особенности:**

* Авторизация через OAuth.
* Сессии сохраняются для повторного использования без повторной авторизации.
* Система профилей: можно сохранять наборы (название + категория) под именем и быстро переключать.

![image](https://github.com/user-attachments/assets/bf66478d-8842-4dc6-8a4c-5c62ea94b295)


## Настройка и запуск

Перед запуском нужно заполнить настройки в `appsettings.json` или через `.NET secrets`, пример конфигурации:

```json
"Twitch": {
  "ClientId": "YOUR_CLIENT_ID",
  "ClientSecret": "YOUR_CLIENT_SECRET",
  "RedirectUrl": "http://localhost:3000"
}
```

* `ClientId` и `ClientSecret` получаете, зарегистрировав приложение здесь: https://dev.twitch.tv/docs/authentication/register-app/

* `RedirectUrl` — URL, на который Twitch вернёт код авторизации. Для теста можно оставить `localhost` с любым портом

* Для публичного запуска нужно указать в `RedirectUrl` реальный URL вашего сервера и также в настройках приложения на https://dev.twitch.tv/console

---

## Пример обработки redirect URL

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", (HttpRequest req, HttpResponse res) =>
{
    var query = req.QueryString.Value;
    var redirectUrl = $"twitchapi://auth{query}";
    res.Redirect(redirectUrl);
    return Task.CompletedTask;
});

app.Run("https://twitchauth.bob217.ru");
```

Эта логика позволяет принимать redirect от Twitch и перенаправлять запрос в приложение по схеме
<br>
`twitchapi://auth?code=...`
