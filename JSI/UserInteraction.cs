using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppBlazor.JSI
{
    public class UserInteraction
    {
        public void ClearViewer(IJSRuntime js, string iFrame_id)
        {
            js.InvokeVoidAsync("ClearPDFViewer",
                iFrame_id
                );
        }

        public void FocusToElementWithID(IJSRuntime js, string id)
        {
            js.InvokeVoidAsync("FocusToElementWithID",
                id
                );
        }
    }
}
