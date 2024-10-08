using AutoMapper;
using Data.DTOs;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping
{
    public class MappingTestQuestion: Profile
    {
        public MappingTestQuestion()
        {
            CreateMap<TestQuestion, TestQuestionDTO>().ReverseMap();
        }
        
    }
}
