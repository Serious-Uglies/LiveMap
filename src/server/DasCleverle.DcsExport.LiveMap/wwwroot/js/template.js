(function ($) {
  const evalExpression = (expr, params) => {
    const keys = Object.keys(params);
    const values = Object.values(params);
    const fn = new Function(
      `return function(${keys.join(',')}) { return ${expr}; }`
    )();

    return fn.apply(fn, values);
  };

  const evalParams = ($el, params) => {
    const paramRe = /\$\{(.+?)\}/g;
    return he
      .decode($el.html())
      .replace(paramRe, (_, expr) => evalExpression(expr, params));
  };

  const evalConditional = ($el, params) => {
    const expr = $el.attr('$if');
    const result = evalExpression(expr, params);

    $el.removeAttr('$if');

    return result;
  };

  const evalLoop = ($el, params) => {
    const match = /(\w+) of (\w+)/.exec($el.attr('$for'));
    const variable = match[1];
    const iteratable = params[match[2]];

    $el.removeAttr('$for');

    const template = $el.html();
    const result = [];

    for (const item of iteratable) {
      const innerParams = Object.assign({}, params, {
        [variable]: item,
      });

      result.push(evalTemplate(template, innerParams));
    }

    return result.join('');
  };

  const evalTemplate = (template, params) => {
    const children = $('<div />').html(template).children();

    if (children.length == 0) {
      return template;
    }

    const result = [];

    children.each((_, el) => {
      const $el = $(el);

      if ($el.attr('$if') && !evalConditional($el, params)) {
        return;
      }

      if ($el.attr('$for')) {
        $el.html(evalLoop($el, params));
      }

      $el.html(evalParams($el, params));

      const html = $el.html();

      if (html.includes('$for') || html.includes('$if')) {
        $el.html(evalTemplate(html, params));
      }

      result.push(el.outerHTML);
    });

    return result.join('');
  };

  $.fn.template = function (template, params, handlers) {
    const content = $(`script[data-template="${template}"]`).html();
    const target = $(this);

    target.html(evalTemplate(content, params));

    if (handlers) {
      target.find(`[data-event][data-handler]`).each((_, el) => {
        const event = el.dataset.event;
        const handlerName = el.dataset.handler;
        const handler = handlers[handlerName];

        if (!handler) {
          return;
        }

        el.addEventListener(event, handler.bind(handlers));
      });
    }

    return target;
  };
})($);
