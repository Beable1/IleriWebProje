﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
	public abstract class BaseDto
	{
		public int Id { get; set; }
		public DateTime CreatedTime { get; set; }
	}
}
