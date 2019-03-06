using Microsoft.AspNetCore.Mvc;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : Controller
    {
        //private ICategoryRepository categories;
        //private ITopicRepository topics;
        //private IUserRepository users;
        //private IForumMessageRepository messages;
        //public TopicController(ICategoryRepository categories, ITopicRepository topics, IUserRepository users, IForumMessageRepository messages)
        //{
        //    this.categories = categories;
        //    this.topics = topics;
        //    this.users = users;
        //    this.messages = messages;
        //}
        //[HttpGet]
        //public ActionResult<ISet<TopicAPIModel>> GetAll() => topics.GetAll();

        //[HttpGet("{id}", Name = "GetTopic")]
        //public ActionResult<TopicAPIModel> GetById(int id) => topics.GetById(id);

        //[Authorize]
        //[HttpPost]
        //public ActionResult Create(NewTopicModel model)
        //{
        //    var topic = new Topic
        //    {
        //        Category = categories.GetByIdWithoutChilds(model.CategoryId),
        //        Name = model.Name,
        //        Date = DateTime.Now,
        //        User = users.GetByEmail(User.Identity.Name),
        //        IsClosed = false,
        //    };
        //    topic = topics.Append(topic);
        //    topic.Messages = new HashSet<ForumMessage>();
        //    var msg = new ForumMessage
        //    {
        //        Text = model.InitialText,
        //        User = topic.User,
        //        Date = DateTime.Now,
        //        Topic = topic,
        //    };
        //    topic.Messages.Add(msg);
        //    messages.Append(msg);
        //    return CreatedAtRoute("GetTopic", new { id = topic.Id }, new TopicAPIModel(topic));
        //}

        //[Authorize]
        //[HttpPut("{id}")]
        //public IActionResult Update(int id, TopicAPIModel topic)
        //{
        //    var dbTopic = topics.GetByIdWithUser(id);
        //    topic.Id = id;
        //    if (User.Identity.Name == dbTopic.User.Email || User.IsInRole("admin"))
        //    {
        //        if(topics.Update(topic))
        //            return NoContent();
        //        return BadRequest();
        //    }
        //    return StatusCode(403);
        //}
    }
}