namespace GeometricProgramming.Services.Concrete
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using GeometricProgramming.Common.Models;
    using GeometricProgramming.Models;
    using GeometricProgramming.Services.Abstract;

    public class PrintService : IPrintService
    {
        public PrintResults PlotAllTriangles(ImagingDto image)
        {
            if (image.IsInvalidImage)
            {
                throw new ArgumentException("Invalid input, valid values for Rows and Columns is between 1-26 and PixelSize >= 10.");
            }

            if (image.IsAnyTriangleInvalid)
            {
                throw new ArgumentException("Invalid Triangle coordinates passed as input.");
            }

            try
            {
                var pixelSize = image.PixelSize;
                var rows = image.Rows;
                var cols = image.Columns;

                var bitMap = new Bitmap(cols * pixelSize, rows * pixelSize);

                // Set white background for bitmap.
                var mainGraphics = Graphics.FromImage(bitMap);
                mainGraphics.Clear(Color.White);

                // Initializing brushes to paint the straight and inverted triangles with different colors.
                var brush = new SolidBrush(Color.FromArgb(91, 155, 213));
                var invertBrush = new SolidBrush(Color.FromArgb(79, 148, 210));

                for (var row = 0; row < rows; row++) // row plotting
                {
                    var i = 1; // used to start the index from 1 for 3rd vertex

                    for (var column = 0; column < cols; column++) // column plotting
                    {
                        var A = new Point(column * pixelSize, row * pixelSize);
                        var B = new Point(column * pixelSize, (row + 1) * pixelSize);
                        var C = new Point(i * pixelSize, (row + 1) * pixelSize);

                        image.Triangles.Add(new TriangleDto { VertexA = new CoordinatesDto { X = A.X, Y = A.Y }, VertexB = new CoordinatesDto { X = B.X, Y = B.Y }, VertexC = new CoordinatesDto { X = C.X, Y = C.Y } });

                        var path = new GraphicsPath();
                        path.AddLine(A, B);
                        path.AddLine(B, C);
                        path.AddLine(C, A);
                        path.CloseFigure();

                        using var straightGraphics = Graphics.FromImage(bitMap);
                        straightGraphics.FillPath(brush, path);

                        var P = new Point(column * pixelSize, row * pixelSize);
                        var Q = new Point(i * pixelSize, row * pixelSize);
                        var R = new Point(i * pixelSize, (row + 1) * pixelSize);

                        image.Triangles.Add(new TriangleDto { VertexA = new CoordinatesDto { X = P.X, Y = P.Y }, VertexB = new CoordinatesDto { X = Q.X, Y = Q.Y }, VertexC = new CoordinatesDto { X = R.X, Y = R.Y } });

                        var invertedPath = new GraphicsPath();
                        invertedPath.AddLine(P, Q);
                        invertedPath.AddLine(Q, R);
                        invertedPath.AddLine(R, P);
                        invertedPath.CloseFigure();

                        using var invertedGraphics = Graphics.FromImage(bitMap);
                        invertedGraphics.FillPath(invertBrush, invertedPath);

                        i++;
                    }
                }

                byte[] imageArray;
                using (var stream = new MemoryStream())
                {
                    bitMap.Save(stream, ImageFormat.Png);
                    imageArray = stream.ToArray();
                }

                return new PrintResults
                {
                    MainImage = imageArray,
                    Result = JsonSerializer.Serialize(image)
                };
            }
            catch (Exception)
            {
                //TODO: Log Exception
                return new PrintResults { MainImage = null, Result = "An error occured while plotting all the triangles." };
            }
        }

        public PrintResults PlotGivenTriangles(ImagingDto imagingDto)
        {
            if (imagingDto.IsAnyTriangleInvalid)
            {
                throw new ArgumentException("Invalid Triangle coordinates passed as input.");
            }

            try
            {
                var bitMap = new Bitmap(imagingDto.Columns * imagingDto.PixelSize, imagingDto.Rows * imagingDto.PixelSize);
                var results = string.Empty;

                // Set white background for bitmap.
                var mainGraphics = Graphics.FromImage(bitMap);
                mainGraphics.Clear(Color.White);

                // Initializing brushes to paint the straight and inverted triangles with different colors.
                var brush = new SolidBrush(Color.FromArgb(91, 155, 213));
                var invertedBrush = new SolidBrush(Color.FromArgb(79, 148, 210));

                foreach (var item in imagingDto.Triangles)
                {
                    var A = new Point(item.VertexA.X, item.VertexA.Y);
                    var B = new Point(item.VertexB.X, item.VertexB.Y);
                    var C = new Point(item.VertexC.X, item.VertexC.Y);

                    var path = new GraphicsPath();
                    path.AddLine(A, B);
                    path.AddLine(B, C);
                    path.AddLine(C, A);
                    path.CloseFigure();

                    using var straightGraphics = Graphics.FromImage(bitMap);
                    if ((B.X + C.X) / imagingDto.PixelSize % 2 == 0)
                    {
                        straightGraphics.FillPath(invertedBrush, path);
                    }
                    else
                    {
                        straightGraphics.FillPath(brush, path);
                    }
                }

                bitMap.Save($"D:\\TrianglesInSquare.png", ImageFormat.Png);
                byte[] imageArray;
                using (var stream = new MemoryStream())
                {
                    bitMap.Save(stream, ImageFormat.Png);
                    imageArray = stream.ToArray();
                }

                return new PrintResults
                {
                    MainImage = imageArray,
                    Result = results
                };
            }
            catch (Exception)
            {
                //TODO: Log Exception
                return new PrintResults { MainImage = null, Result = "An error occured while plotting the given triangles." };
            }
        }

        public PrintResults GetTrianglesPositions(ImagingDto imagingDto)
        {
            if(imagingDto.IsAnyTriangleInvalid)
            {
                throw new ArgumentException("Invalid Triangle coordinates passed as input.");
            }

            try
            {
                var bitMap = new Bitmap(imagingDto.Columns * imagingDto.PixelSize, imagingDto.Rows * imagingDto.PixelSize);
                var results = string.Empty;

                // Set white background for bitmap.
                var mainGraphics = Graphics.FromImage(bitMap);

                // Initializing brushes to paint the straight and inverted triangles with different colors.
                var brush = new SolidBrush(Color.FromArgb(91, 155, 213));
                var invertBrush = new SolidBrush(Color.FromArgb(79, 148, 210));


                foreach (var item in imagingDto.Triangles)
                {
                    var A = new Point(item.VertexA.X, item.VertexA.Y);
                    var B = new Point(item.VertexB.X, item.VertexB.Y);
                    var C = new Point(item.VertexC.X, item.VertexC.Y);

                    var path = new GraphicsPath();
                    path.AddLine(A, B);
                    path.AddLine(B, C);
                    path.AddLine(C, A);
                    path.CloseFigure();

                    mainGraphics.FillPath(brush, path);

                    var upperPosition = (B.X + C.X) / imagingDto.PixelSize;
                    var lowerPosition = (A.X + C.X) / imagingDto.PixelSize;
                    var row = (C.Y) / imagingDto.PixelSize;

                    if (upperPosition % 2 != 0)
                    {
                        mainGraphics.FillPath(brush, path);
                        results += $"The VertexA({A.X},{A.Y}), VertexB({B.X},{B.Y}), VertexC({C.X},{C.Y}) triangle lies in {(char)(row + 64)}{lowerPosition}.{Environment.NewLine}";
                    }
                    else
                    {
                        mainGraphics.FillPath(invertBrush, path);
                        results += $"The VertexA({A.X},{A.Y}), VertexB({B.X},{B.Y}), VertexC({C.X},{C.Y}) triangle lies in {(char)(row + 64)}{upperPosition}.{Environment.NewLine}";
                    }
                }

                byte[] imageArray;
                using (var stream = new MemoryStream())
                {
                    bitMap.Save(stream, ImageFormat.Png);
                    imageArray = stream.ToArray();
                }

                return new PrintResults
                {
                    MainImage = imageArray,
                    Result = results
                };
            }
            catch (Exception)
            {
                //TODO: Log Exception
                return new PrintResults { MainImage = null, Result = "An error occured while getting the triangles' positions." };
            }
        }

        public string GetTriangleCoordinates(GeometryDto geometryDto)
        {
            if (geometryDto.RowChar >= 65 && geometryDto.RowChar <= 90)
            {
                var row = geometryDto.RowChar - 65;
                var column = Convert.ToDecimal(geometryDto.Columns);
                var columnLbound = Math.Floor(column / 2);
                var columnUbound = Math.Ceiling(column / 2);

                if (row + column <= 0 || column < 1 || column > 25 || geometryDto.PixelSize < 10)
                {
                    throw new ArgumentException("Invalid input for Geometry, valid value for Column is between 1-26 and PixelSize >= 10.");
                }

                var isUpper = column % 2 == 0;
                Point A;
                Point B;
                Point C;

                if (isUpper) {
                    A = new Point(((int)columnLbound-1) * geometryDto.PixelSize, (row) * geometryDto.PixelSize);
                    B = new Point((int)columnLbound * geometryDto.PixelSize, row * geometryDto.PixelSize);
                    C = new Point((int)columnUbound * geometryDto.PixelSize, (row + 1) * geometryDto.PixelSize);
                }
                else
                {
                    A = new Point((int)columnLbound * geometryDto.PixelSize, row * geometryDto.PixelSize);
                    B = new Point((int)columnLbound * geometryDto.PixelSize, (row + 1) * geometryDto.PixelSize);
                    C = new Point((int)columnUbound * geometryDto.PixelSize, (row + 1) * geometryDto.PixelSize);
                }

                //return $"The coordinates for {location} are VertexA({A.X},{A.Y}), VertexB({B.X},{B.Y}), VertexC({C.X},{C.Y})";
                return $"The coordinates for {geometryDto.RowChar}{geometryDto.Columns} are VertexA({A.X},{A.Y}), VertexB({B.X},{B.Y}), VertexC({C.X},{C.Y})";

            }
            else
            {
                throw new ArgumentException("Invalid input for Geometry. RowChar can only be between A to Z.");
            }
        }
    }
}