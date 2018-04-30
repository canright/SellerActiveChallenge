using System;
using Microsoft.AspNetCore.Mvc;

using Typicode = SellerActiveChallenge.Models.Typicode;

namespace SellerActiveChallenge.Controllers
{
    [Route("discussion")]
    public class DiscussionController : Controller
    {
        [HttpGet("users")]
        public IActionResult ListAllUsers() =>
            ListAll<Models.User>(Typicode.USERS);

        [HttpGet("users/{id}")]
        public IActionResult FetchUserById(int id) =>
            FetchById<Models.User>(Typicode.USERS, id);

        [HttpGet("users/email/{email}")]
        public IActionResult FetchUserByEmail(string email) =>
            FetchByAlternateKey<Models.User>(Typicode.USERS, Typicode.EMAIL, email);

        [HttpGet("posts")]
        public IActionResult ListAllPosts() =>
            ListAll<Models.Post>(Typicode.POSTS);

        [HttpGet("posts/{id}")]
        public IActionResult FetchPostById(int id) =>
            FetchById<Models.Post>(Typicode.POSTS, id);

        [HttpGet("posts/user/{userId}")]
        public IActionResult ListPostsForUserId(int userId) =>
            ListForField<Models.Post>(Typicode.POSTS, Typicode.USERID, userId);

        [HttpGet("posts/latest")]
        public IActionResult ListLatestPosts() =>
            ListLatest<Models.Post>(Typicode.POSTS);

        [HttpGet("posts/latest/{userId}")]
        public IActionResult ListLatestPostsForUserId(int userId) =>
            ListLatestByFieldValue<Models.Post>(Typicode.POSTS, Typicode.USERID, userId.ToString());

        [HttpGet("comments")]
        public IActionResult ListAllComments() =>
            ListAll<Models.Comment[]>(Typicode.COMMENTS);

        [HttpGet("comments/{id}")]
        public IActionResult FetchCommentById(int id) =>
            FetchById<Models.Comment>(Typicode.COMMENTS, id);

        [HttpGet("comments/post/{postId}")]
        public IActionResult ListCommentsForPostId(int postId) =>
            ListForField<Models.Comment>(Typicode.COMMENTS, Typicode.POSTID, postId);

        // Page size for "show latest"
        private const int PAGESIZE = 10;

        private IActionResult ListAll<T>(String entity)
        {
            try
            {
                return Json(Typicode.GetTableForEntity<T>(entity));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult FetchById<T>(String entity, int id)
        {
            try
            {
                return Json(Typicode.GetRecordById<T>(entity, id));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult ListForField<T>(String entity, String field, int value)
        {
            try
            {
                return Json(Typicode.GetTableByFieldValue<T>(entity, field, value.ToString()));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult FetchByAlternateKey<T>(String entity, String field, string email)
        {
            try
            {
                return Json(Typicode.GetRecordByFieldValue<T>(entity, field, email));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult ListLatest<T>(String entity)
        {
            try
            {
                return Json(Typicode.GetLatest<T>(entity, PAGESIZE));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IActionResult ListLatestByFieldValue<T>(String Entity, String Field, String Value)
        {
            try
            {
                return Json(Typicode.GetLatestByFieldValue<T>(Entity, Field, Value, PAGESIZE));
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private NotFoundResult NotFoundHandler(Exception e)
        {
            if (e.Message.Contains("404") || e.Message.Contains("Not Found"))
                return NotFound();
            else
                throw e;
        }

    }
}
