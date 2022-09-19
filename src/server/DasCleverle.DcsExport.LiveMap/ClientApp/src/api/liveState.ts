import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Dispatch } from '@reduxjs/toolkit';
import { RootState } from '../store';
import { init, setPhase, update } from '../store/liveState';
import { InitPayload } from '../store/types';

interface UpdateRequest {
  state: InitPayload;
}

const apiEndpoint = '/api/state';
const hubEndpoint = '/hub/state';

export const connect =
  () => async (dispatch: Dispatch, getState: () => RootState) => {
    const connection = new HubConnectionBuilder()
      .withUrl(hubEndpoint)
      .withAutomaticReconnect([1000, 2000, 5000, 5000])
      .configureLogging(LogLevel.Information)
      .build();

    connection.on('Update', handleUpdate(dispatch));
    connection.onclose(handleConnectionClosed(dispatch));
    connection.onreconnecting(handleReconnecting(dispatch));
    connection.onreconnected(handleReconnected(dispatch));

    dispatch(setPhase('loading'));

    const liveState = await getLiveState();

    if (!liveState) {
      dispatch(setPhase('error'));
      return;
    }

    dispatch(init(liveState));

    try {
      await connection.start();
      dispatch(setPhase('loaded'));
    } catch (err) {
      console.log(err);
      dispatch(setPhase('error'));
    }
  };

const getLiveState = async (): Promise<InitPayload | null> => {
  try {
    return await fetch(apiEndpoint).then((res) => res.json());
  } catch (err) {
    console.log(err);
    return null;
  }
};

const handleUpdate = (dispatch: Dispatch) => (request: UpdateRequest) => {
  dispatch(update(request.state));
};

const handleConnectionClosed = (dispatch: Dispatch) => () => {
  dispatch(setPhase('error'));
};

const handleReconnecting = (dispatch: Dispatch) => () => {
  dispatch(setPhase('reconnecting'));
};

const handleReconnected = (dispatch: Dispatch) => () => {
  dispatch(setPhase('loaded'));
};
