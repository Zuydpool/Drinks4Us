using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drinks4Us.Views.Main
{
    public class MainFlyoutPageFlyoutMenuItem
    {
        public MainFlyoutPageFlyoutMenuItem()
        {
            TargetType = typeof(MainFlyoutPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }

        public string StyleClass { get; set; }

        //TODO: Custom action property
        public Action CustomAction { get; set; }

        public string IconSource { get; set; }
    }
}