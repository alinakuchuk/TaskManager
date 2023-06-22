# TaskManager

## How to run
To be able to query tasks only **TaskManager.Api** should be run.

To be able to modify tasks **TaskManager.Api** should be run (it sends command messages to Azure Service Bus) **together** with **TaskManager.WorkerService** (it listens to Azure Service Bus queues and executes tasks modification).

## How to test

To be able to test **TaskManager** follow the instructions below.

1. To get all tasks by applying 'dueDateTime' and 'isDone' filter parameters call:

**QueryTasks**

Body:
{
    "dueDateTime": {
        "value": "2023-10-10"
    },
    "isDone": {
        "value": true
    },
    "limit": 10,
    "offset": 0
}

2. To get one task by 'id' parameter call:

**QueryById**

Body:
{
    "id": "c8a79632-d566-4fb8-a044-81f24a9e7c3e"
}

3. To create a new task call:

**CreateTask**

Body:
{
    "task":
    {
        "dueDateTime": "2023-09-09",
        "name" : “Your task name”,
        "description": "Your task description”,
        "isDone": true
    }
}

4. To update a task call:

**UpdateTask**

Body:
{
    "id": "c8a79632-d566-4fb8-a044-81f24a9e7c3e",
    "task":
    {
        "dueDateTime": "2023-09-09",
        "name" : “Updated name”,
        "description": “Updated description”,
        "isDone": true
    }
}

5. To delete a task call:

**DeleteTask**

Body:
{
    "id": "a496d40d-f2e2-4288-b28f-7201471675f7"
}
