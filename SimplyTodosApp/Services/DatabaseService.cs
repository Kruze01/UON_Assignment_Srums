using SQLite;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    async System.Threading.Tasks.Task Init()
    {
        if (_database is not null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyTasks.db");

        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<Task>();
    }

    public async Task<List<Task>> GetTasksAsync()
    {
        await Init();
        return await _database.Table<Task>().ToListAsync();
    }

    public async Task<int> SaveTaskAsync(Task task)
    {
        await Init();
        if (task.Id != 0)   //If task has Id, it's existing task
            return await _database.UpdateAsync(task);   //If existing task, update
        else
            return await _database.InsertAsync(task);   //If new task, insert
    }

    public async Task<int> DeleteTaskAsync(Task task)
    {
        await Init();
        return await _database.DeleteAsync(task);
    }
}