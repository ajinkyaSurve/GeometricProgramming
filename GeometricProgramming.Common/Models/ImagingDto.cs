namespace GeometricProgramming.Common.Models
{
    using System.Collections.Generic;

    public class ImagingDto : GeometryDto
    {
        public List<TriangleDto> Triangles { get; set; }

        public bool IsAnyTriangleInvalid =>
            Triangles.Exists(x =>
            !(x.VertexB.Y - x.VertexA.Y == PixelSize && x.VertexB.X - x.VertexA.X == 0 && x.VertexC.Y == x.VertexB.Y && x.VertexC.X - x.VertexB.X == PixelSize) &&
            !(x.VertexB.Y - x.VertexA.Y == 0 && x.VertexB.X - x.VertexA.X == PixelSize && x.VertexC.Y - x.VertexB.Y == PixelSize && x.VertexC.X - x.VertexB.X == 0));

        public bool IsInvalidImage => Rows + Columns <= 0 || Rows > 26 || Columns < 1 || Columns > 26 || PixelSize < 10;

    }
}
