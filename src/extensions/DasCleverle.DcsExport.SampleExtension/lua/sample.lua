-- This is the extension on the DCS side
-- We just add a simple string to any event payload here.
local extension = {}

function extension.extend(objType, object, info)
    return {
        extensionProperty = "my extension property"
    }
end

return extension