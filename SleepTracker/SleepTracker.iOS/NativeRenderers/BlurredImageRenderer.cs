using CoreImage;
using SleepTracker.Controls;
using SleepTracker.iOS.NativeRenderers;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BlurredImage), typeof(BlurredImageRenderer))]

namespace SleepTracker.iOS.NativeRenderers
{
    public class BlurredImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            if (Control == null)
            {
                SetNativeControl(new BlurredImageView
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                    ClipsToBounds = true
                });
            }

            base.OnElementChanged(e);
        }

        private class BlurredImageView : FormsUIImageView
        {
            public override UIImage Image
            {
                get { return base.Image; }
                set
                {
                    // This may take up to a second so don't block the UI thread.
                    Task.Run(() =>
                    {
                        using (var context = CIContext.Create())
                        using (var inputImage = CIImage.FromCGImage(value.CGImage))
                        using (var filter = new CIGaussianBlur() { InputImage = inputImage, Radius = 5 })
                        using (var resultImage = context.CreateCGImage(filter.OutputImage, inputImage.Extent))
                        {
                            InvokeOnMainThread(() => base.Image = new UIImage(resultImage));
                        }
                    });
                }
            }
        }
    }
}