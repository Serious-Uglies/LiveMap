import jexl from 'jexl';
import Expression from 'jexl/Expression';
import { t } from 'i18next';

jexl.addFunction('translate', t);
jexl.addTransform('length', (array: any[]) => array.length);
jexl.addTransform('join', (array: any[], joiner: string) => array.join(joiner));
jexl.addTransform('map', (array: any[], identifier: string, fn: string) =>
  array.map((item) => jexl.evalSync(fn, { [identifier]: item }))
);

export type PopupConfig = GroupingPopupConfig | PropertyListPopupConfig;

interface Config {
  type: string;
  allowClustering: boolean;
  priority: number;
}

export interface GroupingPopupConfig extends Config {
  type: 'grouping';
  groupBy: Expression;
  render: Expression;
  orderBy?: Expression;
}

export interface PropertyListPopupConfig extends Config {
  type: 'property-list';
  properties: PropertyListItem[];
}

export interface PropertyListItem {
  label: Expression;
  value: Expression;
}

let cache: { [layer: string]: PopupConfig } | null = null;

export async function getPopups(): Promise<{ [layer: string]: PopupConfig }> {
  try {
    if (cache) {
      return cache;
    }

    const response = await fetch('/api/client/popup').then((res) => res.json());

    for (var [layer, item] of Object.entries(response)) {
      map(layer, item);
    }

    return cache!;
  } catch {
    return {};
  }
}

export async function getPopup(layer: string): Promise<PopupConfig | null> {
  try {
    if (cache && cache[layer]) {
      return cache[layer];
    }

    const response = await fetch(`/api/client/popup/${layer}`).then((res) =>
      res.json()
    );

    return map(layer, response);
  } catch {
    return null;
  }
}

function map(layer: string, popup: any): PopupConfig | null {
  if (cache && cache[layer]) {
    return cache[layer];
  }

  switch (popup.type) {
    case 'grouping':
      popup = {
        ...popup,
        type: 'grouping',
        groupBy: jexl.compile(popup.groupBy),
        render: jexl.compile(popup.render),
        orderBy: popup.orderBy ? jexl.compile(popup.orderBy) : undefined,
      } as GroupingPopupConfig;
      break;

    case 'property-list':
      popup = {
        ...popup,
        type: 'property-list',
        properties: popup.properties.map((p: any) => ({
          label: jexl.compile(p.label),
          value: jexl.compile(p.value),
        })),
      } as PropertyListPopupConfig;
      break;
  }

  if (popup) {
    if (!cache) {
      cache = {};
    }

    cache[layer] = popup;
  }

  return popup;
}
