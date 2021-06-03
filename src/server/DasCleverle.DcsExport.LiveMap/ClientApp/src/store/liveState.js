import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  isLoading: false,
  isConnected: false,
  hasError: false,

  isRunning: false,
  objects: {},
  airbases: {},
  missionName: null,
  theatre: null,
  mapCenter: null,
  time: null,
};

const arrayToObject = (array, getIndex) => {
  if (array === null || array === undefined) {
    return null;
  }

  return array.reduce((prev, item) => {
    prev[getIndex(item)] = item;
    return prev;
  }, {});
};

export const liveStateSlice = createSlice({
  name: 'liveState',
  initialState: { ...initialState },
  reducers: {
    setLoading: (state, { payload }) => {
      state.isLoading = payload;
    },

    setConnected: (state, { payload }) => {
      state.isConnected = payload;
    },

    setError: (state, { payload }) => {
      state.hasError = payload;
    },

    init: (state, { payload }) => {
      state.isRunning = payload.isRunning;
      state.missionName = payload.missionName;
      state.theatre = payload.theatre;
      state.mapCenter = payload.mapCenter;
      state.objects = arrayToObject(payload.objects, (o) => o.id) || {};
      state.airbases = arrayToObject(payload.airbases, (o) => o.id) || {};
    },

    end: (state) => {
      Object.assign(state, { ...initialState });
    },

    updateTime: (state, { payload: { time } }) => {
      state.time = time;
    },

    addObject: (state, { payload }) => {
      if (state.objects[payload.id]) {
        return;
      }

      state.objects[payload.id] = payload;
    },

    updateObject: (state, { payload }) => {
      const object = state.objects[payload.id];

      if (!object) {
        return;
      }

      Object.assign(object, payload);
    },

    removeObject: (state, { payload: { id } }) => {
      if (!state.objects[id]) {
        return;
      }

      delete state.objects[id];
    },

    addAirbase: (state, { payload }) => {
      if (state.airbases[payload.id]) {
        return;
      }

      state.airbases[payload.id] = payload;
    },
  },
});

export const {
  setLoading,
  setError,
  setConnected,
  init,
  end,
  updateTime,
  addObject,
  updateObject,
  removeObject,
  addAirbase,
} = liveStateSlice.actions;
export default liveStateSlice.reducer;
