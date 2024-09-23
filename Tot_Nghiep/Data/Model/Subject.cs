﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Subject
    {
        public Guid Id { get; set; } 

        [StringLength(100, ErrorMessage = "Name ko quá 100 ký tự")] 
        public string Name { get; set; } 

        [StringLength(30, ErrorMessage = "Code ko quá 30 ký tự")] 
        public string Code { get; set; } 
        public DateTime CreationTime { get; set; } 
        public int Status { get; set; } 
        public virtual ICollection<Subject_Grade> Subject_Grade { get; set; } 
        public virtual ICollection<Test> Test { get; set; } 
        public virtual ICollection<Exam> Exam { get; set; } 
        public virtual ICollection<Teacher_Subject> Teacher_Subject { get; set; } 
    }
}


