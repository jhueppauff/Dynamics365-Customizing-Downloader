using System.Collections.Generic;

using Windows.ApplicationModel.DataTransfer;

namespace Dynamics365CustomizingManager.Models
{
    public class DragDropCompletedData
    {
        public DataPackageOperation DropResult { get; set; }

        public IReadOnlyList<object> Items { get; set; }
    }
}
