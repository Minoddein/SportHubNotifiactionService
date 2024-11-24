# SportHub Notification Service
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)

Сервис уведомлений для проекта SportHub

## Возможности:
- [x] Позволяет пользователям подписываться на уведомления о предстоящих событиях
- [x] Автоматически отправляет уведомления за 2 месяца, 1 неделю и 2 дня до события
- [x] Поддерживает кастомизированные уведомления по запросам пользователей

## Стек:
- C# — язык программирования
- ASP.NET Core — веб-фреймворк для разработки REST API
- Entity Framework Core 8 — ORM для работы с базой данных
- [Hangfire](https://github.com/HangfireIO/Hangfire/) ^1.18.5 — библиотека для планирования и выполнения фоновых задач
- [Serilog](https://github.com/Serilog/serilog) ^4.11.0 — библиотека для логгирования
- [MailKit](https://github.com/jstedfast/MailKit) ^4.0.0 — библиотека для отправки электронной почты
- [Npgsql](https://github.com/npgsql/npgsql) ^8.0.0 — провайдер для подключения к PostgreSQL

## Установка и запуск

### Посредством Docker

1. Установите и настройте [Docker](https://www.docker.com/), как указано в документации.
2. Из папки проекта выполните команду:
```shell
docker build -t sporthub-notification-service .
```
3. Теперь запускать сервис можно командой:
```shell
docker run -it -d sporthub-notification-service
```

### Без использования Docker

1. Убедитесь, что у вас установлен [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
2. Создайте файл `appsettings.json` на основе `appsettings.template.json` и присвойте переменным соответствующие значения.
3. Выполните миграции базы данных:
```shell
dotnet ef database update
```
4. Теперь запускать сервис можно командой:
```shell
dotnet run --project src/SportHubNotificationService
```

## Конфигурация

Основные настраиваемые параметры:

- Подключение к базе данных (PostgreSQL)
- Настройки SMTP для отправки email-уведомлений
- Интервалы напоминаний (2 месяца, 1 неделя, 2 дня)

Все параметры конфигурации находятся в файле `appsettings.json`.
