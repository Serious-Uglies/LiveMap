local log = {}

local function do_log(fn, msg, ...)
    fn("[TcpExport] " .. string.format(msg, ...))
end

function log.debug(msg, ...)
    do_log(env.debug, msg, ...)
end

function log.info(msg, ...)
    do_log(env.info, msg, ...)
end

function log.warning(msg, ...)
    do_log(env.warning, msg, ...)
end

function log.error(msg, ...)
    do_log(env.error, msg, ...)
end

return log