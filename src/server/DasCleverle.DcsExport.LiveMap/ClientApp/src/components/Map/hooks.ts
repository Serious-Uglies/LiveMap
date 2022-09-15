import { useState, useEffect, Dispatch } from 'react';
import { ViewState } from 'react-map-gl';
import { useAppSelector } from '../../store';

const theatres: { [theatre: string]: { viewState: Partial<ViewState> } } = {
  Caucasus: {
    viewState: {
      latitude: 43.12073409583715,
      longitude: 40.72214289492385,
      zoom: 6.69,
    },
  },
  MarianaIslands: {
    viewState: {
      latitude: 13.924587056525029,
      longitude: 142.35254586809404,
      zoom: 6.37,
    },
  },
  Nevada: {
    viewState: {
      latitude: 37.959029700616,
      longitude: -115.3220498166852,
      zoom: 6.66,
    },
  },
  PersianGulf: {
    viewState: {
      latitude: 26.872559655101398,
      longitude: 55.9127513841311,
      zoom: 6.04,
    },
  },
  Syria: {
    viewState: {
      latitude: 34.890062119982204,
      longitude: 37.71425723879233,
      zoom: 6.37,
    },
  },
};

export function useViewState(): [ViewState, Dispatch<ViewState>] {
  const theatre = useAppSelector((state) =>
    state.liveState.theatre ? theatres[state.liveState.theatre] : null
  );
  const [viewState, setViewState] = useState<ViewState>({
    latitude: 0,
    longitude: 0,
    zoom: 20,
    bearing: 0,
    pitch: 0,
    padding: { top: 0, right: 0, bottom: 0, left: 0 },
  });

  useEffect(() => {
    if (!theatre) {
      return;
    }

    setViewState((s) => ({
      ...s,
      ...theatre.viewState,
    }));
  }, [theatre]);

  return [viewState, setViewState];
}
