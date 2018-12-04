using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeGrab.Models
{
    public class NewsModel
    {
       
        private string _content;
        public int Id { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }

        public int TypeId { get; set; }
        public int CustomerId { get; set; }
        
        public DateTime DateCreate { get; set; }



        public DateTime Dateupdate { get; set; }

        public Nullable<bool> IsPost { get; set; }
        public Nullable<bool> IsActive { get; set; }

    }
}