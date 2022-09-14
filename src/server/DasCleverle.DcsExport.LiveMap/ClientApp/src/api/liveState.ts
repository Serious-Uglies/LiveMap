import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { AnyAction, Dispatch } from '@reduxjs/toolkit';
import { RootState } from '../store';
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
import { InitPayload } from '../store/types';

type EventType =
  | 'Init'
  | 'MissionEnd'
  | 'Time'
  | 'AddObject'
  | 'UpdateObject'
  | 'RemoveObject'
  | 'AddAirbase';

interface Event<T = any> {
  event: EventType;
  payload: T;
}

const stateEndpoint = '/api/state';
const hubEndpoint = '/hub/livemap';

const eventActionMap: {
  [key: string]: (payload: Array<any>) => AnyAction;
} = {
  Init: (payload: InitPayload[]) => init({ ...payload[0], isRunning: true }),
  MissionEnd: end,
  Time: updateTime,
  AddObject: addObject,
  UpdateObject: updateObject,
  RemoveObject: removeObject,
  AddAirbase: addAirbase,
};

export const connect =
  () => async (dispatch: Dispatch, getState: () => RootState) => {
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
    return await fetch(stateEndpoint).then((res) => res.json());
  } catch (err) {
    console.log(err);
    return null;
  }
};

const handleEvent =
  (dispatch: Dispatch) =>
  ({ events }: { events: Event[] }) => {
    const perEvent = events.reduce(
      (prev: { [event: string]: any[] }, { event, payload }) => {
        let payloads = prev[event];

        if (!payloads) {
          payloads = [];
          prev[event] = payloads;
        }

        payloads.push(payload);

        return prev;
      },
      {}
    );

    for (let [event, payloads] of Object.entries(perEvent)) {
      const action = eventActionMap[event];

      if (!action) {
        console.log(`Unhandled event: ${event}, payload: `, payloads);
        return;
      }

      dispatch(action(payloads));
    }
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
