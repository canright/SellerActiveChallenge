using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace SellerActiveChallenge.Models
{
    public class Post
    {
        public int userId { get; set; } // FK - User
        public int id { get; set; } // PK
        public string title { get; set; }
        public string body { get; set; }
    };

    public class Comment
    {
        public int postId { get; set; } // FK - Post
        public int id { get; set; } // PK
        public string name { get; set; }
        public string email { get; set; } // AFK - User
        public string body { get; set; }
    }

    public class User
    {
        public int id { get; set; } // PK
        public string name { get; set; }
        public string userName { get; set; }
        public string email { get; set; } // AK - unique among all Users
        public Address address;
        public string phone { get; set; }
        public string website { get; set; }
        public Company company;
    };

        public class Company
    {
        public string name { get; set; }
        public string catchPhrase { get; set; }
        public string bs { get; set; }

    }

    public class Address
    {
        public string street { get; set; }
        public string suite { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public Geo geo;
    }

    public class Geo
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

   public class Typicode
    {
        public const string ID = "id";
        public const string USERS = "Users";
        public const string POSTS = "Posts";
        public const string COMMENTS = "Comments";
        public const string USERID = "userId";
        public const string EMAIL = "email";
        public const string POSTID = "postId";

        // base web request - returns data response to requested url.
        static private String GetDataFromUrl(string url)
        {
            WebResponse response = WebRequest.Create(url).GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string data = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return data;
        }

        // queries are rooted at this sample data
        static private string rootUrl = "http://jsonplaceholder.typicode.com/";

        // executes rooted query and returns string response
        static public String Query(string query) =>
            GetDataFromUrl(rootUrl + query);

        // executes query and returns generic object from json response
        static public T[] QueryJson<T>(string query) =>
            JsonConvert.DeserializeObject<T[]>(Query(query));

        // get array of records for an entity
        static public T[] GetTableForEntity<T>(string entity) =>
            QueryJson<T>(entity);

        // get array of latest records for an entity
        static public T[] GetLatest<T>(string entity, int pageSize) =>
            SpliceLatest<T>(GetTableForEntity<T>(entity), pageSize);

        // gets identified record of entity
        static public T[] GetRecordById<T>(string entity, int id) =>
            QueryJson<T>(entity + "/" + id.ToString());

        // gets entity record by a field value
        static public T GetRecordByFieldValue<T>(string entity, string field, string value) =>
            QueryJson<T>(entity + "?" + field + "=" + value)[0];

        // gets array of entity records by a field value
        static public T[] GetTableByFieldValue<T>(string entity, string field, string value) =>
            QueryJson<T>(entity + "?" + field + "=" + value);

        // gets array of latest records entity records by a field value
        static public T[] GetLatestByFieldValue<T>(string entity, string field, string value, int pageSize) =>
            SpliceLatest<T>(GetTableByFieldValue<T>(entity, field, value), pageSize);

        // returns last (pageSize) records from the source table - the last records are the latest
        static public T[] SpliceLatest<T>(T[] source, int pageSize)
        {
            int len = (source.Length >= pageSize) ? pageSize : source.Length;
            T[] target = Array.ConvertAll(new T[len], item => (T)item);
            Array.Copy(source, source.Length - len, target, 0, len);
            return target;
        }
    }
}
