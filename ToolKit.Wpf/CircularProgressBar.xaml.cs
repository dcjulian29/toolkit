using System.Windows.Controls;
using Common.Logging;

namespace ToolBox.Wpf
{
    /// <summary>
    /// Interaction logic for CircularProgressBar
    /// </summary>
    public partial class CircularProgressBar : UserControl
    {
        private static ILog _log = LogManager.GetLogger<CircularProgressBar>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularProgressBar"/> class.
        /// </summary>
        public CircularProgressBar()
        {
            InitializeComponent();
        }
    }
}
