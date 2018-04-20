using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
/*
using System.Web;
using System.Net;
using System.Net.Http;
*/

namespace SellerActiveChallenge.Controllers
{
    [Route("discussion")]
    public class DiscussionController : Controller
    {
        // all routes specified here for easy reference
        private static class ROUTES
        {
            public const String USERS = "users";
            public const String USER = "users/{id}";
            public const String USER_EMAIL = "users/email/{email}";
            public const String POSTS = "posts";
            public const String POST = "posts/{id}";
            public const String POSTS_USER = "posts/user/{userId}";
            public const String POSTS_LATEST = "posts/latest";
            public const String POSTS_LATEST_USER = "posts/latest/{userId}";
            public const String COMMENTS = "comments";
            public const String COMMENT = "comments/{id}";
            public const String COMMENTS_POST = "comments/post/{postId}";
        }

        // Page size for "show latest"
        private const int PAGESIZE = 10;

        [HttpGet("routes")]
        public String ListRoutes()
        {
            return "<a href='/discussion/users'>Users</a><br>\n<a href='/discussion/posts'>Posts</a><br>\n";
        }

        [HttpGet(ROUTES.USERS)]
        public IEnumerable<Object> ListAllUsers()
        {
            try
            {
                return Model.GetTableForEntity(Model.USERS);
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.USER)]
        public Object FetchUserById(int id)
        {
            try
            {
                return Model.GetRecordById(Model.USERS, id);
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.USER_EMAIL)]
        public Object FetchUserByEmail(string email)
        {
            try
            {
                return Model.GetRecordByFieldValue(Model.USERS, Model.EMAIL, email);
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.POSTS)]
        public IEnumerable<Object> ListAllPosts()
        {
            try
            {
                return Model.GetTableForEntity(Model.POSTS);
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.POST)]
        public Object FetchPostById(int id)
        {
            try
            {
                return Model.GetRecordById(Model.POSTS, id);
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.POSTS_USER)]
        public IEnumerable<Object> ListPostsForUserId(int userId)
        {
            try
            {
                return Model.GetTableByFieldValue(Model.POSTS, Model.USERID, userId.ToString());
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.POSTS_LATEST)]
        public IEnumerable<Object> ListLatestPosts()
        {
            try
            {
                return Model.GetLatest(Model.POSTS, PAGESIZE);
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.POSTS_LATEST_USER)]
        public IEnumerable<Object> ListLatestPostsForUserId(int userId)
        {
            try
            {
                return Model.GetLatestByFieldValue(Model.POSTS, Model.USERID, userId.ToString(), PAGESIZE);
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.COMMENTS)]
        public IEnumerable<Object> ListAllComments()
        {
            try
            {
                return Model.GetTableForEntity(Model.COMMENTS);
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.COMMENT)]
        public Object FetchCommentById(int id)
        {
            try
            {
                return Model.GetRecordById(Model.COMMENTS, id);
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.COMMENTS_POST)]
        public IEnumerable<Object> ListCommentsForPostId(int postId)
        {
            try
            {
                return Model.GetTableByFieldValue(Model.COMMENTS, Model.POSTID, postId.ToString());
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
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
