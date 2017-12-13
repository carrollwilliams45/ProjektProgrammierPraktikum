﻿using AppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppData
{
    public interface IBeamer
    {
        void Add(Beamer newBeamer);
        IEnumerable<Beamer> GetAll();
        Beamer GetById(int id);
        IEnumerable<Beamer> GetAvailableBeamers();
    }
}
