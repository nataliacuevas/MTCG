﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    
    internal class Package
    {
        public int Id { get; set; }
        public string Card1Id { get; set; }
        public string Card2Id { get; set; }
        public string Card3Id { get; set; }
        public string Card4Id { get; set; }
        public string Card5Id { get; set; }
    }
}
