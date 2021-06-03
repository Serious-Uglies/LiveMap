const stateEndpoint = '/api/state';

export async function getLiveState() {
  try {
    return (await fetch(stateEndpoint).then((res) => res.json())) || {};
  } catch {
    return null;
  }
}
