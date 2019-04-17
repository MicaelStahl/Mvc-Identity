using Mvc_Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.Interfaces
{
    public interface IPersonRepository
    {
        List<Person> AllPeople();

        Person CreatePerson(Person person);

        Person EditPerson(Person person);

        bool DeletePerson(int? id);

        Person FindPerson(int? id);

        Person FindPersonWithEverything(int? id);
    }
}
