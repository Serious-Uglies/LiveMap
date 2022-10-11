local mod = {}

local logger = require("logger")
local config = require("config")

local modules = {}
local waitFors = {}

local function loadModules()
    for i = 1, #config.extensions do
        local name = config.extensions[i]
        local module = require("extensions." .. name)

        if type(module.waitFor) == "function" then
            table.insert(waitFors, { name = name, waitFor = module.waitFor })
        end

        modules[name] = module
    end
end

local function init()
    mod.extensions = {}

    for name, module in pairs(modules) do
        logger.info("Initializing extension %q", name)
        local loaded, error = pcall(module.init)

        if not loaded then
            logger.error("Failed to load extension %q", name)
            logger.error("%s", error)
        else
            mod.extensions[name] = module
        end
    end
end

local function wait(callback, t)
    local result = true

    for i = 1, #waitFors do
        local r = waitFors[i].waitFor()

        if not r then
            logger.info("Extension %q is not ready", waitFors[i].name)
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

function mod.init(callback)
    logger.info("Loading extensions")

    loadModules()

    if #waitFors == 0 then
        init()
        callback()
    else
        local waitForNames = {}
        local names = {}

        for i = 1, #waitFors do
            waitForNames[i] = string.format("%q", waitFors[i].name)
        end

        for name, _ in pairs(modules) do
            table.insert(names, string.format("%q", name))
        end

        logger.info("Waiting for extensions %s to get ready", table.concat(waitForNames, ", "))
        timer.scheduleFunction(wait, function()
            logger.info("All extensions are ready. Loaded extensions: %s", table.concat(names, ", "))
            callback()
        end, timer.getTime() + 1)
    end
end

function mod.registerEvents(eventHandlers)
    for _, ext in pairs(modules) do
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

function mod.call(fn, callback, ...)
    for name, ext in pairs(mod.extensions) do
        if ext[fn] and type(ext[fn]) == "function" then
            local success, result = pcall(ext[fn], ...)

            if not success then
                logger.error("An error ocurred in function '%s.%s'", name, fn)
                logger.error("%s", result)
            elseif callback and type(callback) == "function" then
                callback(result, name)
            end
        end
    end
end

return mod
