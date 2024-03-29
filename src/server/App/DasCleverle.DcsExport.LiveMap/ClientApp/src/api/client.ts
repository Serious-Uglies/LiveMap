import jexl from 'jexl';
import Expression from 'jexl/Expression';
import { t } from 'i18next';
import { AnyLayer } from 'mapbox-gl';

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
  properties: PropertyListPopupItem[];
}

export interface PropertyListPopupItem {
  id: string;
  label: Expression;
  display: PropertyDisplay;
}

export type PropertyDisplay = ScalarPropertyDisplay | ListPropertyDisplay;

export interface ScalarPropertyDisplay {
  type: 'scalar';
  value: Expression;
}

export interface ListPropertyDisplay {
  type: 'list';
  selector: Expression;
  value: Expression;
}

export interface IconInfo {
  id: string;
  url: string;
}

let layerCache: AnyLayer[] | null = null;
let popupCache: { [layer: string]: PopupConfig } | null = null;

export async function getLayers(): Promise<AnyLayer[]> {
  try {
    if (layerCache) {
      return layerCache;
    }

    const response = await fetch('/api/client/layers').then((res) =>
      res.json()
    );
    layerCache = response;

    return response!;
  } catch {
    return [];
  }
}

export async function getPopups(): Promise<{ [layer: string]: PopupConfig }> {
  try {
    if (popupCache) {
      return popupCache;
    }

    const response = await fetch('/api/client/popup').then((res) => res.json());

    for (var [layer, item] of Object.entries(response)) {
      mapPopups(layer, item);
    }

    return popupCache!;
  } catch {
    return {};
  }
}

export async function getPopup(layer: string): Promise<PopupConfig | null> {
  try {
    if (popupCache && popupCache[layer]) {
      return popupCache[layer];
    }

    const response = await fetch(`/api/client/popup/${layer}`).then((res) =>
      res.json()
    );

    return mapPopups(layer, response);
  } catch {
    return null;
  }
}

function mapPopups(layer: string, popup: any): PopupConfig | null {
  if (popupCache && popupCache[layer]) {
    return popupCache[layer];
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
          id: p.id,
          label: jexl.compile(p.label),
          display: getPropertyDisplay(p.display),
        })),
      } as PropertyListPopupConfig;
      break;
  }

  if (popup) {
    if (!popupCache) {
      popupCache = {};
    }

    popupCache[layer] = popup;
  }

  return popup;
}

function getPropertyDisplay(display: any) {
  switch (display.type) {
    case 'scalar':
      return {
        type: 'scalar',
        value: jexl.compile(display.value),
      } as ScalarPropertyDisplay;

    case 'list':
      return {
        type: 'list',
        selector: jexl.compile(display.selector),
        value: jexl.compile(display.value),
      } as ListPropertyDisplay;
  }
}
