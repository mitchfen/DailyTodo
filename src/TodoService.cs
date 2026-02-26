using System.Collections.Concurrent;
using System.Text.Json;

namespace DailyTodo
{
    public class TodoService
    {
        private readonly AppConfig _appConfig;
        private readonly string _filePath = "data/todo-state.json";
        // Key is date string (yyyy-MM-dd), Value is a map of TaskText -> IsCompleted
        private ConcurrentDictionary<string, ConcurrentDictionary<string, bool>> _dailyState = new();
        private readonly object _fileLock = new();

        public TodoService(AppConfig appConfig)
        {
            _appConfig = appConfig;
            LoadFromFile();
        }

        public bool GetState(DateTime date, string taskText)
        {
            CleanupOldDates();
            var dateKey = date.ToString("yyyy-MM-dd");
            if (_dailyState.TryGetValue(dateKey, out var tasks))
            {
                return tasks.TryGetValue(taskText, out var isCompleted) && isCompleted;
            }
            return false;
        }

        public void SetState(DateTime date, string taskText, bool isCompleted)
        {
            CleanupOldDates();
            var dateKey = date.ToString("yyyy-MM-dd");
            var tasks = _dailyState.GetOrAdd(dateKey, _ => new ConcurrentDictionary<string, bool>());
            tasks[taskText] = isCompleted;
            SaveToFile();
        }

        public void CleanupOldDates()
        {
            var today = _appConfig.GetToday().ToString("yyyy-MM-dd");
            var keysToRemove = _dailyState.Keys.Where(k => k != today).ToList();
            
            if (keysToRemove.Any())
            {
                foreach (var key in keysToRemove)
                {
                    _dailyState.TryRemove(key, out _);
                }
                SaveToFile();
            }
        }

        private void LoadFromFile()
        {
            lock (_fileLock)
            {
                try
                {
                    if (File.Exists(_filePath))
                    {
                        var json = File.ReadAllText(_filePath);
                        var data = JsonSerializer.Deserialize<ConcurrentDictionary<string, ConcurrentDictionary<string, bool>>>(json);
                        if (data != null)
                        {
                            _dailyState = data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading state: {ex.Message}");
                    _dailyState = new();
                }
            }
            CleanupOldDates(); // Ensure we don't start with old data
        }

        private void SaveToFile()
        {
            lock (_fileLock)
            {
                try
                {
                    var directory = Path.GetDirectoryName(_filePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var json = JsonSerializer.Serialize(_dailyState);
                    File.WriteAllText(_filePath, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving state: {ex.Message}");
                }
            }
        }
    }
}
