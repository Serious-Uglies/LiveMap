using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using static DasCleverle.DcsExport.Client.Abstractions.Expressions.JexlExtensions;

namespace DasCleverle.DcsExport.SampleExtension;

// The IPopupExtender interface allows us to modify any popup defined by the core or even other extensions.
// This interface is generic, so we can define which type of popup to extend.
// Here we add our custom property to the popup shown, when clicking on an airbase.
public class SamplePopupExtender : IPopupExtender<PropertyListPopup>
{
    // Determines the map layer whose popups will be extended.
    // Here we extend the 'airbases' layer.
    public string Layer => Layers.Airbases;

    // This method is called by the core with the popup as it was configured by its associated IPopupProvider.
    public IPopup Extend(PropertyListPopup popup)
    {
        // Add a new scalar property to the end of the property list.
        // The 'Scalar' method is generic, so we can supply our custom 
        // property type for ease of use. There is also a non-generic
        // version that uses indexers instead of statically typed members.
        // NOTE: The lambda expressions below are not not executed on the server,
        // instead they are transpiled to a JEXL (https://www.npmjs.com/package/jexl)
        // which can be interpreted by the JavaScript code on the client.
        return popup.Add(PropertyListPopupItem.Scalar<SampleProperties>(
            // The unique ID of the property. 
            // It is a good practice to prefix this to 
            // prevent conflicts with other extensions.
            id: "sample.sample", 

            // The label to show for the property. 
            // Most of the time this is only a call to the 'Translate' method.
            // Note the 'using static' on line 3. This allows to not have to 
            // write 'JexlExtensions.Translate' everywhere.
            label: o => Translate("extension.sample.popupProperty"), 

            // The value of the property
            value: o => o.SampleFeatureProperty
        ));
    }
}
