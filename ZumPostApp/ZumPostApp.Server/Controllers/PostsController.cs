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
        public async Task<IActionResult> GetTag(string tag)
        {
            var posts = await _postRepository.GetPostsAsync(tag);

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
        Task<List<Post>> GetPostsAsync(string tag);
    }

    public class ThirdPartyApiRepo : IPostRepository
    {
        private readonly HttpClient _httpClient;

        public ThirdPartyApiRepo(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.hatchways.io/assessment/blog/posts");
        }


        public async Task<List<Post>> GetPostsAsync(string tag)
        {
            var apiURL = $"{_httpClient.BaseAddress}?tag={tag}";

            var response = await _httpClient.GetAsync(apiURL);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<FilteredPosts>(responseContent);

                if (posts != null && posts.Posts != null)
                    return posts.Posts;
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
