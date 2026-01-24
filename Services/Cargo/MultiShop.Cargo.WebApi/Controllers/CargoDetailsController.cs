using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.Dtos.CargoDetailDtos;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoDetailsController : ControllerBase
    {
        private readonly ICargoDetailService _cargoDetailService;
        public CargoDetailsController(ICargoDetailService cargoDetailService)
        {
            _cargoDetailService = cargoDetailService;
        }

        [HttpGet]
        public IActionResult CargoDetailList()
        {
            var values = _cargoDetailService.TGetAll();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateCargoDetailCompany(CreateCargoDetailDto _createCargoDetailDto)
        {
            CargoDetail cargoDetail = new CargoDetail()
            {
                Barcode = _createCargoDetailDto.Barcode,
                ReceiverCustomer = _createCargoDetailDto.ReceiverCustomer,
                SenderCustomer = _createCargoDetailDto.SenderCustomer,
                CargoCompanyId = _createCargoDetailDto.CargoCompanyId
            };
            _cargoDetailService.TInsert(cargoDetail);
            return Ok("Kargo Detayları Başarıyla Oluşturuldu");
        }

        [HttpDelete]
        public IActionResult RemoveCargoDetail(int id)
        {
            _cargoDetailService.TDelete(id);
            return Ok("Kargo Detayları Başarıyla Silindi");
        }

        [HttpGet("{id}")]
        public IActionResult GetCargoDetailById(int id)
        {
            var values = _cargoDetailService.TGetById(id);
            return Ok(values);
        }

        [HttpPut]
        public IActionResult UpdateCargoDetail(UpdateCargoDetailDto _updateCargoDetailDto)
        {
            CargoDetail cargoDetail = new CargoDetail()
            {
                CargoDetailId = _updateCargoDetailDto.CargoDetailId,
                Barcode= _updateCargoDetailDto.Barcode,
                ReceiverCustomer = _updateCargoDetailDto.ReceiverCustomer,
                SenderCustomer = _updateCargoDetailDto.SenderCustomer,
                CargoCompanyId = _updateCargoDetailDto.CargoCompanyId
            };
            _cargoDetailService.TUpdate(cargoDetail);
            return Ok("Kargo Detayları Başarıyla Güncellendi");
        }
    }
}
