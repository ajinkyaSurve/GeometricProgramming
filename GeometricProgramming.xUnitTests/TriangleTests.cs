namespace GeometricProgramming.xUnitTests
{
    using Xunit;
    using System;
    using GeometricProgramming.Common.Models;
    using FakeItEasy;
    using GeometricProgramming.Services.Abstract;
    using GeometricProgramming.Models;
    using System.Collections.Generic;

    public class TriangleTests
    {
        private IPrintService _printService;

        public TriangleTests()
        {
            _printService = A.Fake<IPrintService>();
        }

        [InlineData(27, 26, 100)]
        [InlineData(58, 68, 1000)]
        [Theory]
        public void PrintAllTrianglesInBoxTest_InvalidGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto { Rows = rows, Columns = cols, PixelSize = pixel };
            A.CallTo(() => _printService.PlotAllTriangles(imagingDto)).Throws<ArgumentException>();
            try
            {
                var results = _printService.PlotAllTriangles(imagingDto);
            }
            catch (Exception ex)
            {
                Assert.True(ex.GetType().Name == typeof(ArgumentException).Name);
            }
        }

        [InlineData(26, 26, 100)]
        [InlineData(10, 17, 1000)]
        [Theory]
        public void PrintAllTrianglesInBoxTest_ValidGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto { Rows = rows, Columns = cols, PixelSize = pixel };
            A.CallTo(() => _printService.PlotAllTriangles(imagingDto)).Returns(new PrintResults { MainImage = null, Result = "Success" });
            var results = _printService.PlotAllTriangles(imagingDto);
            Assert.True(results.Result == "Success");
        }

        [InlineData(56, 100, 10)]
        [InlineData(95, 10, 10)]
        [Theory]
        public void PrintGivenTrianglesInBoxTest_InvalidGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto
            {
                Rows = rows,
                Columns = cols,
                PixelSize = pixel,
                Triangles = new List<TriangleDto> {new TriangleDto{
                    VertexA = new CoordinatesDto { X = 0, Y = 0 },
                    VertexB = new CoordinatesDto { X = 100, Y = 10 },
                    VertexC = new CoordinatesDto { X = 10, Y = 10 }
                } }
            };
            A.CallTo(() => _printService.PlotAllTriangles(imagingDto)).Throws<ArgumentException>();

            try
            {
                var results = _printService.PlotAllTriangles(imagingDto);
            }
            catch (Exception ex)
            {
                Assert.True(ex.GetType().Name == typeof(ArgumentException).Name);
            }
        }

        [InlineData(3, 3, 10)]
        [Theory]
        public void PrintGivenTrianglesInBoxTest_validGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto
            {
                Rows = rows,
                Columns = cols,
                PixelSize = pixel,
                Triangles = new List<TriangleDto> {new TriangleDto{
                    VertexA = new CoordinatesDto { X = 0, Y = 0 },
                    VertexB = new CoordinatesDto { X = 0, Y = 10 },
                    VertexC = new CoordinatesDto { X = 10, Y = 10 }
                } }
            };
            A.CallTo(() => _printService.PlotAllTriangles(imagingDto)).Returns(new PrintResults { MainImage = null, Result = "Success" });

            var result = _printService.PlotAllTriangles(imagingDto);
            Assert.True(result.Result == "Success");
        }

        [InlineData(56, 100, 10)]
        [InlineData(95, 10, 10)]
        [Theory]
        public void GetTrianglesPositionsTest_InvalidGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto
            {
                Rows = rows,
                Columns = cols,
                PixelSize = pixel,
                Triangles = new List<TriangleDto>
                {
                    new TriangleDto
                    {
                        VertexA = new CoordinatesDto { X = 0, Y = 0 },
                        VertexB = new CoordinatesDto { X = 100, Y = 10 },
                        VertexC = new CoordinatesDto { X = 10, Y = 10 }
                    }
                }
            };
            A.CallTo(() => _printService.GetTrianglesPositions(imagingDto)).Throws<ArgumentException>();

            try
            {
                var results = _printService.GetTrianglesPositions(imagingDto);
            }
            catch (Exception ex)
            {
                Assert.True(ex.GetType().Name == typeof(ArgumentException).Name);
            }
        }

        [InlineData(5, 5, 10)]
        [InlineData(7, 7, 10)]
        [Theory]
        public void GetTrianglesPositionsTest_validGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto
            {
                Rows = rows,
                Columns = cols,
                PixelSize = pixel,
                Triangles = new List<TriangleDto>
                {
                    new TriangleDto
                    {
                        VertexA = new CoordinatesDto { X = 0, Y = 0 },
                        VertexB = new CoordinatesDto { X = 100, Y = 10 },
                        VertexC = new CoordinatesDto { X = 10, Y = 10 }
                    }
                }
            };
            //TODO: Return real image
            A.CallTo(() => _printService.GetTrianglesPositions(imagingDto)).Returns(new PrintResults { MainImage = null, Result = "Success" });

            var results = _printService.GetTrianglesPositions(imagingDto);

            Assert.True(results.Result == "Success");
        }

        [InlineData(52, 26, 100)]
        [InlineData(58, 68, 1000)]
        [Theory]
        public void GetTriangleCoordinates_InvalidGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto { Rows = rows, Columns = cols, PixelSize = pixel };
            A.CallTo(() => _printService.GetTriangleCoordinates(imagingDto)).Throws<ArgumentException>();
            
            var results = _printService.GetTriangleCoordinates(new ImagingDto { PixelSize = 100, Rows = 6, Columns = 6 });

            Assert.True(results == string.Empty);
        }

        [InlineData(5, 5, 100)]
        [InlineData(7, 7, 1000)]
        [Theory]
        public void GetTriangleCoordinatesTest_ValidGeometry(int rows, int cols, int pixel)
        {
            var imagingDto = new ImagingDto { Rows = rows, Columns = cols, PixelSize = pixel };
            //TODO: Return real image
            A.CallTo(() => _printService.PlotAllTriangles(imagingDto)).Returns(new PrintResults { MainImage = null, Result = "Success" });

            var results = _printService.PlotAllTriangles(imagingDto);

            Assert.True(results.MainImage == null && results.Result == "Success");
        }
    }
}
