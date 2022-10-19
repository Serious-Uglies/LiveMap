const stringIdentity = (v: any, lng: string) => String(v);

export default function join(value: any, lng: string, map = stringIdentity) {
  return value.map(map).join(', ');
}
