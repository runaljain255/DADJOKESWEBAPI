using System;
namespace DadJokesAPIProject
{
	public class ApiSearchResponse
    {
        public int Current_Page { get; set; }
        public int Limit { get; set; }
        public int Next_Page { get; set; }
        public int Previous_Page { get; set; }
        public List<BaseApiResponse>? Results { get; set; }
        public string? Search_Term { get; set; }
        public int Status { get; set; }
        public int Total_Jokes { get; set; }
        public int Total_Pages { get; set; }
    }
}

