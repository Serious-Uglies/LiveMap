import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  phase: null,
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
    setPhase: (state, { payload }) => {
      state.phase = payload;
    },

    init: (state, { payload }) => {
      Object.assign(state, {
        ...payload,
        phase: state.phase,
        objects: arrayToObject(payload.objects, (o) => o.id) || {},
        airbases: arrayToObject(payload.airbases, (o) => o.id) || {},
      });
    },

    end: (state) => {
      Object.assign(state, { ...initialState, phase: state.phase });
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
