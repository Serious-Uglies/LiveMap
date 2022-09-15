import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { LiveState } from '../api/types';
import {
  AddAirbasePayload,
  AddObjectPayload,
  InitPayload,
  RemoveObjectPayload,
  UpdateObjectPayload,
  UpdateTimePayload,
} from './types';

const initialState: LiveState = {
  phase: 'uninitialized',
  isRunning: false,
  objects: {},
  airbases: {},
};

function arrayToObject<T>(array: T[], getIndex: (item: T) => string) {
  if (array === null || array === undefined) {
    return null;
  }

  return array.reduce((prev: any, item) => {
    prev[getIndex(item)] = item;
    return prev;
  }, {});
}

function last<T>(array: T[]) {
  return array[array.length - 1];
}

export const liveStateSlice = createSlice({
  name: 'liveState',
  initialState: initialState,
  reducers: {
    setPhase: (state, { payload }) => {
      state.phase = payload;
    },

    init: (state, { payload }: PayloadAction<InitPayload>) => {
      Object.assign(state, {
        ...payload,
        phase: state.phase,
        time: payload.time ? payload.time : undefined,
        objects: arrayToObject(payload.objects, (o) => String(o.id)) || {},
        airbases: arrayToObject(payload.airbases, (o) => o.id) || {},
      });
    },

    end: (state) => {
      Object.assign(state, { ...initialState, phase: state.phase });
    },

    updateTime: (state, { payload }: PayloadAction<UpdateTimePayload[]>) => {
      state.time = last(payload).time;
    },

    addObject: (state, { payload }: PayloadAction<AddObjectPayload[]>) => {
      for (let object of payload) {
        if (state.objects[object.id]) {
          continue;
        }

        state.objects[object.id] = object;
      }
    },

    updateObject: (
      state,
      { payload }: PayloadAction<UpdateObjectPayload[]>
    ) => {
      for (let update of payload) {
        const object = state.objects[update.id];

        if (!object) {
          continue;
        }

        Object.assign(object, update);
      }
    },

    removeObject: (
      state,
      { payload }: PayloadAction<RemoveObjectPayload[]>
    ) => {
      for (let { id } of payload) {
        if (!state.objects[id]) {
          continue;
        }

        delete state.objects[id];
      }
    },

    addAirbase: (state, { payload }: PayloadAction<AddAirbasePayload[]>) => {
      for (let airbase of payload) {
        if (state.airbases[airbase.id]) {
          continue;
        }

        state.airbases[airbase.id] = airbase;
      }
    },
  },
});

export const {
  setPhase,
  init,
  end,
  updateTime,
  addObject,
  updateObject,
  removeObject,
  addAirbase,
} = liveStateSlice.actions;
export default liveStateSlice.reducer;
