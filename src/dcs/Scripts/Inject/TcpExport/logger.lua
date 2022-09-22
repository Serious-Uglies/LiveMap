local mod = {}

local function do_log(fn, msg, ...)
    fn("[TcpExport] " .. string.format(msg, ...))
end

function mod.info(msg, ...)
    do_log(env.info, msg, ...)
end

function mod.warning(msg, ...)
    do_log(env.warning, msg, ...)
end

function mod.error(msg, ...)
    do_log(env.error, msg, ...)
end

return mod