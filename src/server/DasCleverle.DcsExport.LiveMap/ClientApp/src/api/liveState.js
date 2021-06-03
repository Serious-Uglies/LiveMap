import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import {
  addAirbase,
  addObject,
  setConnected,
  end,
  init,
  removeObject,
  setError,
  setLoading,
  updateObject,
  updateTime,
} from '../store/liveState';

const stateEndpoint = '/api/state';
const hubEndpoint = '/hub/livemap';

const eventActionMap = {
  Init: init,
  MissionEnd: end,
  Time: updateTime,
  AddObject: addObject,
  UpdateObject: updateObject,
  RemoveObject: removeObject,
  AddAirbase: addAirbase,
};

export const connect = () => async (dispatch, getState) => {
  const { isConnected, hasError } = getState().liveState;

  if (isConnected || hasError) {
    return;
  }

  const connection = new HubConnectionBuilder()
    .withUrl(hubEndpoint)
    .withAutomaticReconnect([1000, 2000, 5000, 5000])
    .configureLogging(LogLevel.Information)
    .build();

  connection.on('Event', handleEvent(dispatch));
  connection.onclose(handleConnectionClosed(dispatch));
  connection.onreconnecting(handleReconnecting(dispatch));
  connection.onreconnected(handleReconnected(dispatch));

  dispatch(setLoading(true));

  const liveState = await getLiveState(dispatch);

  if (!liveState) {
    dispatch(setError(true));
    return;
  }

  dispatch(init(liveState));

  try {
    await connection.start();
    dispatch(setConnected(true));
  } catch (err) {
    console.log(err);
    dispatch(setError(true));
  } finally {
    dispatch(setLoading(false));
  }
};

const getLiveState = async () => {
  try {
    return await fetch(stateEndpoint).then((res) => res.json());
  } catch (err) {
    console.log(err);
    return null;
  }
};

const handleEvent =
  (dispatch) =>
  ({ event: { event, payload } }) => {
    const action = eventActionMap[event];

    if (!action) {
      console.log(`Unhandled event: ${event}, payload: `, payload);
      return;
    }

    dispatch(action(payload));
  };

const handleConnectionClosed = (dispatch) => () => {
  dispatch(setConnected(false));
  dispatch(setError(true));
};

const handleReconnecting = (dispatch) => () => {
  dispatch(setLoading(true));
  dispatch(setConnected(false));
};

const handleReconnected = (dispatch) => () => {
  dispatchEvent(setLoading(false));
  dispatchEvent(setConnected(true));
};
