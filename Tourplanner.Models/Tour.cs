﻿using System;
using System.Collections.Generic;

namespace Tourplanner.Models
{
    public class Tour
    {
        public string Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public string From { get; set; } = String.Empty;
        public string To { get; set; } = String.Empty;
        public TransportType Transporttype { get; set; }
        public double Distance { get; set; }
        public int Time { get; set; }
        public string PicPath { get; set; } = String.Empty;
        public PopularityEnum? Popularity { get; set; }
        public bool ChildFriendly { get; set; }
        public List<Log>? Logs { get; set; }
    }
}
