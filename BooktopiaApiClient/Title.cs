using System;
using System.Collections.Generic;
using System.Text;

namespace BooktopiaApiClient
{
    public class Title
    {
        public int TitleId { get; set; }
        public string Title1 { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public DateTime PubDate { get; set; }
        public decimal? Price { get; set; }

        public static void PrintTitleTableColumns()
        {
            Console.WriteLine($"{String.Format("{0, -10}", "Title_Id")}" +
                $"{String.Format("{0, -40}", "Title")} " +
                $"{String.Format("{0, -20}", "ISBN")} " +
                $"{String.Format("{0, -25}", "Author")} " +
                $"{String.Format("{0, -25}", "Publisher")} " +
                $"{String.Format("{0, -20}", "Genre")} " +
                $"{String.Format("{0, -20}", "Published Date")} " +
                $"Price");
            StringBuilder sbLine = new StringBuilder();
            for (int i = 0; i < 180; i++)
            {
                sbLine.Append('=');
            }
            Console.WriteLine(sbLine);
        }

        public override string ToString()
        {
            // truncate too long title
            string shortTitle = Title1;
            if (shortTitle.Length > 35)
            {
                shortTitle = shortTitle.Substring(0, 35);
            }

            // format ISBN
            string fIsbn = Isbn.Substring(0, 3) + '-' + Isbn.Substring(3);

            return $"{String.Format("{0, -10}", TitleId)}" +
                $"{String.Format("{0, -40}", shortTitle)} " +
                $"{String.Format("{0, -20}", fIsbn)} " +
                $"{String.Format("{0, -25}", Author)} " +
                $"{String.Format("{0, -25}", Publisher)} " +
                $"{String.Format("{0, -20}", Genre)} " +
                $"{String.Format("{0, -20}", PubDate.ToShortDateString())} " +
                $"{"$ " + String.Format("{0, 5}", String.Format("{0:n2}", Price))}";
        }
        public void PrintSingleTitle()
        {
            Console.WriteLine($"{String.Format("{0, -10}", "Title_Id")}" +
                $"{String.Format("{0, -40}", "Title")} " +
                $"{String.Format("{0, -20}", "ISBN")} " +
                $"{String.Format("{0, -25}", "Author")} " +
                $"{String.Format("{0, -25}", "Publisher")} " +
                $"{String.Format("{0, -20}", "Genre")} " +
                $"{String.Format("{0, -20}", "Published Date")} " +
                $"Price");

            // print a line
            StringBuilder sbLine = new StringBuilder();
            for (int i = 0; i < 180; i++)
            {
                sbLine.Append('=');
            }
            Console.WriteLine(sbLine);

            // truncate too long title
            string shortTitle = Title1;
            if (shortTitle.Length > 35)
            {
                shortTitle = shortTitle.Substring(0, 35);
            }

            // format ISBN
            string fIsbn = Isbn.Substring(0, 3) + '-' + Isbn.Substring(3);

            Console.WriteLine($"{String.Format("{0, -10}", TitleId)}" +
                $"{String.Format("{0, -40}", shortTitle)} " +
                $"{String.Format("{0, -20}", fIsbn)} " +
                $"{String.Format("{0, -25}", Author)} " +
                $"{String.Format("{0, -25}", Publisher)} " +
                $"{String.Format("{0, -20}", Genre)} " +
                $"{String.Format("{0, -20}", PubDate.ToShortDateString())} " +
                $"{"$ " + String.Format("{0, 5}", String.Format("{0:n2}", Price))}");
        }
    }
}
