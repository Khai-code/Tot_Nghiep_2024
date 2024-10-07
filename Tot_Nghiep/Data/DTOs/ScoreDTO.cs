﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class ScoreDTO
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid PointTypeId { get; set; }
        public double Scores { get; set; }
        public string SubjectName { get; set; }
        public string PointTypeName { get; set; }
    }
}