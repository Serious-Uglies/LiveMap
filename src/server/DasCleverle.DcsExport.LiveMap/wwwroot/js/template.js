(function ($) {
  const replaceParams = (content, params) => {
    const paramRe = /\$\{(\w+)(\.(\w+))?\}/g;

    return content.replace(paramRe, (_, name, __, property) => {
      let value = params[name];

      if (property !== undefined) {
        value = value[property];
      }

      return value;
    });
  };

  const runLoops = (content, params) => {
    if (!content.includes('$for')) {
      return content;
    }

    const forRe = /(\w+) of (\w+)/;
    const executeLoop = (el) => {
      const match = forRe.exec(el.attributes['$for'].value);
      const variable = match[1];
      const arrayName = match[2];

      const $el = $(el);
      const tpl = $el.html();
      const array = params[arrayName];

      const result = [];

      array.forEach((item) => {
        const innerParams = Object.assign({}, params, {
          [variable]: item,
        });

        result.push(replaceParams(tpl, innerParams));
      });

      $el.removeAttr('$for');
      $el.html(result.join(''));
    };

    const $content = $('<div />').html(content);

    if ($content.attr('$for')) {
      executeLoop($content[0]);
    } else {
      $content.find('[\\$for]').each((_, el) => {
        executeLoop(el);
      });
    }

    return $content.html();
  };

  $.fn.template = function (template, handlers, params) {
    const content = $(`script[data-template="${template}"]`).html();
    const target = $(this);

    const withLoops = runLoops(content, params);
    const withParams = replaceParams(withLoops, params);

    target.html(withParams);

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
