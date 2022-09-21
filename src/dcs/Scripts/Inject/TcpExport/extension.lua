local logger = require("logger")
local config = require("config")

local extension = {}
local extensions = {}
local waitFors = {}

local function loadModules()
    for i = 1, #config.extensions do
        local name = config.extensions[i]

        logger.info("Loading extension %s", name)
        local ext = require("extensions." .. name)

        if type(ext.waitFor) == "function" then
            table.insert(waitFors, { name = name, waitFor = ext.waitFor })
        end

        extensions[name] = ext
    end
end

local function init()
    extension.extensions = {}

    for name, ext in pairs(extensions) do
        logger.info("Initializing extension %s", name)
        local loaded, error = pcall(ext.init)

        if not loaded then
            logger.error("Failed to load extension %s", name)
            logger.error("%s", error)
        else
            extension.extensions[name] = ext
        end
    end
end

local function wait(callback, t)
    local result = true

    for i = 1, #waitFors do
        local r = waitFors[i].waitFor()

        if not r then
            logger.info("Extension %s is not ready", waitFors[i].name)
        end

        result = result and r
    end

    if result then
        init()
        callback()
        return nil
    end

    return t + 1
end

function extension.init(callback)
    loadModules()

    if #waitFors == 0 then
        init()
        callback()
    else
        local names = {}

        for i = 1, #waitFors do
            names[i] = waitFors[i].name
        end

        logger.info("Waiting for extensions %s to get ready", table.concat(names, ", "))
        timer.scheduleFunction(wait, callback, timer.getTime() + 1)
    end
end

function extension.registerEvents(eventHandlers)
    for _, ext in pairs(extensions) do
        if ext.registerEvents then
            local handlers = ext.registerEvents()

            if handlers then
                for event, handler in pairs(handlers) do
                    if eventHandlers[event] then
                        table.insert(eventHandlers[event], handler)
                    else
                        eventHandlers[event] = { handler }
                    end
                end
            end
        end
    end

    return eventHandlers
end

return extension
