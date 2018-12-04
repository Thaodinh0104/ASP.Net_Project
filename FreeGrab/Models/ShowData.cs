using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeGrab.Models
{
    public class ShowData
    {

        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();

        public List<Patient> patients { get; set; }
        public List<News> news { get; set; }
        public List<News> newspost { get; set; }
        public List<News> notpost { get; set; }
        public News newDetail { get; set; }
        public List<Comment> comments { get; set; }
        
    }
}