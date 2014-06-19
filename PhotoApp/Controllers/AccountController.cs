using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PhotoApp.Helpers;
using System.Security.Cryptography;
using PhotoApp.Models;
using System.Web.Mvc;
using System.Web.Http;

namespace PhotoApp.Controllers
{
    public class AccountController : Controller
    {

        public Object Register(String username, String password, bool remember)
        {
            try
            {
                PhotoAppContext db = new PhotoAppContext();

                SHA256 sha = SHA256Managed.Create();
                byte[] password_digest = sha.ComputeHash(Utils.GetBytes(password + "584967"));

                var list = db.users.ToList();
                User user = list.Where(x => x.username == username).FirstOrDefault();

                if (user != null)
                {
                    return JObject.Parse("{ success: false, error: 'Username is already taken' }");
                }

                User u = new User
                {
                    username = username,
                    password = password_digest
                };

                db.users.Add(u);
                db.SaveChanges();

                HttpCookie cookie = new HttpCookie("PhotoApp");
                cookie.Value = Utils.GetString(password_digest);
                Response.Cookies.Add(cookie);
                Session["username"] = username;
                Session["password"] = password_digest;

                return JObject.Parse("{ success: true, username: '" + username + "' }");
            }
            catch (Exception e)
            {
                return JObject.Parse("{ success: false, error: '" + e.Message + "' }");
            }
        }

        public Object Login(String username, String password, bool remember)
        {
            try
            {
                PhotoAppContext db = new PhotoAppContext();

                SHA256 sha = SHA256Managed.Create();
                byte[] password_digest = sha.ComputeHash(Utils.GetBytes(password + "584967"));

                User u = db.users.Where(x => x.username == username).FirstOrDefault();
                if (u == null)
                {
                    return JObject.Parse("{ success: false, error: 'Username/password is invalid' }");
                }
                else
                {
                    if (Utils.ArrayCompare(u.password, password_digest))
                    {
                        HttpCookie cookie = new HttpCookie("PhotoApp");
                        cookie.Value = Utils.GetHex(password_digest);
                        cookie.Expires = DateTime.Now.AddHours(1);
                        Response.Cookies.Add(cookie);
                        Session["username"] = username;
                        Session["password"] = password_digest;

                        return JObject.Parse("{ success: true, username: '" + username + "' }");
                    }
                    else
                    {
                        return JObject.Parse("{ success: false, error: 'Username/password is invalid' }");
                    }
                }
            }
            catch (Exception e)
            {
                return JObject.Parse("{ success: false, error: '" + e.Message + "' }");
            }
        }

        public Object Renew()
        {
            try
            {
                if (Session["username"] != null && Session["password"] != null)
                {
                    String username = Session["username"].ToString();
                    byte[] password = (byte[])Session["password"];

                    SHA256 sha = SHA256Managed.Create();
                    byte[] password_digest = sha.ComputeHash(Utils.GetBytes(password + "584967"));

                    HttpCookie cookie = new HttpCookie("PhotoApp");
                    cookie.Value = Utils.GetHex(password_digest);
                    cookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(cookie);
                    Session["username"] = username;
                    Session["password"] = password_digest;

                    return JObject.Parse("{ success: true, username: '" + username + "' }");
                }
                else
                {
                    return JObject.Parse("{ success: false, error: 'Existing session not found' }");
                }
            }
            catch (Exception e)
            {
                return JObject.Parse("{ success: false, error: '" + e.Message + "' }");
            }
        }

        public Object Logout()
        {
            try
            {
                HttpCookie cookie = new HttpCookie("PhotoApp");
                cookie.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(cookie);

                Session["username"] = "";
                Session["password"] = "";

                return JObject.Parse("{ success: true }");
            }
            catch (Exception e)
            {
                return JObject.Parse("{ success: false, error: '" + e.Message + "' }");
            }
        }
    }
}
