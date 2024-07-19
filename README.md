# SkillsManagement

## Описание

SkillsManagement - это приложение для управления навыками, построенное с использованием ASP.NET Core и Entity Framework Core. Это приложение позволяет вам управлять данными о людях и их навыках через API.

## Структура проекта

Проект состоит из следующих основных частей:

- `Application`: Логика приложения, включающая в себя сервисы.
- `Domain`: Доменные модели.
- `Infrastructure`: Контекст базы данных и миграции.

## Запуск приложения

### Требования

- [Docker](https://www.docker.com/get-started)

### Запуск в Docker

1. Клонируйте репозиторий:

    ```sh
    git clone https://github.com/koliashka/SkillsManagement.git
    cd SkillsManagement
    ```

2. Запустите приложение и базу:

    ```sh
    docker-compose up -d
    ```
    Приложение будет доступно по адресу http://localhost:8080/swagger


## API Документация

После запуска приложения вы можете получить доступ к Swagger UI для тестирования API:


## Основные функции

- **Получение всех людей:** `GET /api/v1/persons`
- **Получение человека по ID:** `GET /api/v1/persons/{id}`
- **Создание нового человека:** `POST /api/v1/persons`
- **Обновление человека:** `PUT /api/v1/persons/{id}`
- **Удаление человека:** `DELETE /api/v1/persons/{id}`

