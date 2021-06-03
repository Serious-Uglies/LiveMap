import { configureStore } from '@reduxjs/toolkit';
import liveStateReducer from './liveState';

export default configureStore({
  reducer: {
    liveState: liveStateReducer,
  },
});
