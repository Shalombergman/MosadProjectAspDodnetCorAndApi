﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using MosadApiServer.Enums;

namespace MosadApiServer.Models
{
    public class Target
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }    
        public string? photo_url { get; set; }
        public TargetStatuses? status { get; set; }
        public Coordinates coordinate { get; set; }
       
        
    }
}

