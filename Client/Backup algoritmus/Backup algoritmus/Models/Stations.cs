﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Backup_algoritmus
{
    internal class Stations
    {
        public int Id { get; set; }
        public int[] configIds { get; set; }
        public string[] configs { get; set; }
        public string Mac { get; set; }
        public string Ip { get; set; }
        public string Alias { get; set; }
        public bool Verified { get; set; }
        public bool Ban { get; set; }
    }
}
