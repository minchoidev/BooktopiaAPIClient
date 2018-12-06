using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Text;

namespace BooktopiaApiClient
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            ServiceBooktopia_RunAsync().GetAwaiter().GetResult();
        }


        // Main controller
        static async Task ServiceBooktopia_RunAsync()
        {
            // Note: Uri only recognize the base domain. If it has "domain/additionalPart", then it ignores the part after '/'
            // client.BaseAddress = new Uri(@"http://booktopiaapiservice-dev.us-east-1.elasticbeanstalk.com");  // Elastic Bean Stalk
            //client.BaseAddress = new Uri(@"https://mchoi34-eval-test.apigee.net");                              // Apigee Proxy
            client.BaseAddress = new Uri(@"https://localhost:44346");                              // Local
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API key to verify
            client.DefaultRequestHeaders.Add("booktopia-apikey", "V7awRZcors3ONC1GkYxU2qYyTiYdAbg1");
            // client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6,ru;q=0.4");

            // as long as 'isRunning' is true, the application continues
            bool isRunning = true;

            Console.WriteLine("Welcome to Booktopia API Client!!");
            while (isRunning)
            {
                try
                {
                    Console.WriteLine("\n\n\nPlease selet an option in the following menu.");

                    Console.WriteLine("\t1. Display all titles\n\t2. Search titles\n\t3. Search Top 10 popular titles" +
                        "\n\t4. Search Top 5 popular genres\n\t5. Search Top 5 popular authors\n\t6. Add a new title" +
                        "\n\t7. Edit a title\n\t8. Delete a title\n\t9. Exit");
                    Console.Write("Option#: ");
                    int optNum = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("\n\n\n");

                    // executing proper method depends on the input
                    switch (optNum)
                    {
                        case 1:
                            await GetAllTitlesAsync();
                            break;
                        case 2:
                            await SearchTitlesOperatorAsync();
                            break;
                        case 3:
                            await SearchPopularTitlesAsync();
                            break;
                        case 4:
                            await SearchPopularGenresAsync();
                            break;
                        case 5:
                            await SearchPopularAuthorsAsync();
                            break;
                        case 6:
                            await AddTitleAsync();
                            break;
                        case 7:
                            await UpdateTitleAsync();
                            break;
                        case 8:
                            await DeleteTitleAsync();
                            break;
                        case 9:
                            isRunning = false;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Please enter Enter key to end...");
            Console.ReadLine();
        }

        static async Task GetAllTitlesAsync()
        {
            Console.WriteLine("All Titles in Booktopia:\n");
            // get all titles
            HttpResponseMessage response = await client.GetAsync("/api/titles");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }

        #region Search Titles by values
        // this method controls all searching books methods
        static async Task SearchTitlesOperatorAsync()
        {
            bool keepSearching = true;

            while (keepSearching)
            {
                try
                {
                    // choose the searching way
                    Console.WriteLine("\n\n\nPlease select a searching way to find books.");
                    Console.WriteLine("\t1. Title\n\t2. ISBN\n\t3. Author\n\t4. Publisher" +
                        "\n\t5. Genre\n\t6. Published Date\n\t7. Price\n\t8. Title_Id\n\t9. Exit");
                    Console.Write("Option#: ");
                    int optNum = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("\n\n");

                    // executing proper method depends on the input
                    switch (optNum)
                    {
                        case 1:
                            await SearchTitlesByTitleAsync();
                            break;
                        case 2:
                            await SearchTitlesByIsbnAsync();
                            break;
                        case 3:
                            await SearchTitlesByAuthorAsync();
                            break;
                        case 4:
                            await SearchTitlesByPublisherAsync();
                            break;
                        case 5:
                            await SearchTitlesByGenreAsync();
                            break;
                        case 6:
                            await SearchTitlesByPubDateAsync();
                            break;
                        case 7:
                            await SearchTitlesByPricesAsync();
                            break;
                        case 8:
                            await SearchTitleByIdAsync();
                            break;
                        case 9:
                            keepSearching = false;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        static async Task SearchTitlesByTitleAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter a book title.");
            Console.Write("\tTitle: ");
            string title = Console.ReadLine();

            HttpResponseMessage response = await client.GetAsync("/api/titles/title/" + title);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchTitlesByIsbnAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter a book ISBN.");
            Console.Write("\tISBN: ");
            string isbn = Console.ReadLine();

            // ISBN Regex
            Match match = Regex.Match(isbn, @"^[0-9]{13}$");
            if (!match.Success)
            {
                throw new Exception("ISBN must be 13 number of digits.");
            }

            HttpResponseMessage response = await client.GetAsync("/api/titles/isbn/" + isbn);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchTitlesByAuthorAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter a book author.");
            Console.Write("\tAuthor: ");
            string author = Console.ReadLine();

            HttpResponseMessage response = await client.GetAsync("/api/titles/author/" + author);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchTitlesByPublisherAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter a book publisher.");
            Console.Write("\tPublisher: ");
            string publisher = Console.ReadLine();

            HttpResponseMessage response = await client.GetAsync("/api/titles/publisher/" + publisher);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchTitlesByGenreAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter a book genre.");
            Console.Write("\tGenre: ");
            string genre = Console.ReadLine();

            HttpResponseMessage response = await client.GetAsync("/api/titles/genre/" + genre);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchTitlesByPubDateAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter the range of published date");
            Console.Write("\tStart date (yyyy-MM-dd): ");
            var startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.Write("\tEnd date (yyyy-MM-dd): ");
            var endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("/api/titles/pubdate/"
                + startDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + '/'
                + endDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchTitlesByPricesAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter the range of price");
            Console.Write("\tMinimum price: ");
            double minPrice = Convert.ToDouble(Console.ReadLine());
            Console.Write("\tMaximum price: ");
            double maxPrice = Convert.ToDouble(Console.ReadLine());

            HttpResponseMessage response = await client.GetAsync("/api/titles/price/" + minPrice + '/' + maxPrice);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                IEnumerable<Title> titles = JsonConvert.DeserializeObject<IEnumerable<Title>>(json);

                Console.WriteLine("\n\n");
                Title.PrintTitleTableColumns();
                foreach (Title t in titles)
                {
                    Console.WriteLine(t);
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchTitleByIdAsync()
        {
            // search title with a keyword
            Console.WriteLine("Please enter a title's id.");
            Console.Write("\tTitle_id#: ");
            int id = Convert.ToInt32(Console.ReadLine());
            await SearchTitleByIdAsync(id);
        }
        static async Task SearchTitleByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync("/api/titles/" + id);

            if (response.IsSuccessStatusCode)
            {
                Title title = await response.Content.ReadAsAsync<Title>();
                title.PrintSingleTitle();
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        #endregion

        #region Search Popular Items
        static async Task SearchPopularAuthorsAsync()
        {
            Console.WriteLine("Search Top 5 Authors");
            Console.WriteLine("Please enter the period and count");
            Console.Write("\tStart date (yyyy-MM-dd): ");
            var startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.Write("\tEnd date (yyyy-MM-dd): ");
            var endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("/api/titles/top/authors/"
                + startDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + '/'
                + endDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "/5");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var j = JArray.Parse(json);

                // table header
                Console.WriteLine($"{String.Format("{0, -10}", "Rank")}" +
                    $"{String.Format("{0, -30}", "Author")}" +
                    $"{String.Format("{0, -10}", "Count")} ");

                // print a line
                StringBuilder sbLine = new StringBuilder();
                for (int i = 0; i < 60; i++)
                {
                    sbLine.Append('=');
                }
                Console.WriteLine(sbLine);

                // display top titles list
                // json object structure: { string author, string count }
                int rank = 1;
                foreach (var v in j)
                {
                    // deserialize to Title and Int count
                    string author = v.SelectToken("author").ToString();
                    string count = v.SelectToken("count").ToString();
                    Console.WriteLine($"{String.Format("{0, -10}", rank)}" +
                        $"{String.Format("{0, -30}", author)}" +
                        $"{String.Format("{0, -10}", count)}");
                    rank++;
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchPopularGenresAsync()
        {
            Console.WriteLine("Search Top 5 Genres");
            Console.WriteLine("Please enter the period and count");
            Console.Write("\tStart date (yyyy-MM-dd): ");
            var startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.Write("\tEnd date (yyyy-MM-dd): ");
            var endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("/api/titles/top/genres/"
                + startDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + '/'
                + endDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "/5");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var j = JArray.Parse(json);

                // table header
                Console.WriteLine($"{String.Format("{0, -10}", "Rank")}" +
                    $"{String.Format("{0, -30}", "Genre")}" +
                    $"{String.Format("{0, -10}", "Count")} ");

                // print a line
                StringBuilder sbLine = new StringBuilder();
                for (int i = 0; i < 60; i++)
                {
                    sbLine.Append('=');
                }
                Console.WriteLine(sbLine);

                // display top titles list
                // json object structure: { string genre, string count }
                int rank = 1;
                foreach (var v in j)
                {
                    // deserialize to Title and Int count
                    string genre = v.SelectToken("genre").ToString();
                    string count = v.SelectToken("count").ToString();
                    Console.WriteLine($"{String.Format("{0, -10}", rank)}" +
                        $"{String.Format("{0, -30}", genre)}" +
                        $"{String.Format("{0, -10}", count)}");
                    rank++;
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        static async Task SearchPopularTitlesAsync()
        {
            Console.WriteLine("Search Top 10 titles");
            Console.WriteLine("Please enter the period and count");
            Console.Write("\tStart date (yyyy-MM-dd): ");
            var startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.Write("\tEnd date (yyyy-MM-dd): ");
            var endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            HttpResponseMessage response = await client.GetAsync("/api/titles/top/"
                + startDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + '/'
                + endDate.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + "/10");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var j = JArray.Parse(json);

                // table header
                Console.WriteLine($"{String.Format("{0, -10}", "Rank")}" +
                    $"{String.Format("{0, -10}", "Title_id")}" +
                    $"{String.Format("{0, -80}", "Title")}" +
                    $"{String.Format("{0, -10}", "Count")} ");

                // print a line
                StringBuilder sbLine = new StringBuilder();
                for (int i = 0; i < 120; i++)
                {
                    sbLine.Append('=');
                }
                Console.WriteLine(sbLine);

                // display top titles list
                // json object structure: { Title, COUNT }
                int rank = 1;
                foreach (var v in j)
                {
                    // deserialize to Title and Int count
                    var title = JsonConvert.DeserializeObject<Title>(v.SelectToken("title").ToString());
                    string count = v.SelectToken("count").ToString();
                    Console.WriteLine($"{String.Format("{0, -10}", rank)}" +
                        $"{String.Format("{0, -10}", title.TitleId)}" +
                        $"{String.Format("{0, -80}", title.Title1)}" +
                        $"{String.Format("{0, -10}", count)}");
                    rank++;
                }
            }
            else
                Console.WriteLine("Internal Server Error");
        }
        #endregion

        #region Add, Update, Delete Titles
        static async Task AddTitleAsync()
        {
            Console.WriteLine("Add Title");
            Console.WriteLine("Please fill out the format of Title to add.\n");

            // create a new title
            Title title = new Title();

            Console.Write("Title: ");
            title.Title1 = Console.ReadLine();

            Console.Write("ISBN: ");
            title.Isbn = Console.ReadLine();
            // ISBN Regex
            Match match = Regex.Match(title.Isbn, @"^[0-9]{13}$");
            if (!match.Success)
            {
                throw new Exception("ISBN must be 13 number of digits.");
            }

            Console.Write("Author: ");
            title.Author = Console.ReadLine();
            Console.Write("Publisher: ");
            title.Publisher = Console.ReadLine();
            Console.Write("Genre: ");
            title.Genre = Console.ReadLine();
            Console.Write("Published Date (yyyy-MM-dd): ");
            title.PubDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.Write("Price: ");
            title.Price = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("\n");

            // add a title
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/titles", title);

            // display result
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine($"The title has been added.");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }


        static async Task UpdateTitleAsync()
        {
            Console.WriteLine("Update Title");
            Console.WriteLine("Please fill out the format of Title to update.\n");

            // create a new title
            Title title = new Title();

            Console.Write("Title ID: ");
            title.TitleId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Title: ");
            title.Title1 = Console.ReadLine();

            Console.Write("ISBN: ");
            title.Isbn = Console.ReadLine();
            // ISBN Regex
            Match match = Regex.Match(title.Isbn, @"^[0-9]{13}$");
            if (!match.Success)
            {
                throw new Exception("ISBN must be 13 number of digits.");
            }

            Console.Write("Author: ");
            title.Author = Console.ReadLine();
            Console.Write("Publisher: ");
            title.Publisher = Console.ReadLine();
            Console.Write("Genre: ");
            title.Genre = Console.ReadLine();
            Console.Write("Published Date (yyyy-MM-dd): ");
            title.PubDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.Write("Price: ");
            title.Price = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("\n");

            // update a title
            HttpResponseMessage response = await client.PutAsJsonAsync("/api/titles/" + title.TitleId, title);

            // display result
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"The title {title.TitleId} has been updated.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"The title {title.TitleId} does not exist.");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        static async Task DeleteTitleAsync()
        {
            Console.WriteLine("Delete Title");
            Console.WriteLine("Please enter a Title Id to delete the book.\n");
            int titleId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\n");

            // delete the title
            HttpResponseMessage response = await client.DeleteAsync("/api/titles/" + titleId);

            // display result
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"The title {titleId} has been deleted.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"The title {titleId} does not exist.");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }

        #endregion

    }
}
