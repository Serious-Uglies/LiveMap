import { Action } from 'redux';
import { Map, Airbase, MapObject, Phase, Position } from '../api/types';

export interface PayloadAction<T = any> extends Action<string> {
  payload: T;
}

export interface InitPayload {
  phase: Phase;
  isRunning: boolean;
  objects?: Map<MapObject>;
  airbases?: Map<Airbase>;
  missionName?: string;
  theatre?: string;
  time?: string;
}

export interface UpdateTimePayload {
  time: string;
}

export type AddObjectPayload = MapObject;

export interface UpdateObjectPayload {
  id: number;
  position?: Position;
}

export interface RemoveObjectPayload {
  id: number;
}

export type AddAirbasePayload = Airbase;
