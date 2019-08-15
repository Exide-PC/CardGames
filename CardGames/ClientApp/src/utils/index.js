export const serializeObject = obj => {
  let result = '?';
  console.log(obj);
  Object.entries(obj).forEach(([key, value]) => {
    result = `${result}${encodeURIComponent(key)}=${encodeURIComponent(
      value
    )}&`;
  });
  result = result.substring(0, result.length - 1);
  return result;
};
