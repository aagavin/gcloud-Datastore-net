using System;
using System.Collections;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;
using System.Windows.Forms;

namespace AaronForm
{
    class Tasks
    {
        string _projectId;
        DatastoreDb _db;
        public Tasks()
        {
            string fullPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\COMP-306-50ff31942be8.json");
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", fullPath);
            _projectId = "comp-306";
            this._db = DatastoreDb.Create(_projectId);

        }


        public List<Task> GetTasks()
        {
            List<Task> taskList = new List<Task>();

            Query query = new Query("Task");
            foreach (Entity entity in _db.RunQueryLazily(query))
            {
                taskList.Add(new Task(entity.Key.Path[0].Id, entity["CreatedBy"].StringValue, entity["Description"].StringValue, entity["Done"].BooleanValue));
            }

            return taskList;
        }
    }
}
