using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Notification_Class
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public Guid NotificationId { get; set; }
        public Guid ClassId { get; set; }
        public virtual Notification? Notification { get; set; }
        public virtual Class? Class { get; set; }
    }
}
