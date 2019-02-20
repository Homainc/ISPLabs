using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using ISPLabs.Models;
using ISPLabs.Models.API;
using NHibernate;
using Microsoft.AspNetCore.Authorization;
using ISPLabs.Repositories.Interfaces;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        //private ICategoryRepository categories;
        //public CategoryController(NHibernateHelper nHibernateHelper, ICategoryRepository categories)
        //{
        //    this.categories = categories;
        //}

        //[HttpGet]
        //public ActionResult<ISet<CategoryAPIModel>> GetAll() => 
        //    categories.GetAll().Select(x => new CategoryAPIModel(x, false)).ToHashSet();

        //[HttpGet("{id}", Name = "GetCategory")]
        //public ActionResult<CategoryAPIModel> GetById(int id) => categories.GetById(id);

        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //public ActionResult Create(CategoryAPIModel cat)
        //{
        //    var newCategory = categories.Append(cat);
        //    if (newCategory == null)
        //        return StatusCode(403);
        //    return CreatedAtRoute("GetCategory", new { id = newCategory.Id }, newCategory);

        //}
        //[Authorize(Roles = "admin")]
        //[HttpPut("{id}")]
        //public IActionResult Update(int id, CategoryAPIModel cat)
        //{
        //    cat.Id = id;
        //    if(categories.Update(cat))
        //        return NoContent();
        //    return BadRequest();
        //}
        //[Authorize(Roles = "admin")]
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    if (categories.Remove(id))
        //        return NoContent();
        //    return BadRequest();
        //}
    }
}