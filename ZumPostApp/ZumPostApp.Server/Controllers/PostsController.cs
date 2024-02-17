using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZumPostApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet("{tag}")]
        public async Task<IActionResult> GetTag(string tag, string sortBy = null, string direction = null)
        {
            var posts = await _postRepository.GetPostsAsync(tag, sortBy, direction);

            if (posts == null)
                return NotFound();

            return Ok(posts);
        }
    }

    public class Post
    {
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public int Id { get; set; }
        public int Likes { get; set; }
        public double Popularity { get; set; }
        public int Reads { get; set; }
        public string[] Tags { get; set; }
    }

    public interface IPostRepository
    {
        Task<List<Post>> GetPostsAsync(string tag, string sortBy, string direction);
    }

    public class ThirdPartyApiRepo : IPostRepository
    {
        private readonly HttpClient _httpClient;

        public ThirdPartyApiRepo(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.hatchways.io/assessment/blog/posts");
        }


        public async Task<List<Post>> GetPostsAsync(string tag, string sortBy, string direction)
        {
            // set default value for sortBy = id, direction = asc 

            var apiURL = $"{_httpClient.BaseAddress}?tag={tag}";

            var response = await _httpClient.GetAsync(apiURL);
            sortBy = string.IsNullOrEmpty(sortBy) ? sortBy = "id" : sortBy;
            direction = string.IsNullOrEmpty(direction) ? direction = "asc" : direction;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<FilteredPosts>(responseContent)?.Posts;
                
                // sort result posts by direction and 
                // Determine sorting order
                bool descending =  direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

                // Sort posts based on user-selected property
                switch (sortBy.ToLower())
                {
                    case "id":
                        posts = descending ? posts?.OrderByDescending(p => p.Id).ToList() : posts?.OrderBy(p => p.Id).ToList();
                        break;
                    case "reads":
                        posts = descending ? posts?.OrderByDescending(p => p.Reads).ToList() : posts?.OrderBy(p => p.Reads).ToList();
                        break;
                    case "likes":
                        posts = descending ? posts?.OrderByDescending(p => p.Likes).ToList() : posts?.OrderBy(p => p.Likes).ToList();
                        break;
                    case "popularity":
                        posts = descending ? posts?.OrderByDescending(p => p.Popularity).ToList() : posts?.OrderBy(p => p.Popularity).ToList();
                        break;
                    default:
                        // If invalid sortBy value is provided, return original list
                        Console.WriteLine("Invalid sortBy value provided. Sorting by ID.");
                        posts = posts?.OrderBy(p => p.Id).ToList();
                        break;
                }

                if (posts != null)
                    return posts;
            }

            return null;
        }
    }

    public class FilteredPosts
    {
        [JsonProperty("posts")]
        public List<Post> Posts { get; set; }
    }
}
