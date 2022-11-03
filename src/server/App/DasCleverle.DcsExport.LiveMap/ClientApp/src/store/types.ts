import { Action } from 'redux';
import { LiveState, MapFeatures, Phase } from '../api/types';

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

export interface UpdatePayload {
  state: LiveState;
}
