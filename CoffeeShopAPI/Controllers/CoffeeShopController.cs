using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeShopController : ControllerBase
    {
        // private CoffeeShopContext db;

        // private void GetData()
        //{
        //db = new CoffeeShopContext();
        //}

        [HttpGet]
        [Route("[action]")]
        public List<Items> GetItems()
        {
            CoffeeShopContext db = new CoffeeShopContext();

            return db.Items.ToList();
        }

        [HttpGet]
        [Route("[action]")]
        public void AddItems(string item)
        {
            // made db context into using statement
            using (var db = new CoffeeShopContext())
            {
                // wrapped our database actions in a transaction
                using (var scope = db.Database.BeginTransaction())
                {
                    try
                    {
                        var Item = JsonSerializer.Deserialize<Items>(item);
                        Item.id = 0;
                        db.Items.Add(Item);
                        db.SaveChanges();

                        // run commit to SaveChanges made to the database
                        scope.Commit();
                    }
                    catch (Exception ex)
                    {
                        //if we run rollback we can cancel all database actions
                        scope.Rollback();
                    }
                }
            }
        }

        [HttpGet]
        [Route("[action]")]
        public void DeleteItem(string itemID)
        {
            using (var db = new CoffeeShopContext())
            {
                using (var scope = db.Database.BeginTransaction())
                {
                    try
                    {
                        // use a Linq where statement to find the Item you need
                        //I also call SingleOrDefault this way if there is no match in the DB
                        //it will return a default object instead
                        var Item = db.Items
                            .Where(item => item.id == Convert.ToInt32(itemID))
                            .SingleOrDefault();

                        //pass in the object to be removed
                        db.Items.Remove(Item);
                        db.SaveChanges();
                        //call commit to SaveChanges
                        scope.Commit();
                    }
                    catch (Exception ex)
                    {
                        //call rollback to cancel all DB actions
                        scope.Rollback();
                    }
                   
                }
            }

        }
    }
}