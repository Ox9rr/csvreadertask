using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskBit.Models
{
    public class EmployeeViewModel
    {
        //[Name(Constants.CsvHeaders.Id)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMarried { get; set; }
        public int Phone { get; set; }
        public decimal Salary { get; set; }
    }
}