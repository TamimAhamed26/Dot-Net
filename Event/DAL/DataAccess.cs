using DAL.EF.Entities;
using DAL.Repos;
using DAL.Interface;

namespace DAL
{
    public static class DataAccess
    {
      
        private static LayerContext CreateContext()
        {
            return new LayerContext();
        }

       
        public static EventRepo EventData()
        {
            return new EventRepo(CreateContext());  
        }
    }
}
