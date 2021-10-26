﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NBG.Visitor.Storage;
using NBG.Visitor.Storage.Models;

namespace NBG.Visitor.Services.DB
{
    public class VisitRepository
    {
        private VisitContext _context = new();

        // get
        public async Task<Storage.Models.Visitor> GetVisitorIfExists(string firstName, string lastName, string phoneNumber)
        {
            return await Task.Run(_context.Visitors.Where(x => x.FirstName == firstName && x.LastName == lastName && x.PhoneNumber == phoneNumber).FirstOrDefault);
        }

        public async Task<ContactPerson[]> GetContactPeople()
        {
            return await Task.Run(_context.ContactPeople.ToArray);
        }

        public async Task<ContactPerson> GetContactPersonByName(string name)
        {
            return await _context.ContactPeople.FindAsync(name);
        }
        
        public async Task<Company[]> GetCompanies()
        {
            return await Task.Run(_context.Companies.ToArray);
        }

        public async Task<Company> GetCompanyByLabel(string label)
        {
            return await _context.Companies.FindAsync(label);
        }

        // add
        public async Task AddVisitor(Storage.Models.Visitor visitor)
        {
            await _context.Visitors.AddAsync(visitor);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a visit entry to the Database.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="contactPersonName"></param>
        /// <param name="companyLabel"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        public async Task<Visit> AddVisit(DateTime start, ContactPerson contactPerson, Company company, string firstName, string lastName, string phoneNumber, string email = null)
        {
            var visitor = await GetVisitorIfExists(firstName, lastName, phoneNumber);
            if (visitor == null)
            {
                visitor = new Storage.Models.Visitor() { FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, Email = email };
                await AddVisitor(visitor);
            }
            var visit = (await _context.Visits.AddAsync(new Visit() { VisitStart = start, Visitor = visitor, ContactPerson = contactPerson, Company = company })).Entity;
            await _context.SaveChangesAsync();
            return visit;
        }

        // remove
        public async Task RemoveVisitor(Storage.Models.Visitor visitor)
        {
            await Task.Run(() => _context.Visitors.Remove(visitor));
            await _context.SaveChangesAsync();
        }

        public async Task RemoveVisit(Visit visit)
        {
            await Task.Run(() => _context.Visits.Remove(visit));
            await _context.SaveChangesAsync();
        }

        #region Read
        public async Task<Visit> ReadVisit(int id)
        {
            return await _context.Visits
                .Include(v => v.Visitor)
                .Include(v => v.ContactPerson)
                .Include(v => v.Company)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        public async Task<IEnumerable<Visit>> ReadAllVisits()
        {
            return await _context.Visits
                .Select(v => new Visit
                {
                    ContactPerson = v.ContactPerson,
                    Company = v.Company,
                    Visitor = v.Visitor
                }).ToListAsync();
        }
        public async Task<Storage.Models.Visitor> ReadVisitor(int id)
        {
            return await _context.Visitors.FindAsync(id);
        }
        public async Task<IEnumerable<Storage.Models.Visitor>> ReadAllVisitors()
        {
            return await _context.Visitors.ToListAsync();
        }
        #endregion
    }
}
