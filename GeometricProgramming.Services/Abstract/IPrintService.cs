using System.Drawing;
using GeometricProgramming.Common.Models;
using GeometricProgramming.Models;

namespace GeometricProgramming.Services.Abstract
{
    public interface IPrintService
    {
        PrintResults PlotAllTriangles(ImagingDto image);

        PrintResults PlotGivenTriangles(ImagingDto imagingDto);

        PrintResults GetTrianglesPositions(ImagingDto imagingDto);

        string GetTriangleCoordinates(GeometryDto geometryDto);
    }
}
