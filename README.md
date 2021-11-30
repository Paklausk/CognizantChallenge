# CognizantChallenge
[![GitHub](https://img.shields.io/github/license/Paklausk/CognizantChallenge?style=for-the-badge)](https://github.com/Paklausk/CognizantChallenge/blob/master/LICENSE)
[![GitHub last commit](https://img.shields.io/github/last-commit/Paklausk/CognizantChallenge.svg?style=for-the-badge)]()

 Cognizant challenge for Full-Stack .NET developer position.
 
## Installation

1. Download repository;
2. Validate database connection string in appsettings.json file;
3. Download all nuget packages;
4. Run command 'Update-Database' in Package Manager Console to create database at your MS Sql database server;
5. Build and run Project with IIS Express;
6. Enjoy.

## Todo
* Clean up code;
* Add more unit tests;
* Create better errors handling at backend;
* Polish frontend;

## Test data samples

**Request**

`GET` `https://localhost/challenge/getTasksList`

**Response**

```json
[
    {
        "id": 1,
        "name": "Python add",
        "description": "Write in Python function body, which takes its parameters, adds them and returns a result",
        "functionHeader": "def fnc(val1, val2):",
        "functionFooter": "    return result"
    },
    {
        "id": 2,
        "name": "Python modulo",
        "description": "Write in Python function body, which takes its parameters, performs modulo operation on them and returns a result",
        "functionHeader": "def fnc(val1, val2):",
        "functionFooter": "    return result"
    },
    {
        "id": 3,
        "name": "Python subtract",
        "description": "Write in Python function body, which takes its parameters, subtracts them and returns a result",
        "functionHeader": "def fnc(val1, val2):",
        "functionFooter": "    return result"
    }
]
```

**Request**

`POST` `https://localhost/challenge/submitTask`

```json
{
    "taskId": 1,
    "developersName": "Name",
    "code": "result = val1 + val2"
}
```

**Response**

```json
{
    "success": true,
    "error": null
}
```
