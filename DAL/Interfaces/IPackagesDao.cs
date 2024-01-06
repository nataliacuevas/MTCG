using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL.Interfaces
{
    public interface IPackagesDao
    {
        public void CreatePackage(List<Card> cards);
        public Package SelectPackageById(int id);
        public void DeletePackageById(int id);
        public Package PopRandomPackage();
    }
}
