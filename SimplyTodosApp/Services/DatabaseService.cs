using SQLite;
using SimplyTodosApp.Models;
using Task = SimplyTodosApp.Models.Task;

namespace SimplyTodosApp.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    async System.Threading.Tasks.Task Init()
    {
        try
        {
            if (_database is not null) 
                return;
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyTasks.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<Task>();
        }

        catch (Exception e)
        {
            // message to tell the user that loading database failed
            
        }
    }

    public async Task<List<Task>> GetTasksAsync()
    {
        await Init();
        return 
            await _database.Table<Task>().ToListAsync();
    }

    public async Task<int> SaveTaskAsync(Task task)
    {
        await Init();
        return 
            await _database.InsertAsync(task);
    }

    public async Task<int> DeleteTaskAsync(SimplyTodosApp.Models.Task task)
    {
        await Init();
        return 
            await _database.DeleteAsync(task);
    }
}