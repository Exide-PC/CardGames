module.exports = {
  plugins: [
    require('postcss-import'),
    require('postcss-preset-env')({
      stage: 0,
      importFrom: './src/base.css'
    }),
    require('postcss-nested')
  ]
};
