import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import {
  addAirbase,
  addObject,
  end,
  init,
  removeObject,
  setPhase,
  updateObject,
  updateTime,
} from '../store/liveState';

const stateEndpoint = '/api/state';
const hubEndpoint = '/hub/livemap';

const eventActionMap = {
  Init: (payload) => init({ ...payload[0], isRunning: true }),
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

  dispatch(setPhase('loading'));

  const liveState = await getLiveState(dispatch);

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
  ({ events }) => {
    const perEvent = events.reduce((prev, { event, payload }) => {
      let payloads = prev[event];

      if (!payloads) {
        payloads = [];
        prev[event] = payloads;
      }

      payloads.push(payload);

      return prev;
    }, {});

    for (let [event, payloads] of Object.entries(perEvent)) {
      const action = eventActionMap[event];

      if (!action) {
        console.log(`Unhandled event: ${event}, payload: `, payloads);
        return;
      }

      dispatch(action(payloads));
    }
  };

const handleConnectionClosed = (dispatch) => () => {
  dispatch(setPhase('error'));
};

const handleReconnecting = (dispatch) => () => {
  dispatch(setPhase('reconnecting'));
};

const handleReconnected = (dispatch) => () => {
  dispatch(setPhase('loaded'));
};
