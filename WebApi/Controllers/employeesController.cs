﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EmployeeDataAccess;
using System.Web.Http.Cors;
using System.Threading;

namespace WebApi.Controllers
{
    [EnableCorsAttribute("http://localhost:61884", "*","*")]
    [RequireHttps]
    public class employeesController : ApiController
    {
        private TestEntities db = new TestEntities();

        // GET: api/employees
   
        [BasicAuthentication]
        public HttpResponseMessage Getemployees(string gender="All")
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            switch (username.ToLower())
            {
                case "all":
                    return Request.CreateResponse(HttpStatusCode.OK, db.employees);
                case "male":
                    return Request.CreateResponse(HttpStatusCode.OK, db.employees.Where(e => e.gender == "male"));
                case "female":
                    return Request.CreateResponse(HttpStatusCode.OK, db.employees.Where(e => e.gender == "female"));
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            
        }
        [DisableCors]
        //GET: api/employees?fname=hanson
        public IQueryable<employee> GetEmployeeByFirstName(string fname)
        {
            return db.employees.Where(e => e.fname == fname);
        }

        // GET: api/employees/5
        [ResponseType(typeof(employee))]
        public IHttpActionResult Getemployee(int id)
        {
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/employees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putemployee(int id, [FromBody]employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.id)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/employees
        [ResponseType(typeof(employee))]
        public IHttpActionResult Postemployee(employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.employees.Add(employee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (employeeExists(employee.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employee.id }, employee);
        }

        // DELETE: api/employees/5
        [ResponseType(typeof(employee))]
        public IHttpActionResult Deleteemployee(int id)
        {
            employee employee = db.employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool employeeExists(int id)
        {
            return db.employees.Count(e => e.id == id) > 0;
        }
    }
}