﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Core
{
    public class Subgroup : IGroup
    {
        #region PROPERTIES

        public string Name { get; set; }

        #endregion



        #region CONSTRUCTORS

        public Subgroup()
        {
            Name = string.Empty;
        }

        #endregion
    }
}
