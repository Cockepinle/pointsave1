using POINT.ViewModel.Helpers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing.Imaging;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace POINT.ViewModel
{
    internal class ViewModelStarts: BindingHelper
    {
        public ICommand Down { get; set; }
        public ICommand Up { get; set; }
        public ICommand Move { get; set; }
        public ICommand UpdateColorCommand { get; set; }
        public ICommand SizeSelectionCommandSmall { get; set; }
        public ICommand SizeSelectionCommandMedium { get; set; }
        public ICommand SizeSelectionCommandLarge { get; set; }
        public ICommand ShapeSelectionCommandShedding { get; set; }
        public ICommand ShapeSelectionCommandCircle { get; set; }
        public ICommand ShapeSelectionCommandSquare { get; set; }
        public ICommand Save { get; set; }
        public WindowStart Window { get; set; }

        private WindowStart startWindow;
        public ICommand Back { get; set; }
        private System.Windows.Controls.Canvas currentCanvas;

        public enum ShapeType
        {
            Circle,
            Square,
            Line 
        }

        System.Windows.Point currentPoint = new System.Windows.Point();
        
        private bool isDrawing = false;

        private ShapeType currentShape = ShapeType.Line; 

        private bool isRadioButtonChecked = false;

        private System.Windows.Media.Color selectedColor; 

        public double lineThickness = 1;
        public ICommand ColorSelectionCommand
        {
            get
            {
                return new RelayCommand<System.Windows.Media.Color>((color) =>
                {
                    selectedColor = color; 
                });
            }
        }

        public ViewModelStarts(WindowStart Window)
        {
            selectedColor = Colors.Black;
            Down = new BindableCommand(DownMoment);
            Up = new BindableCommand(UpMoment);
            Move = new BindableCommand(MoveMoment);
            SizeSelectionCommandSmall = new BindableCommand(CheckedSmall);
            SizeSelectionCommandMedium = new BindableCommand(CheckedMedium);
            SizeSelectionCommandLarge = new BindableCommand(CheckedLarge);
            ShapeSelectionCommandShedding = new BindableCommand(CheckedShedding);
            ShapeSelectionCommandCircle = new BindableCommand(CheckedCircle);
            ShapeSelectionCommandSquare = new BindableCommand(CheckedSquare);
            Save = new BindableCommand(SaveMoment);
            Back = new BindableCommand(BackMoment);
            this.Window = Window;
        }

        public void DownMoment(object canvas)
        {
            isDrawing = true;
            currentPoint = Mouse.GetPosition((System.Windows.Controls.Canvas)canvas);
            currentCanvas = (System.Windows.Controls.Canvas)canvas; 
        }
        public void UpMoment(object canvas)
        {
            isDrawing = false;
        }


        public void MoveMoment(object canvas)
        {
            var currentCanvas = (System.Windows.Controls.Canvas)canvas;

            if (isDrawing)
            {
                System.Windows.Point newPoint = Mouse.GetPosition(currentCanvas);

                System.Windows.Shapes.Shape shape = null;

                switch (currentShape)
                {
                    case ShapeType.Line:
                        shape = new System.Windows.Shapes.Line
                        {
                            X1 = currentPoint.X,
                            Y1 = currentPoint.Y,
                            X2 = newPoint.X,
                            Y2 = newPoint.Y,
                            Stroke = new SolidColorBrush(selectedColor),
                            StrokeThickness = lineThickness
                        };
                        break;
                    case ShapeType.Circle:
                        shape = new Ellipse
                        {
                            Width = 50, 
                            Height = 50, 
                            Stroke = new SolidColorBrush(selectedColor),
                            StrokeThickness = lineThickness
                        };
                        System.Windows.Controls.Canvas.SetLeft(shape, currentPoint.X);
                        System.Windows.Controls.Canvas.SetTop(shape, currentPoint.Y);
                        break;
                    case ShapeType.Square:
                        shape = new System.Windows.Shapes.Rectangle
                        {
                            Width = 50, 
                            Height = 50, 
                            Stroke = new SolidColorBrush(selectedColor),
                            StrokeThickness = lineThickness
                        };
                        System.Windows.Controls.Canvas.SetLeft(shape, currentPoint.X);
                        System.Windows.Controls.Canvas.SetTop(shape, currentPoint.Y);
                        break;
                }

                currentCanvas.Children.Add(shape);

                currentPoint = newPoint;

            }
        }

        public void CheckedSmall(object canvas)
        {
            isRadioButtonChecked = true;
            lineThickness = 1;
            MoveMoment(canvas);
        }
        public void CheckedMedium(object canvas)
        {
            isRadioButtonChecked = true;
            lineThickness = 10;
            MoveMoment(canvas);
        }
        public void CheckedLarge(object canvas)
        {
            isRadioButtonChecked = true;
            lineThickness = 20;
            MoveMoment(canvas);
        }
        public void CheckedShedding(object canvas)
        {
            isRadioButtonChecked = true;
            currentShape = ShapeType.Line;
        }
        public void CheckedCircle(object canvas)
        {
            isRadioButtonChecked = true;
            currentShape = ShapeType.Circle;

        }
        public void CheckedSquare(object canvas)
        {
            isRadioButtonChecked = true;
            currentShape = ShapeType.Square;
        }
        public void SaveMoment(object canvas)
        {
            BitmapSource bitmapSource = CaptureCanvas((UIElement)canvas);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string fileName = $"your_image_name_{DateTime.Now.Ticks}.png"; 

            string filePath = System.IO.Path.Combine(desktopPath, fileName);

            Bitmap bitmap = new Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            bitmapSource.CopyPixels(Int32Rect.Empty, data.Scan0, data.Stride * data.Height, data.Stride);
            bitmap.UnlockBits(data);

            bitmap.Save(filePath, ImageFormat.Png);

            WindowSave windowSave = new WindowSave();
            windowSave.Show();
            Window.Close();
        }
        public BitmapSource CaptureCanvas(UIElement canvas)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }

            rtb.Render(dv);
            return rtb;
        }

        public void BackMoment(object canvas)
        {
            if (currentCanvas != null && currentCanvas.Children.Count > 0)
            {
                int elementsToRemove = 10;
                for (int i = 0; i < elementsToRemove; i++)
                {
                    if (currentCanvas.Children.Count > 0)
                    {
                        currentCanvas.Children.Remove(currentCanvas.Children[currentCanvas.Children.Count - 1]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            }
    }
}

