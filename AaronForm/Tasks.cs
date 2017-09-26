using System;
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
        private KeyFactory _keyFactory;

        /// <summary>
        /// Manages DAO between application and Google Datastore
        /// </summary>
        public Tasks()
        {
            string fullPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\COMP-306-fad385355cc2.json");
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", fullPath);
            _projectId = "comp-306";
            this._db = DatastoreDb.Create(_projectId);
            this._keyFactory = this._db.CreateKeyFactory("Task");
            this.taskList = new List<Task>();

        }

        /// <summary>
        /// Gets a list of tasks
        /// </summary>
        /// <returns></returns>
        public List<Task> GetTasks()
        {

            Query query = new Query("Task");
            foreach (Entity entity in _db.RunQueryLazily(query))
            {
                taskList.Add(new Task(entity.Key.Path[0].Id, entity["CreatedBy"].StringValue, entity["Description"].StringValue, entity["Created"].TimestampValue.ToDateTime(), entity["Done"].BooleanValue));
            }

            return taskList;
        }

        /// <summary>
        /// removes a list of tasks by id
        /// </summary>
        /// <param name="ids"></param>
        public void deleteTasks(List<string> ids)
        {
            foreach (string id in ids)
            {
                Key key = this._keyFactory.CreateKey(long.Parse(id));
                this._db.Delete(key);
            }

        }

        /// <summary>
        /// adds a task 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void addTask(string name, string description)
        {
            Entity task = new Entity()
            {
                Key = this._keyFactory.CreateIncompleteKey(),
                ["Created"] = DateTime.UtcNow,
                ["Description"] = new Value()
                {
                    StringValue = description,
                    ExcludeFromIndexes = true
                },
                ["CreatedBy"] = name,
                ["Done"] = false
            };

            this._db.Insert(task);
        }


        /// <summary>
        /// replaces a task (update)
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="colToUpdate"></param>
        /// <param name="value"></param>
        public void UpdateTask(long taskId, string colToUpdate, string value)
        {
            Task a = this.taskList.First(t => t.Id == taskId);
            PropertyInfo propertyInfo = a.GetType().GetProperty(colToUpdate);
            propertyInfo.SetValue(a, Convert.ChangeType(value, propertyInfo.PropertyType), null);

            Entity task = new Entity() {
                Key = this._keyFactory.CreateKey(a.Id),
                ["CreatedBy"] = a.CreatedBy,
                ["Description"] = new Value() {
                    StringValue = a.Description,
                    ExcludeFromIndexes = true
                },
                ["Created"] = a.Created,
                ["Done"] = a.Done
            };
            this._db.Update(task);

        }

    }
}
