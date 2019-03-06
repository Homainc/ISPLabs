using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using ISPLabs.Models;
using ISPLabs.Manager;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private OracleConnection _conn;
        private CategoryManager _categories;

        public CategoryController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
            _categories = new CategoryManager(_conn);
        }

        //[HttpGet]
        //public ActionResult<ISet<CategoryAPIModel>> GetAll() {
            
        //}

        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<Category> GetById(int id) => await _categories.GetByIdAsync(id);

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
        ~CategoryController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}