using e_Commerce.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Interfaces
{
    public interface ISexService
    {
        List<Sex> GetAll();
        Sex GetById(int id);
    }
}
