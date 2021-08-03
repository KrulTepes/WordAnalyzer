# WordAnalyzer



## Настройка базы
Проект подговлен для использования PostgreSQL

#### Создание базы
```sql
CREATE DATABASE word_analyzer
    WITH
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;
```

#### Создание таблицы
```sql
CREATE TABLE statistics
(
    StatisticsId UUID,
    Date TIMESTAMP WITH TIME ZONE,
    JsonData JSON
);
```

После в `Properties\wordAnalyzerSettings.json` изменяем **ConnectionString** в соответствии с настройками подготовленной базой.
