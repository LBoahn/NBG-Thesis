using NBG.Visitor.Storage.Models;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NBG.Visitor.Services.DB.Test
{
    public class Tests
    {
        private VisitRepository _repo = new();

        [SetUp]
        public void Setup()
        {

        }

        #region Write

        [Test]
        public async Task TestAddVisitor()
        {
            var visitor = new Storage.Models.Visitor() { FirstName = "Max", LastName = "Mustermann", PhoneNumber = "0664 0815" };
            await _repo.AddVisitor(visitor);
            Assert.IsNotNull(visitor.Id);

            await _repo.RemoveVisitor(visitor);
        }

        [Test]
        public async Task TestGetVisitor()
        {
            var visitor = await _repo.GetVisitorIfExists("Pat", "Du Plantier", "724 964 6781");
            Assert.IsInstanceOf<Storage.Models.Visitor>(visitor);
            visitor = await _repo.GetVisitorIfExists("Non", "existent", "person");
            Assert.IsNull(visitor);
        }

        [Test]
        public async Task TestAddVisit()
        {
            Visit visit = await _repo.AddVisit(DateTime.Now, await _repo.GetContactPersonByName("Andreas Steiner"), await _repo.GetCompanyByLabel("Siemens"), "Dietfried", "Haber", "0664 1234567", "d.haber@email.com");
            Assert.IsInstanceOf<Visit>(visit);
            var visitor = await _repo.GetVisitorIfExists("Dietfried", "Haber", "0664 1234567");
            Assert.IsInstanceOf<Storage.Models.Visitor>(visitor);

            await _repo.RemoveVisit(visit);
            await _repo.RemoveVisitor(visitor);
        }
        #endregion

        #region Read
        [Test]
        public async Task TestReadVisit()
        {
            var visit = await _repo.ReadVisit(28);
            Assert.IsTrue(visit.ContactPerson.Name == "Andreas Steiner");
        }

        [Test]
        public async Task TestReadVisitor()
        {
            var visitor = await _repo.ReadVisitor(1);
            Assert.IsTrue(visitor.Email == "pduplantier0@opera.com");
        }

        [Test]
        public async Task TestReadAllVisits()
        {
            var visits = await _repo.ReadAllVisits();
            Assert.IsTrue(visits.First().ContactPerson.Name == "Andreas Steiner");
        }

        [Test]
        public async Task TestReadAllVisitors()
        {
            var visitors = await _repo.ReadAllVisitors();
            Assert.IsTrue(visitors.First().Email == "pduplantier0@opera.com");
        }
        #endregion
    }
}
