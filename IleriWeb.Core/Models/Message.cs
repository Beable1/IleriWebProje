using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Models
{
    class Message
    {
		public class MessageRequest
		{
			public string Content { get; set; }
		}

		public class MessageResponse
		{
			public string Status { get; set; }
		}
	}
}
