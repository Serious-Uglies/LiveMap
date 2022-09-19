import { Action } from 'redux';
import {
  Airbase,
  LiveState,
  MapFeatures,
  MapObject,
  Phase,
  Position,
} from '../api/types';

export interface PayloadAction<T = any> extends Action<string> {
  payload: T;
}

export interface InitPayload {
  phase: Phase;
  isRunning: boolean;
  mapFeatures: MapFeatures;
  missionName?: string;
  theatre?: string;
  time?: string;
}

export interface UpdateTimePayload {
  time: string;
}

export interface UpdatePayload {
  state: LiveState;
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
