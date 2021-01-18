using ProductApp.Asp.NetWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Asp.NetWebApi.Interface
{
   public  interface IProductRepository
    {

        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Delete(Product product);
        void Create(Product product);
        void Update(Product product);

    }
}
