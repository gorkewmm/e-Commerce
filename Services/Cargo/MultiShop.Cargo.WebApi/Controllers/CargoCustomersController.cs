using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.Dtos.CargoCustomerDtos;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoCustomersController : ControllerBase
    {
        private readonly ICargoCustomerService _cargoCustomerService;

        public CargoCustomersController(ICargoCustomerService cargoCustomerService)
        {
            _cargoCustomerService = cargoCustomerService;
        }

        [HttpGet]
        public IActionResult CargoCustomerList() 
        {
            var values = _cargoCustomerService.TGetAll();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public IActionResult GetCargoCustomerById(int id) 
        {
            var value = _cargoCustomerService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete]
        public IActionResult RemoveCargoCustomer(int id)
        {
            _cargoCustomerService.TDelete(id);
            return Ok("Kargo Müşteri Silme İşlemi Başarıyla Yapıldı");
        }

        [HttpPost]
        public IActionResult CreateCargoCustomer(CreateCargoCustomerDto _createCargoCustomerDto)
        {
            CargoCustomer cargoCustomer = new CargoCustomer()
            {
                Name = _createCargoCustomerDto.Name,
                Surname = _createCargoCustomerDto.Surname,
                Email = _createCargoCustomerDto.Email,
                Phone = _createCargoCustomerDto.Phone,
                District = _createCargoCustomerDto.District,
                City = _createCargoCustomerDto.City,
                Address = _createCargoCustomerDto.Address
            };
            _cargoCustomerService.TInsert(cargoCustomer);
            return Ok("Kargo Müşteri Ekleme İşlemi Başarıyla Yapıldı");
        }

        [HttpPut]
        public IActionResult UpdateCargoCustomer(UpdateCargoCustomerDto _updateCargoCustomerDto)
        {
            CargoCustomer cargoCustomer = new CargoCustomer()
            {
                CargoCustomerId = _updateCargoCustomerDto.CargoCustomerId,
                Name = _updateCargoCustomerDto.Name,
                Surname = _updateCargoCustomerDto.Surname,
                Email = _updateCargoCustomerDto.Email,
                Phone = _updateCargoCustomerDto.Phone,
                District = _updateCargoCustomerDto.District,
                City = _updateCargoCustomerDto.City,
                Address = _updateCargoCustomerDto.Address
            };
            _cargoCustomerService.TUpdate(cargoCustomer);
            return Ok("Kargo Müşteri Güncelleme İşlemi Başarıyla Yapıldı");
        }
    }
}
