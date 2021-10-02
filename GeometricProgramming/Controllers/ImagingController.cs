namespace GeometricProgramming
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using GeometricProgramming.Common.Models;
    using GeometricProgramming.Services.Abstract;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagingController : ControllerBase
    {
        private readonly IPrintService _printService;
        public ImagingController(IPrintService printService)
        {
            this._printService = printService;
        }

        [HttpPost]
        [ActionName("PrintAllTrianglesInBox")]
        public IActionResult PrintAllTrianglesInBox([FromBody] GeometryDto geometryDto)
        {
            try
            {
                var result = _printService.PlotAllTriangles(new ImagingDto { Columns = geometryDto.Columns, Rows = geometryDto.Rows, PixelSize = geometryDto.PixelSize, Triangles = new List<TriangleDto>() });
                return File(result.MainImage, "image/jpeg");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("PrintGivenTriangles")]
        public IActionResult PrintGivenTriangles([FromBody] ImagingDto imagingDto)
        {
            try
            {
                var result = _printService.PlotGivenTriangles(imagingDto);
                return File(result.MainImage, "image/jpeg");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("EvaluateTrianglesPositions")]
        public IActionResult EvaluateTrianglesPositions([FromBody] ImagingDto imagingDto)
        {
            try
            {
                var result = _printService.GetTrianglesPositions(imagingDto);
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("EvluateTriangleCoordinates")]
        public IActionResult EvluateTriangleCoordinates([FromBody] GeometryDto geometryDto)
        {
            try
            {
                var result = _printService.GetTriangleCoordinates(geometryDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}