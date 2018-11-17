using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ISPLabs.Models;
using System;

namespace ISPLabs.Services
{
    public class NHibernateHelper
    {
        public ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(@"Server=(localdb)\ProjectsV13; initial catalog=ForumDB; Integrated Security=SSPI;")
                .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Partition>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
        public void AddTest()
        {
            using (ISession session = OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    //Создать, добавить
                    var urole = new Role { Name = "user" };
                    var arole = new Role { Name = "admin" };
                    var admin = new User
                    {
                        Role = arole,
                        Login = "spritefok",
                        Email = "spritefok@gmail.com",
                        Password = "123456",
                        RegistrationDate = DateTime.Now
                    };
                    arole.Users.Add(admin);
                    var part = new Partition { Name = "Test Partition" };
                    var cat = new Category { Name = "Test Category 1", Description = "Test description 1, test, yep", Partition = part };
                    var topic = new Topic { Name = "Test Topic 1", Date = DateTime.Now, Category = cat, User = admin };
                    var msg1 = new ForumMessage
                    {
                        Text = "Lorem Ipsuns skasjssjkssas kdk jdkak jdjk ajdaskdjas dsakdsajdsajdsadsdd" +
                        "dsadddsdsadsadsadsadsaa d",
                        Date = DateTime.Now,
                        User = admin,
                        Topic = topic
                    };
                    var msg2 = new ForumMessage
                    {
                        Text = "Lorem Ipsuns skasjssjkssas kdk jdkak jdjk ajdaskdjas dsakdsajdsajdsadsdd" +
                        "dsadddsdsadsadsadsadsaa d",
                        Date = DateTime.Now,
                        User = admin,
                        Topic = topic
                    };
                    admin.Topics.Add(topic);
                    admin.Messages.Add(msg1);
                    admin.Messages.Add(msg2);
                    part.Categories.Add(cat);
                    cat.Topics.Add(topic);
                    topic.Messages.Add(msg1);
                    topic.Messages.Add(msg2);
                    session.SaveOrUpdate(urole);
                    session.SaveOrUpdate(arole);
                    session.SaveOrUpdate(part);
                    session.SaveOrUpdate(cat);
                    session.SaveOrUpdate(admin);
                    session.SaveOrUpdate(topic);
                    session.SaveOrUpdate(msg1);
                    session.SaveOrUpdate(msg2);
                    
                    //Обновить (По идентификатору)
                    //var series = session.Get<Series>(1);
                    //var updateBook = session.Get<Book>(1);
                    //updateBook.Name = "Metro2034";
                    //updateBook.Description = "Антиутопия";
                    //updateBook.Authors.ElementAt(0).Name = "Глуховский";
                    //updateBook.Genres.ElementAt(0).Name = "Антиутопия";
                    //updateBook.Series = series;
                    //updateBook.Mind.MyMind = "11111";
                    //session.SaveOrUpdate(updateBook);

                    //Удаление (По идентификатору)
                    //var deleteBook = session.Get<Book>(1);
                    //session.Delete(deleteBook);

                    transaction.Commit();
                }
            }
        }
    }
}