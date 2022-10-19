module.exports = {
  babel: {
    loaderOptions: (options, { env }) => {
      if (env !== 'production') {
        return options;
      }

      const mapboxGLExclude = './node_modules/mapbox-gl/dist/mapbox-gl.js';

      options.exclude = options.exclude
        ? [...options.exclude, mapboxGLExclude]
        : [mapboxGLExclude];

      return options;
    },
  },
};
