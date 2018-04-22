using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace SellerActiveChallenge.Controllers
{
   public class Model
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
        static private String Query(string query)
        {
            return Model.GetDataFromUrl(Model.rootUrl + query);
        }

        // executes query and returns generic object from json response
        static private T QueryJson<T>(string query)
        {
            String data = Query(query);
            return JsonConvert.DeserializeObject<T>(data);
        }

        // get array of records for an entity
        static public Object[] GetTableForEntity(string entity)
        {
            return QueryJson<Object[]>(entity);
        }

                // get array of latest records for an entity
        static public Object[] GetLatest(string entity, int pageSize)
        {
            Object[] table = GetTableForEntity(entity);
            return SpliceLatest(table, pageSize);
        }

        // gets identified record of entity
        static public Object GetRecordById(string entity, int id)
        {
            string query = entity + "/" + id.ToString();
            return QueryJson<Object>(query);
        }

        // gets entity record by a field value
        static public Object GetRecordByFieldValue(string entity, string field, string value)
        {
            string query = entity + "?" + field + "=" + value;
            return QueryJson<Object[]>(query)[0];
        }

        // gets array of entity records by a field value
        static public Object[] GetTableByFieldValue(string entity, string field, string value)
        {
            string query = entity + "?" + field + "=" + value;
            return QueryJson<Object[]>(query);
        }

        // gets array of latest records entity records by a field value
        static public Object[] GetLatestByFieldValue(string entity, string field, string value, int pageSize)
        {
            Object[] table = GetTableByFieldValue(entity, field, value);
            return SpliceLatest(table, pageSize);
        }

        // returns last (pageSize) records from the sourc table
        // the last records are the latest
        static public Object[] SpliceLatest(Object[] source, int pageSize)
        {
            int len = (source.Length >= pageSize) ? pageSize : source.Length;
            Object[] target = Array.ConvertAll(new Array[len], item => (Object)item);
            Array.Copy(source, source.Length - len, target, 0, len);
            return target;
        }
    }
}
