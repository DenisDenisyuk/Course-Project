using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp2015_5Var8.Models;
using WebApp2015_5Var8.DAO;

namespace WebApp2015_5Var8.Controllers
{
    public class HomeController : Controller
    {
        // класс контроллера использует определение объекта доступа к таблице university_employees (см. класс EmployeeDAO)
        EmployeeDAO employeeDAO = new EmployeeDAO();

        // GET: Home
        public ActionResult Index()
        {
            // возвращает список всех заказов, полученных из БД
            return View(employeeDAO.GetAllRecords());
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View(employeeDAO.GetRecord(id));
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "ID")] Employee employee)
        {
            try
            {
                // TODO: Add insert logic here
                if (employeeDAO.AddRecord(employee))
                    return RedirectToAction("Index");
                else
                    return View("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View(employeeDAO.GetRecord(id));
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                // TODO: Add update logic here
                if (employeeDAO.UpdateRecord(employee))
                    return RedirectToAction("Index");

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View(employeeDAO.GetRecord(id));
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Employee employee)
        {
            try
            {
                // TODO: Add delete logic here
                if (employeeDAO.DeleteRecord(id, employee))
                    return RedirectToAction("Index");
                else
                    return View("Create");
            }
            catch
            {
                return View();
            }
        }
    }
}
