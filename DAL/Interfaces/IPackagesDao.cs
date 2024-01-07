using MTCG.HttpServer.Schemas;
using MTCG.Models;
using System.Collections.Generic;

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
