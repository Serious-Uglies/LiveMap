const identity = (v) => v;

export default function join(value, map = identity) {
  return value.map(map).join(', ');
}
