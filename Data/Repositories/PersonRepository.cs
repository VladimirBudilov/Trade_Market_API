using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(TradeMarketDbContext context) : base(context)
    {
    }
    public new void Update(Person entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        var currentPerson = GetByIdAsync(entity.Id).Result;
        currentPerson.Name = entity.Name;
        currentPerson.Surname = entity.Surname;
        currentPerson.BirthDate = entity.BirthDate;
    }
}