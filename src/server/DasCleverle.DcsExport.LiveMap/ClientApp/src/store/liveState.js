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

const last = (array) => array[array.length - 1];

export const liveStateSlice = createSlice({
  name: 'liveState',
  initialState: { ...initialState },
  reducers: {
    setPhase: (state, { payload }) => {
      state.phase = payload;
    },

    init: (state, { payload }) => {
      Object.assign(state, {
        ...(last(payload) || payload),
        phase: state.phase,
        objects: arrayToObject(payload.objects, (o) => o.id) || {},
        airbases: arrayToObject(payload.airbases, (o) => o.id) || {},
      });
    },

    end: (state) => {
      Object.assign(state, { ...initialState, phase: state.phase });
    },

    updateTime: (state, { payload }) => {
      state.time = last(payload).time;
    },

    addObject: (state, { payload }) => {
      for (let object of payload) {
        if (state.objects[object.id]) {
          continue;
        }

        state.objects[object.id] = object;
      }
    },

    updateObject: (state, { payload }) => {
      for (let update of payload) {
        const object = state.objects[update.id];

        if (!object) {
          continue;
        }

        Object.assign(object, update);
      }
    },

    removeObject: (state, { payload }) => {
      for (let { id } of payload) {
        if (!state.objects[id]) {
          continue;
        }

        delete state.objects[id];
      }
    },

    addAirbase: (state, { payload }) => {
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
