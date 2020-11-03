using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using TaskBit.Models;
using Dapper;
using System.IO;
using CsvHelper;

namespace TaskBit.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View(DapperORM.ReturnList<Employee>("EmployeeViewAll", null));
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View();
            else
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                return View(DapperORM.ReturnList<Employee>("EmployeeViewByID", param).FirstOrDefault<Employee>());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Employee employee)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", employee.Id);
            param.Add("@Name", employee.Name);
            param.Add("@DateOfBirth", employee.DateOfBirth);
            param.Add("@IsMarried", employee.IsMarried);
            param.Add("@Phone", employee.Phone);
            param.Add("@Salary", employee.Salary);
            DapperORM.ExecuteWithoutReturn("EmployeeAddOrEdit", param);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", id);
            DapperORM.ExecuteWithoutReturn("EmployeeDeleteById", param);
            return RedirectToAction("Index");
        }

        //[HttpGet]
        public ActionResult UpploadFile()
        {
            return View("UpploadFile");
        }

        [HttpPost]
        public ActionResult UpploadFile(HttpPostedFileBase file)
        {
            string path = null;
            try
            {
                if(file.ContentLength>0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = AppDomain.CurrentDomain.BaseDirectory + "upload\\" + fileName;
                    file.SaveAs(path);

                    using (var reader = new StreamReader(path))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    { 
                        var emplList = csv.GetRecords<Employee>();

                        foreach (var el in emplList)
                        {

                            DynamicParameters param = new DynamicParameters();
                            //param.Add("@Id", el.Id);
                            param.Add("@Name", el.Name);
                            param.Add("@DateOfBirth", el.DateOfBirth);
                            param.Add("@IsMarried", el.IsMarried);
                            param.Add("@Phone", el.Phone);
                            param.Add("@Salary", el.Salary);
                            DapperORM.ExecuteWithoutReturn("AddDataFromCSV", param);
                            //return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return RedirectToAction("Index");
        }

    }
}