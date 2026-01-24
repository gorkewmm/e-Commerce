using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.Dtos.CargoOperationDtos;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoOperationsController : ControllerBase
    {
        private readonly ICargoOperationService _cargoOperationService;

        public CargoOperationsController(ICargoOperationService cargoOperationService)
        {
            _cargoOperationService = cargoOperationService;
        }

        [HttpGet]
        public IActionResult CargoOperationList()
        {
            var values = _cargoOperationService.TGetAll();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateCargoOperationCompany(CreateCargoOperationDto _createCargoOperationDto)
        {
            CargoOperation cargoOperation = new CargoOperation()
            {
                Barcode = _createCargoOperationDto.Barcode,
                Description = _createCargoOperationDto.Description,
                OperationDate = _createCargoOperationDto.OperationDate,
            };
            _cargoOperationService.TInsert(cargoOperation);
            return Ok("Kargo İşlemi Başarıyla Oluşturuldu");
        }

        [HttpDelete]
        public IActionResult RemoveCargoOperation(int id)
        {
            _cargoOperationService.TDelete(id);
            return Ok("Kargo İşlemi Başarıyla Silindi");
        }

        [HttpGet("{id}")]
        public IActionResult GetCargoOperationById(int id)
        {
            var values = _cargoOperationService.TGetById(id);
            return Ok(values);
        }

        [HttpPut]
        public IActionResult UpdateCargoOperation(UpdateCargoOperationDto _updateCargoOperationDto)
        {
            CargoOperation cargoOperation = new CargoOperation()
            {
                CargoOperationId = _updateCargoOperationDto.CargoOperationId,
                Barcode = _updateCargoOperationDto.Barcode,
                Description = _updateCargoOperationDto.Description,
                OperationDate= _updateCargoOperationDto.OperationDate
            };
            _cargoOperationService.TUpdate(cargoOperation);
            return Ok("Kargo İşlemi Başarıyla Güncellendi");
        }
    }
}
