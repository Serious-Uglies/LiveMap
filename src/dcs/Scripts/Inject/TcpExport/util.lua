local util = {}

local function serializeValue(s)
    if s == nil then
        return "\"\""
    else
        if ((type(s) == 'number') or (type(s) == 'boolean') or (type(s) == 'function') or (type(s) == 'table') or (type(s) == 'userdata') ) then
            return tostring(s)
        elseif type(s) == 'string' then
            return string.format('%q', s)
        end
    end
end

function util.serialize(name, value, level)
    local result = {}

    if level == nil then level = "" end
    if level ~= "" then level = level.."  " end

    table.insert(result, level .. name .. " = ")

    if type(value) == "table" then
        table.insert(result, "\n"..level.."{\n")

        for k,v in pairs(value) do -- serialize its fields
            local key
            if type(k) == "number" then
                key = string.format("[%s]", k)
            else
                key = string.format("[%q]", k)
            end

            table.insert(result, util.serialize(key, v, level.."  "))
        end

        if level == "" then
            table.insert(result, level.."} -- end of "..name.."\n")
        else
            table.insert(result, level.."}, -- end of "..name.."\n")
        end
    else
        table.insert(result, serializeValue(value) ..  ",\n")
    end

    return table.concat(result)
end

function util.serializeSafe(name, value, visited)
    local result = {}
    visited = visited or {}       -- initial value

    if type(value) == 'string' or type(value) == 'number' or type(value) == 'table' or type(value) == 'boolean' then

        table.insert(result, name .. " = ")

        if type(value) == "table" then
            if visited[value] then    -- value already saved?
                table.insert(result, visited[value] .. "\n")
            else
                visited[value] = name   -- save name for next time

                table.insert(result, "{}\n")

                for k,v in pairs(value) do      -- save its fields
                    local fieldname = string.format("%s[%s]", name, serializeValue(k))
                    table.insert(result, util.serializeSafe(fieldname, v, visited))
                end
            end
        else
            table.insert(result, serializeValue(value) ..  "\n")
        end

        return table.concat(result)
    else
        return ""
    end
end

return util