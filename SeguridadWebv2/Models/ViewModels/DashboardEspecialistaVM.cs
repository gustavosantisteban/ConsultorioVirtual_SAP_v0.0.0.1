﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.ViewModels
{
    public class DashboardEspecialistaVM
    {
        public ICollection<TurnosViewModel> TurnosViewModel { get; set; }
    }
}