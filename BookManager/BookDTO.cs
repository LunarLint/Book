using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManager
{
    public class BookDTO
    {
        //책
        public int Isbn { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int Page { get; set; }

        //사람
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string isBorrowed { get; set; }
        public DateTime BorrowedAt { get; set; }
    }
}
