# Web API для игры в "Крестики-нолики"

## Запуск проекта
```
docker-compose up --build
```
Для удаления базы данных используйте `docker-compose down -v`

## Архитектура

Проект реализован в виде ASP.NET Core Web API с применением принципов SOLID, с использованием паттерна Repository. Все ключевые задачи, касающиеся идемпотентности и конкурентного доступа, сохранения состояния игры, persistence crash-safe успешно реализованы.
Используется ETag для реализации механизма проверки состояния клиента. Конфигурация приложения выполняется через переменные окружения `BOARD_SIZE` (размер поля) и `WIN_CONDITION` (количество символов одного типа в ряд для победы). Реализовано взаимодействие с базой данной SQLite с использованием Entity Framework и механизма миграций.

## Unit-тесты

Реализованы Unit-тесты с использованием xUnit. Для генерации отчета о покрытии тестами выполните:
```
dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"./TestResults/*/coverage.cobertura.xml" -targetdir:"./CoverageReport" -reporttypes:Html
```
Отчет о покрытии кода тестами будет в папке `CoverageReport` проекта `TickTackToe.Tests`. В данный момент покрытие кода тестами 64%:

<img width="631" height="482" alt="image" src="https://github.com/user-attachments/assets/5502447d-a81b-42f8-b41d-36aef7f6f955" />

## Основные эндпоинты:

- `/health (возвращает 200)(GET)`

- `/games (создание новой игры)(POST)`

- `/games/{id} (информация об определенной игре)(GET)`

- ```
  /games/{id}/move (сделать ход) (PUT)

  {
    "player": "O",
    "row": 2,
    "column": 2
  }
