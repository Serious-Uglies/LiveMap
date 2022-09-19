import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { LiveState } from '../api/types';
import { InitPayload } from './types';

const initialState: LiveState = {
  phase: 'uninitialized',
  isRunning: false,
  mapFeatures: {},
};

export const liveStateSlice = createSlice({
  name: 'liveState',
  initialState: initialState,
  reducers: {
    setPhase: (state, { payload }) => {
      state.phase = payload;
    },

    init: (state, { payload }: PayloadAction<InitPayload>) => {
      state.isRunning = payload.isRunning;
      state.mapFeatures = payload.mapFeatures;
      state.missionName = payload.missionName;
      state.theatre = payload.theatre;
      state.time = payload.time;
    },

    end: (state) => {
      Object.assign(state, { ...initialState, phase: state.phase });
    },

    update: (state, { payload }: PayloadAction<InitPayload>) => {
      state.isRunning = payload.isRunning;
      state.mapFeatures = payload.mapFeatures;
      state.missionName = payload.missionName;
      state.theatre = payload.theatre;
      state.time = payload.time;
    },
  },
});

export const { setPhase, init, end, update } = liveStateSlice.actions;
export default liveStateSlice.reducer;
