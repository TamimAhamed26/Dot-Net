using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Interfaces
{
    public interface IRepo<CLASS, ID, RET>
    {
        List<CLASS> Get(); // Retrieve all entities
        CLASS Get(ID id); // Retrieve a single entity by ID
        RET Create(CLASS obj); // Add a new entity and return a result
        RET Update(CLASS obj); // Update an existing entity and return a result
        bool Delete(ID id); // Remove an entity by ID and return a success flag
    }
}
