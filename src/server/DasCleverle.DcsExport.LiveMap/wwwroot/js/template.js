(function ($) {
  const functions = {};

  $.template = {
    addFunction: function (name, fn) {
      functions[name] = fn;
    },
  };

  const evalExpression = (expr, params) => {
    params = Object.assign({}, params, functions);

    const keys = Object.keys(params);
    const values = Object.values(params);
    const fn = new Function(
      `return function(${keys.join(',')}) { return ${expr}; }`
    )();

    return fn.apply(fn, values);
  };

  const evalParams = (html, params) => {
    const paramRe = /\$\{(.+?)\}/g;

    return he.decode(html).replace(paramRe, (_, expr) => {
      const result = evalExpression(expr, params);

      if (result === null || result === undefined) {
        return '';
      }

      return result;
    });
  };

  const evalConditional = (node, params) => {
    const attribute = node.attributes && node.attributes['$if'];

    if (!attribute) {
      return true;
    }

    node.removeAttribute('$if');
    return evalExpression(attribute.value, params);
  };

  const evalLoop = (node, params) => {
    const attribute = node.attributes && node.attributes['$for'];

    if (!attribute) {
      return null;
    }

    node.removeAttribute('$for');

    const match = /(\w+) of (\w+)/.exec(attribute.value);
    const variable = match[1];
    const iteratable = params[match[2]];

    const fragment = document.createDocumentFragment();

    for (const item of iteratable) {
      const innerParams = Object.assign({}, params, {
        [variable]: item,
      });

      fragment.appendChild(evalTemplate(node, innerParams, false));
    }

    const clone = node.cloneNode();
    clone.append(fragment);

    return clone;
  };

  const evalTemplate = (template, params, isChild) => {
    if (template.nodeType === Node.TEXT_NODE) {
      template.data = evalParams(template.data, params);
      return template;
    }

    if (template.nodeType !== Node.ELEMENT_NODE) {
      return null;
    }

    const element = isChild
      ? template
      : document.createRange().createContextualFragment(template.innerHTML);

    if (!evalConditional(element, params)) {
      return null;
    }

    const loop = evalLoop(element, params);

    if (loop) {
      return loop;
    }

    const children = Array.from(element.childNodes);

    for (let child of children) {
      const newChild = evalTemplate(child, params, true);

      if (!newChild) {
        element.removeChild(child);
      } else if (newChild !== child) {
        element.insertBefore(newChild, child);
        element.removeChild(child);
      }
    }

    return element;
  };

  $.fn.template = function (template, params, handlers) {
    const container = document.querySelector(
      `script[data-template="${template}"]`
    );

    this.html(evalTemplate(container, params));

    if (handlers) {
      this.find(`[data-event][data-handler]`).each((_, el) => {
        const event = el.dataset.event;
        const handlerName = el.dataset.handler;
        const handler = handlers[handlerName];

        if (!handler) {
          return;
        }

        el.addEventListener(event, handler.bind(handlers));
      });
    }

    return this;
  };
})($);
