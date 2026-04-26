using SQLite;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyTasks.db");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Task>().Wait();
    }

    public async Task<List<Task>> GetTasksAsync()
    {
        return await _database.Table<Task>().ToListAsync();
    }

    public async Task<int> SaveTaskAsync(Task task)
    {
        if (task.Id != 0)   //If task has Id, it's existing task
            return await _database.UpdateAsync(task);   //If existing task, update
        else
            return await _database.InsertAsync(task);   //If new task, insert
    }

    public async Task<int> DeleteTaskAsync(Task task)
    {
        return await _database.DeleteAsync(task);
    }

    public async System.Threading.Tasks.Task DeleteAllAsync(IEnumerable<Task> tasks)
    {

        //Remove all selected tasks at once
        await _database.RunInTransactionAsync(trans =>
        {
            foreach (var task in tasks)
            {
                trans.Delete(task);
            }
        });
    }
}