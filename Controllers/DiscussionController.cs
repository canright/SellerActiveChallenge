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

        private IEnumerable<Object> ListAll(String entity)
        {
            try
            {
                return Model.GetTableForEntity(entity);
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        private Object FetchById(String entity, int id)
        {
            try
            {
                return Model.GetRecordById(entity, id);
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        private IEnumerable<Object> ListForField(String entity, String field, int value)
        {
            try
            {
                return Model.GetTableByFieldValue(entity, field, value.ToString());
            }
            catch (Exception e)
            {
                return (IEnumerable<Object>)NotFoundHandler(e);
            }
        }

        private Object FetchByAlternateKey(String entity, String field, string email)
        {
            try
            {
                return Model.GetRecordByFieldValue(entity, field, email);
            }
            catch (Exception e)
            {
                return NotFoundHandler(e);
            }
        }

        [HttpGet(ROUTES.USERS)]
        public IEnumerable<Object> ListAllUsers()
        {
            return ListAll(Model.USERS);
        }

        [HttpGet(ROUTES.USER)]
        public Object FetchUserById(int id)
        {
            return FetchById(Model.USERS, id);
        }

        [HttpGet(ROUTES.USER_EMAIL)]
        public Object FetchUserByEmail(string email)
        {
            return FetchByAlternateKey(Model.USERS, Model.EMAIL, email);
        }

        [HttpGet(ROUTES.POSTS)]
        public IEnumerable<Object> ListAllPosts()
        {
            return ListAll(Model.POSTS);
        }

        [HttpGet(ROUTES.POST)]
        public Object FetchPostById(int id)
        {
            return FetchById(Model.POSTS, id);
        }

        [HttpGet(ROUTES.POSTS_USER)]
        public IEnumerable<Object> ListPostsForUserId(int userId)
        {
            return ListForField(Model.POSTS, Model.USERID, userId);
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
            return ListAll(Model.USERS);
        }

        [HttpGet(ROUTES.COMMENT)]
        public Object FetchCommentById(int id)
        {
            return FetchById(Model.COMMENTS, id);
        }

        [HttpGet(ROUTES.COMMENTS_POST)]
        public IEnumerable<Object> ListCommentsForPostId(int postId)
        {
            return ListForField(Model.COMMENTS, Model.POSTID, postId);
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
