using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Employee;
using e_Commerce.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Interfaces
{
    public interface IEmployeeService
    {
        PaginatedList<EmployeeWithDetail> GetAllPaginatedWithDetailBySearchFilter(EmployeeSearchFilter searchFilter);
        EmployeeWithDetail GetByUserNameAndPassword(string userName, string password);
        Employee GetById(int id);
        EmployeeWithDetail GetByIdWithDetail(int id);
        int Add(Data.Entity.Employee record);
        int Update(Data.Entity.Employee record);
    }
}
