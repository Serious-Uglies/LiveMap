import { Action } from 'redux';
import { Airbase, MapObject, Phase, Position } from '../api/types';

export interface PayloadAction<T = any> extends Action<string> {
  payload: T;
}

export interface InitPayload {
  phase: Phase;
  isRunning: boolean;
  objects: MapObject[];
  airbases: Airbase[];
  missionName?: string;
  theatre?: string;
  mapCenter?: string;
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
