using IronOcr;
using System.Drawing;
using IronSoftware.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string screenshotFilename = @"./screenshot.png";
            string[] filenames = Directory.GetFiles("./");
            bool hasScreenshotInFolder = filenames.Where(o => o.Equals(screenshotFilename)).Any();
            Bitmap bitmap;
            if (!hasScreenshotInFolder)
            {
                Console.WriteLine("screenshot nicht gefunden");
                Environment.Exit(0);
            }

            bitmap = new Bitmap(screenshotFilename);

            IronTesseract ocrRect = new IronTesseract();

            #region Ganzes Bild
            using (OcrInput input = new OcrInput())
            {
                Console.WriteLine("Erkennen von Text in einem Bild");
                input.AddImage(bitmap);

                OcrResult result = ocrRect.Read(input);
                Console.WriteLine("erkannter Text als ein langer String:");
                string Text = result.Text;
                Console.WriteLine(Text);
                Console.WriteLine("_______________________________________________________");

                // Ergebnis der bilderkennung, aufgeteilt in Zeilen, mit diesem Array aus Objekten kann man arbeiten (ausprobieren)
                var lines = result.Lines;

            }
            #endregion

            #region Teil vom Bild
            using (OcrInput input = new OcrInput())
            {
                Console.WriteLine("Erkennen von Text in einem Teilbereich des Bildes");

                // Rechteck zum ausschneiden
                // Dimensions are in pixel
                IronSoftware.Drawing.Rectangle contentArea = new System.Drawing.Rectangle(355, 400, 700, 270);


                var pixelformat = bitmap.PixelFormat; //zwischenspeichern, weil ich keine Ahnung hab was das is... brauch ich aber fürs ausschneiden
                Bitmap? fingerle = bitmap.Clone(contentArea, pixelformat);

                #region bilder speichern (Kontrolle)
                // unter dem Pfad "Texterkennung\ConsoleApp\bin\Debug\net6.0" kannst nachschauen wie der Bereich ausgeschnitten ausschaut
                bitmap.Save(@"_bitmap.png", ImageFormat.Png);
                fingerle.Save(@"_crop.png", ImageFormat.Png);
                #endregion

                input.AddImage(fingerle);
                fingerle.Dispose(); // Wird das Bild nicht mehr benötigt, .Dispose() aufrufen, damit RAM frei wird

                OcrResult result = ocrRect.Read(input);
                Console.WriteLine("erkannter Text als ein langer String:\n");
                string Text = result.Text;
                Console.WriteLine(Text);
                var lines = result.Lines;

            }
            bitmap.Dispose();
            #endregion

        }

    }

}