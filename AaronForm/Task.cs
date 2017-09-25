using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaronForm
{
    class Task
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }

        public Task(long id, string createdBy, string description, bool? done=false)
        {
            Id = id;
            CreatedBy = createdBy;
            Description = description;
            Done = done!=null? true: false;
        }
    }
}
