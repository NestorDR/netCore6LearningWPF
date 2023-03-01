using System.Collections.Generic;
// --- App modules ---
using LearningWPF.Models;

namespace LearningWPF.Services
{
    internal class TaskService
    {
        public IEnumerable<TaskModel> Get()
        {
            return new List<TaskModel>
            {
                new () { Id = 1, Name = "Learning how to develop", Completion = 30 },
                new () { Id = 2, Name = "Learn C#", Completion = 50 },
                new () { Id = 3,  Name = "Learning WPF", Completion = 75 },
                new () { Id = 4, Name = "Wash the car", Completion = 85 },
                new () { Id = 5, Name = "Buy beer", Completion = 90 },
                new () { Id = 6, Name = "Enjoy it!", Completion = 100 }
            };
        }
    }
}
