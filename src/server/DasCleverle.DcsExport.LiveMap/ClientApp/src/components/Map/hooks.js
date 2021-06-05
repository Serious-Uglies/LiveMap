import { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';

const theatres = {
  Caucasus: {
    viewport: {
      latitude: 43.12073409583715,
      longitude: 40.72214289492385,
      zoom: 6.69,
    },
    timezone: 'Asia/Tbilisi',
  },
  Nevada: {
    viewport: {
      latitude: -115.3220498166852,
      longitude: 37.959029700616,
      zoom: 6.66,
    },
    timezone: 'America/Los_Angeles',
  },
  PersianGulf: {
    viewport: {
      latitude: 55.9127513841311,
      longitude: 26.872559655101398,
      zoom: 6.04,
    },
    timezone: 'Asia/Dubai',
  },
  Syria: {
    viewport: {
      latitude: 37.71425723879233,
      longitude: 34.890062119982204,
      zoom: 6.37,
    },
    timezone: 'Asia/Damascus',
  },
};

export function useViewport() {
  const theatre = useSelector((state) => theatres[state.liveState.theatre]);
  const [viewport, setViewport] = useState({
    latitude: 0,
    longitude: 0,
    zoom: 20,
  });

  useEffect(() => {
    if (!theatre) {
      return;
    }

    setViewport((vp) => ({
      ...vp,
      ...theatre.viewport,
    }));
  }, [theatre]);

  return [viewport, setViewport];
}
