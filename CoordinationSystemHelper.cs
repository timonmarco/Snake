using System;
using System.Drawing;

namespace Snake
{
    public class CoordinationSystemHelper
    {
        public SizeF DisplaySize { get; set; }
        public Size GameFieldSize { get; set; }

        public CoordinationSystemHelper(SizeF displaySize, Size gameFieldSize)
        {
            DisplaySize = displaySize;
            GameFieldSize = gameFieldSize;
        }

        public SizeF FieldSize => GetFieldSize();

        private SizeF GetFieldSize()
        {
            // Errechne die Größe eines einzigen Feldes
            return new SizeF(DisplaySize.Width / GameFieldSize.Width, DisplaySize.Height / GameFieldSize.Height);
        }

        /// <summary>
        /// Controlgröße :1000 x 1000 Pixel
        /// Spielfeld 20 x 20 
        /// Feldgröße 1000 / 20 = 50 x 1000 / 20 = 50 = 50x50
        /// Input = Feld x = 5: y = 5  Output = x = 250 : y = 250
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PointF ToDrawingPoint(Point p)
        {
            SizeF fieldSize = GetFieldSize();
            return new PointF(p.X * fieldSize.Width, p.Y * fieldSize.Height);
        }

        public Point ToCoordPoint(PointF p)
        {
            SizeF fieldSize = GetFieldSize();
            return new Point((int)(p.X / fieldSize.Width), (int)(p.Y / fieldSize.Height));
        }

        public SizeF ToDrawingSize(Size size)
        {
            SizeF fieldSize = GetFieldSize();
            return new SizeF(size.Width * fieldSize.Width, size.Height * fieldSize.Height);
        }

        public Size ToCoordSize(SizeF size)
        {
            throw new NotImplementedException();
        }

        public RectangleF ToDrawingRectangle(Rectangle rect)
        {
            return new RectangleF(ToDrawingPoint(rect.Location), ToDrawingSize(rect.Size));
        }

        public Rectangle ToCoordRectangle(RectangleF rect)
        {
            return new Rectangle(ToCoordPoint(rect.Location), ToCoordSize(rect.Size));
        }
    }
}
