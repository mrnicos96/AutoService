# AutoService
* О сервисе. 

Этот сервис предоставляет ежемесячные отчеты в различных форматах через REST API.

* Используемые технологии

Сервис работает на основе ASP NET Core. База данных контейнезирована в докер. Данные можно получить в формате JSON, csv, excel отчета.
Так же в шаблоне /src/AutoServiceReportTemplate.xlsm  реализована возможность выгрузки данных из сервиса в шаблон.

* Методы доступа.

Получить отчет в JSON формате
 
GET /api/Reports/monthly?month={month}&year={year}

Получить отчет в CSV формате

GET /api/Reports/monthly/csv?month={month}&year={year}

Получить отчет в Excel формате

GET /api/Reports/monthly/excel?month={month}&year={year}

Получить отчет из шаблона AutoServiceReportTemplate.xlsm 

Запуск шаблона AutoServiceReportTemplate.xlsm 

Структура сервиса

src/
  Api/            # Web API слой
  Application/	  # Бизнес-логика
  Core/     	  # Модели и интерфейсы
  Infrastructure/ # Реализации репозиториев, DB контекст

* Дополнительно.

листинги скриптов к базе данных лежат в папке database