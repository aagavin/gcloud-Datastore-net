using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;

namespace AaronForm
{
    class Tasks
    {
        private List<Task> taskList;
        private string _projectId;
        private DatastoreDb _db;

        public Tasks()
        {
            string fullPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\COMP-306-50ff31942be8.json");
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", fullPath);
            _projectId = "comp-306";
            this._db = DatastoreDb.Create(_projectId);

            taskList = new List<Task>();

        }


        public List<Task> GetTasks()
        {

            Query query = new Query("Task");
            foreach (Entity entity in _db.RunQueryLazily(query))
            {
                taskList.Add(new Task(entity.Key.Path[0].Id, entity["CreatedBy"].StringValue, entity["Description"].StringValue, entity["Done"].BooleanValue));
            }

            return taskList;
        }

        public void UpdateTask(long taskId, string colToUpdate, string value)
        {
            Task a = this.taskList.First(t => t.Id == taskId);
            PropertyInfo propertyInfo = a.GetType().GetProperty(colToUpdate);
            propertyInfo.SetValue(a, Convert.ChangeType(value, propertyInfo.PropertyType), null);

            Entity task = new Entity() {
                Key = this._db.CreateKeyFactory("Task").CreateKey(a.Id),
                ["CreatedBy"] = a.CreatedBy,
                ["Description"] = a.Description,
                ["Done"] = a.Done
            };
            this._db.Update(task);

        }

    }
}
