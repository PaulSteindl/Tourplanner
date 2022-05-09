﻿using System;
using System.Collections.Generic;
using Tourplanner.Enums;

namespace Tourplanner.Models
{
    public class Log
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public DifficultyEnum Difficulty { get; set; }
        public int TotalTime { get; set; }
        public PopularityEnum Rating { get; set; }
    }
}
