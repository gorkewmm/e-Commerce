using MultiShop.Order.Application.Features.CQRS.Queries.AddressQueries;
using MultiShop.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class GetAddressByIdQueryHandler
    {
        private readonly IRepository<Address> _repository;

        public GetAddressByIdQueryHandler(IRepository<Address> addressRepository)
        {
            _repository = addressRepository;
        }

        public async Task<GetAdressByIdQueryResult> Handle(GetAddressByIdQuery query)
        {
            var values = await _repository.GetByIdAsync(query.Id);
            return new GetAdressByIdQueryResult
            {
                AddressId = values.AddressId,
                UserId = values.UserId,
                District = values.District,
                City = values.City,
                Detail = values.Detail,
            };
        }
        
    }
}
