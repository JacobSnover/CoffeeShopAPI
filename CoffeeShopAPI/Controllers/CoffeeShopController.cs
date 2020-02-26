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
            using (var db = new CoffeeShopContext())
            {
                using (var scope = db.Database.BeginTransaction())
                {
                    var Item = JsonSerializer.Deserialize<Items>(item);
                    Item.id = 0;
                    db.Items.Add(Item);
                    db.SaveChanges();
                    scope.Rollback();
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
                        var Item = db.Items.Where(item => item.id == Convert.ToInt32(itemID)).SingleOrDefault();
                        db.Items.Remove(Item);
                        db.SaveChanges();
                        scope.Commit();
                    }
                    catch (Exception ex)
                    {
                        scope.Rollback();
                    }
                   
                }
            }

        }
    }
}